// System libaries
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

// Lidgren libraries
using Lidgren.Network;

// Game libraries
using GameInstanceServer.Game;
using GameInstanceServer.Game.Objects;
using GameInstanceServer.Map;
using GameInstanceServer.Systems;

namespace GameInstanceServer
{
    /// <summary>
    /// Enum of possible server states.
    /// </summary>
    public enum ServerManagerState
    {
        Init,
        WaitingForClients,
        Countdown,
        InGame,
        Closing,
        Shutdown
    }

    /// <summary>
    /// Manages the game server.
    /// </summary>
    public class ServerManager
    {
        // Id used for lidgren to identify game.
        private const string LidgrenAppId = "smc20";

        // Maps the token provided by clients to the client itself.
        // (These are provided by the matchmaking server).
        private Dictionary<int, Client> Clients;

        // Reference to gamesimulation
        private GameSimulation GameSimulation;

        // Networking
        private NetServer NetServer;

        // State of game server
        private ServerManagerState State = ServerManagerState.Init;
        private Stopwatch Timer;

        /// <summary>
        /// Returns whether the server is currently active.
        /// </summary>
        public bool Running
        {
            get
            {
                return State != ServerManagerState.Shutdown;
            }
        }

        /// <summary>
        /// Creates a new server manager, which will listen for clients
        /// on a given port.
        /// </summary>
        /// <param name="port">Port to listen on.</param>
        /// <param name="tokens">User tokens for connection.</param>
        public ServerManager(int port, int[] tokens)
        {
            State = ServerManagerState.Init;
            GameSimulation = GameSimulation.GetGameSimulation();

            InitNetworking(port);
            InitClients(tokens);

            Timer = new Stopwatch();
            Timer.Start();

            State = ServerManagerState.WaitingForClients;
        }

        /// <summary>
        /// Runs a frame of the game server.
        /// </summary>
        public void Run()
        {
            switch(State)
            {
                // Should never call Run() while the state is in Init.
                case ServerManagerState.Init:
                    throw (new InvalidOperationException());

                // Handle the waiting period for clients to join.
                case ServerManagerState.WaitingForClients:
                    DoWaitingForClients();
                    break;

                // Handle the countdown until the game starts.
                case ServerManagerState.Countdown:
                    DoCountdown();
                    break;

                // Handle the normal game state.
                case ServerManagerState.InGame:
                    DoInGame();
                    break;
                
                // Handle end of game
                case ServerManagerState.Closing:
                    GameSimulation.Stop();
                    State = ServerManagerState.Shutdown;
                    break;
                
                // Generally this event won't be triggered.
                case ServerManagerState.Shutdown:
                    break;
            }

            Thread.Sleep(15);
        }

        /// <summary>
        /// Initialises the NetServer.
        /// </summary>
        /// <param name="port">Port to listen to.</param>
        private void InitNetworking(int port)
        {
            Trace.WriteLine("Initialising Networking.");
            Trace.IndentLevel++;

            // Configure server
            NetPeerConfiguration config = 
                new NetPeerConfiguration(LidgrenAppId)
            {
                Port = port
            };
            config.EnableMessageType(NetIncomingMessageType.ConnectionApproval);

            // Start server
            NetServer = new NetServer(config);
            NetServer.Start();

            Trace.IndentLevel--;
            Trace.WriteLine("End Networking.");
        }

        /// <summary>
        /// Initialises the client map.
        /// </summary>
        /// <param name="tokens"></param>
        private void InitClients(int[] tokens)
        {
            Trace.WriteLine("Initialising Clients.");
            Trace.IndentLevel++;

            if (tokens.Length != 6)
            {
                throw (new ArgumentException());
            }

            Clients = new Dictionary<int, Client>();
            for (int i = 0; i < 6; i++)
            {
                // Create token representing expected team member
                Clients[tokens[i]] = new Client()
                {
                    Id = tokens[i],
                    Team = i < 3 ? (byte)0 : (byte)1,
                    Active = false,
                    IsReady = false,
                    NetPeer = null
                };
            }

            Trace.IndentLevel--;
            Trace.WriteLine("End Clients.");
        }

        /// <summary>
        /// Reads an incoming message, and determines whether we can approve
        /// the new connection.
        /// </summary>
        /// <param name="msg">Incoming message.</param>
        private void ApproveIncomingConnection(NetIncomingMessage msg)
        {
            Trace.WriteLine("Incoming Connection.");
            Trace.Indent();

            int token = msg.ReadInt32();

            // Try find token in Tokens
            if (Clients.ContainsKey(token))
            {
                Client client = Clients[token];
                // Make sure client hasn't already connected
                if (client.Active)
                {
                    Trace.WriteLine("Denied attempted connection.");
                    msg.SenderConnection.Deny();
                }
                else
                {
                    // Approve on server
                    msg.SenderConnection.Approve();
                    client.Active = true;
                    client.NetPeer = msg.SenderConnection;
                    Trace.WriteLine("Approved client.");

                    // Spawn object
                    MapData.GetMapData().SpawnPlayerObject(client);
                }
            }
            else
            {
                Trace.WriteLine("Denied attempted connection.");
                msg.SenderConnection.Deny();
            }

            Trace.Unindent();
            Trace.WriteLine("End Connection.");
        }

