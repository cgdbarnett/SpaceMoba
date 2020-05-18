using System;
using System.Collections.Generic;
using System.Diagnostics;

using Lidgren.Network;

using LoginServer.Instances;
using LoginServer.Networking;

namespace LoginServer
{
    /// <summary>
    /// Runs the login server.
    /// </summary>
    public class LoginServerManager
    {
        // Local state.
        private readonly InstancePool InstancePool;
        private readonly Queue<Client> ClientsQueuedForGame;
        private readonly NetworkManager NetworkManager;

        /// <summary>
        /// Returns whether the server is still active.
        /// </summary>
        public bool Active
        {
            get;
            private set;
        }

        /// <summary>
        /// Creates an instance of a LoginServerManager.
        /// </summary>
        public LoginServerManager()
        {
            Active = true;

            // Init instance pools
            Trace.WriteLine("Init instance pools.");
            Trace.Indent();

            InstancePool = new InstancePool();

            Trace.Unindent();
            Trace.WriteLine("End instance pools.");

            // Init datastructures
            ClientsQueuedForGame = new Queue<Client>();

            // Init Networking
            NetworkManager = new NetworkManager();
            NetworkManager.OnClientConnect += HandleClientConnect;
        }

        /// <summary>
        /// Runs an iteration of the server.
        /// </summary>
        public void Run()
        {
            int teamSize = Settings.MaxTeamSize * 2;

            // If enough clients are in queue for 2 teams,
            // and a game is available, start a game and 
            // instruct the clients to join.
            if(
                ClientsQueuedForGame.Count >= teamSize
                && InstancePool.GameAvailable()
                )
            {
                StartGame();
            }
        }

        /// <summary>
        /// Starts a game, and sends a packet to the relevant clients
        /// to join the game.
        /// </summary>
        private void StartGame()
        {
            Trace.WriteLine("Starting game...");
            int teamSize = Settings.MaxTeamSize * 2;

            Client[] clients = new Client[teamSize];
            int[] tokens = new int[teamSize];
            for (int i = 0; i < teamSize; i++)
            {
                clients[i] = ClientsQueuedForGame.Dequeue();
                tokens[i] = clients[i].Token;
            }
            int port = InstancePool.StartGame(tokens);

            // Send join packet to clients.
            for(int i = 0; i < teamSize; i++)
            {
                clients[i].Connection.SendMessage(
                    PacketWriter.JoinGame(
                        clients[i].Connection, port, tokens[i]
                        ),
                    NetDeliveryMethod.ReliableOrdered,
                    0
                    );
            }
        }

        /// <summary>
        /// Handles a connecting client.
        /// </summary>
        /// <param name="sender">NetConnection of client.</param>
        /// <param name="state">Null.</param>
        private void HandleClientConnect(object sender, object state)
        {
            Trace.WriteLine("Client connected.");
            Client client = new Client()
            {
                Connection = (NetConnection)sender
            };

            ClientsQueuedForGame.Enqueue(client);
            Trace.WriteLine(ClientsQueuedForGame.Count.ToString() + "/" +
                (Settings.MaxTeamSize * 2).ToString() + " in queue.");
        }
    }
}