        /// <summary>
        /// Runs one iteration of the waiting for client loop.
        /// </summary>
        /// <remarks>
        /// Opportunity to improve this by asynchronously appending messages to
        /// the list, and to asynchronously clearing the list. A linked list
        /// would probably be preferred for such a setup.
        /// </remarks>
        private void DoWaitingForClients()
        {
            List<NetIncomingMessage> messages = new List<NetIncomingMessage>();
            int token;

            NetServer.ReadMessages(messages);

            // Iterate through all received messages
            foreach(NetIncomingMessage msg in messages)
            {
                switch(msg.MessageType)
                {
                    // Attempt to approve incoming connection
                    case NetIncomingMessageType.ConnectionApproval:
                        ApproveIncomingConnection(msg);
                        break;

                    // Handle incoming data, expecting ClientIsReady packet.
                    case NetIncomingMessageType.Data:
                        token = msg.ReadInt32();
                        Client client = Clients[token];
                        NetOpCode opcode = (NetOpCode)msg.ReadInt16();

                        switch(opcode)
                        {
                            // Client is ready to start the game
                            case NetOpCode.ClientIsReady:
                                {
                                    Trace.WriteLine("Client is ready.");
                                    if (msg.ReadBoolean())
                                    {
                                        client.IsReady = true;
                                    }
                                    break;
                                }

                            // Client is requesting the Id of the locally
                            // assigned object
                            case NetOpCode.RequestLocalObject:
                                {
                                    Trace.WriteLine("Client requested "
                                        + "LocalObject.");

                                    // Assign local player object
                                    NetOutgoingMessage assignLocalObject =
                                    PacketWriter.AssignLocalPlayerObject(
                                        client.NetPeer,
                                        client.GameObject.GetId()
                                        );
                                    NetServer.SendMessage(
                                        assignLocalObject,
                                        client.NetPeer,
                                        NetDeliveryMethod.ReliableOrdered
                                        );
                                    Trace.WriteLine("Assigned LocalObject.");
                                    break;
                                }

                            default:
                                // Write a helpful error message.
                                Trace.WriteLine("Unexpected data received.");
                                break;
                        }
                        break;
                }
            }

            // Check if all clients are ready, or we have timed out:
            if (Timer.Elapsed.TotalSeconds < 30)
            {
                bool ready = true;
                foreach(Client client in Clients.Values)
                {
                    if(!client.IsReady)
                    {
                        ready = false;
                        break;
                    }
                }

                if(ready)
                {
                    StartCountdown();
                }
            }
            else
            {
                StartCountdown();
            }
        }

        /// <summary>
        /// Transition state into countdown.
        /// </summary>
        private void StartCountdown()
        {
            Trace.WriteLine("Starting Countdown.");

            Timer.Reset();
            Timer.Start();
            State = ServerManagerState.Countdown;

            // Create and send Start packet to all clients
            foreach (Client client in Clients.Values)
            {
                if (client.Active)
                {
                    NetOutgoingMessage msg =
                        PacketWriter.GameStartCountdown(client.NetPeer, 6000);
                    NetServer.SendMessage(
                        msg, client.NetPeer, NetDeliveryMethod.ReliableOrdered
                        );
                }
            }

            // Create welcome packets for all clients, and send
            // Done in a separate loop so that all clients start countdown at
            // as similar time as possible.
            foreach(Client client in Clients.Values)
            {
                if(client.Active)
                {
                    NetOutgoingMessage msg =
                        PacketWriter.WelcomePacket(client.NetPeer, client);
                    NetServer.SendMessage(
                        msg, client.NetPeer, NetDeliveryMethod.ReliableOrdered
                        );
                }
            }
        }

        /// <summary>
        /// Runs a single iteration of the countdown loop.
        /// </summary>
        private void DoCountdown()
        {
            if(Timer.ElapsedMilliseconds >= 6000)
            {
                StartInGame();
            }
        }

        /// <summary>
        /// Transitions state to in game.
        /// </summary>
        private void StartInGame()
        {
            Trace.WriteLine("Starting Game.");

            // Starts the game
            State = ServerManagerState.InGame;
            GameSimulation.Start();
            Timer.Restart();
        }

        /// <summary>
        /// Runs a single iteration of in game state.
        /// </summary>
        private void DoInGame()
        {
            List<NetIncomingMessage> messages = new List<NetIncomingMessage>();

            NetServer.ReadMessages(messages);

            // Iterate through all received messages
            foreach (NetIncomingMessage msg in messages)
            {
                switch (msg.MessageType)
                {
                    // Attempt to approve incoming connection
                    // (For reconnecting).
                    case NetIncomingMessageType.ConnectionApproval:
                        ApproveIncomingConnection(msg);
                        break;

                    // Incoming data
                    case NetIncomingMessageType.Data:
                        HandlePacket(msg);
                        break;
                }
            }
        }

        /// <summary>
        /// Handles an incoming packet of inputs.
        /// </summary>
        /// <param name="msg">Incoming packet</param>
        private void HandlePacket(NetIncomingMessage msg)
        {
            int token = msg.ReadInt32();
            NetOpCode opcode = (NetOpCode)msg.ReadInt16();

            switch(opcode)
            {
                default:
                    Trace.WriteLine("Received unexpected opcode.");
                    break;
            }
        }
    }
}
