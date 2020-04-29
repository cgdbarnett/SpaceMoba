using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using Lidgren.Network;

using SpaceMobaClient.GamePlay.Components;
using SpaceMobaClient.GamePlay.Objects;
using SpaceMobaClient.Systems.IO;
using SpaceMobaClient.Systems.Objects;

namespace SpaceMobaClient.Systems.Server
{
    /// <summary>
    /// A class that handles communicating with the remote game server.
    /// </summary>
    public class RemoteServer : IRemoteServer
    {
        // Reference to match maker
        private readonly MatchmakerServer MatchMaker;

        // Lidgren netclient object that handles communications with
        // remote server.
        private NetClient Client;

        // Event handlers for generic events occuring on server.
        public event IntEventHandler OnAssignToLocalPlayer;
        public event IntEventHandler OnGameStart;

        /// <summary>
        /// Creates an instance of the remote server, and starts the
        /// lidgren client.
        /// </summary>
        public RemoteServer()
        {
            MatchMaker = MatchmakerServer.GetMatchmakerServer();

            Client = new NetClient(new NetPeerConfiguration("smc20"));
            Client.Start();
        }

        /// <summary>
        /// Connect to remote server.
        /// </summary>
        /// <param name="host">IP address of server.</param>
        /// <param name="port">Port number of server.</param>
        /// <param name="token">Token id for client.</param>
        public void Connect(string host, int port, int token)
        {
            NetOutgoingMessage hailMessage = Client.CreateMessage();
            hailMessage.Write(token);
            NetConnection connection = Client.Connect(host, port, hailMessage);

            // Wait for connection to end. (This is running asynchronously to
            // main thread).
            bool approved = false;
            while(!approved)
            {
                NetIncomingMessage approvalMessage = Client.ReadMessage();
                if(approvalMessage != null)
                {
                    switch(approvalMessage.MessageType)
                    {
                        case NetIncomingMessageType.Data:
                            Trace.WriteLine("Unexpected data received.");
                            break;

                        case NetIncomingMessageType.StatusChanged:
                            switch(connection.Status)
                            {
                                case NetConnectionStatus.Connected:
                                    approved = true;
                                    break;

                                    // When these events occur, we have either
                                    // entered an error state, or been denied.
                                case NetConnectionStatus.Disconnected:
                                case NetConnectionStatus.Disconnecting:
                                case NetConnectionStatus.None:
                                    Trace.WriteLine("Disconnected.");
                                    throw (new NetException());
                            }
                            break;
                    }
                }

                Thread.Sleep(10);
            }
        }

        /// <summary>
        /// Sends a message to the game server to let it know the client
        /// has loaded resources and is ready to start.
        /// </summary>
        public void ClientIsReady()
        {
            NetOutgoingMessage packet = Client.CreateMessage();
            packet.Write((short)NetOpCode.ClientIsReady);

            Client.SendMessage(packet, NetDeliveryMethod.ReliableOrdered);
        }

        /// <summary>
        /// Parses the InputState, and sends it to the RemoteServer.
        /// </summary>
        /// <param name="input">Current input state.</param>
        public void UpdateInput(InputState input)
        {
            // Packet structure:
            // token (int)
            // forward (byte) 0 = backward, 1 = none, 2 = forward
            // sideways (byte) 0 = left, 1 = none, 2 = right

            NetOutgoingMessage msg = Client.CreateMessage();
            msg.Write((short)NetOpCode.UpdatePlayerInput);

            byte xx = 1;
            byte yy = 1;
            if (input.Forward) xx++;
            if (input.Backward) xx--;
            if (input.Left) yy--;
            if (input.Right) yy++;

            msg.Write(xx);
            msg.Write(yy);
            msg.Write(input.Attack);

            Client.SendMessage(msg, NetDeliveryMethod.UnreliableSequenced);
        }

        /// <summary>
        /// Handles incoming messages from the remote server and generates
        /// relevant events.
        /// </summary>
        public void HandleIncomingMessages()
        {
            List<NetIncomingMessage> messages = new List<NetIncomingMessage>();
            Client.ReadMessages(messages);

            foreach(NetIncomingMessage msg in messages)
            {
                try
                {
                    switch (msg.MessageType)
                    {
                        case NetIncomingMessageType.StatusChanged:
                            HandleStatusChanged();
                            break;

                        case NetIncomingMessageType.Data:
                            HandleData(msg);
                            break;
                    }
                }
                catch(Exception e)
                {
                    Trace.WriteLine("Error occured in RemoteServer."
                        + "HandleIncomingMessages():");
                    Trace.WriteLine(e);
                }
            }
        }

        /// <summary>
        /// Handles a change in status.
        /// </summary>
        /// <remarks>
        /// Todo: Determine if we should try reconnect, or exit game.
        /// </remarks>
        private void HandleStatusChanged()
        {
            Trace.WriteLine("Status changed:");
            Trace.Indent();

            switch (Client.ConnectionStatus)
            {
                case NetConnectionStatus.Connected:
                    Trace.WriteLine("Connected.");
                    break;

                case NetConnectionStatus.Disconnected:
                case NetConnectionStatus.Disconnecting:
                    Trace.WriteLine("Disconnected.");
                    GameClient.GetGameClient().Exit();
                    break;
            }

            Trace.Unindent();
            Trace.WriteLine("End status.");
        }

        /// <summary>
        /// Handles data messages from the remote server.
        /// </summary>
        /// <param name="msg">Incoming message to handle.</param>
        private void HandleData(NetIncomingMessage msg)
        {
            NetOpCode opcode = (NetOpCode)msg.ReadInt16();

            switch(opcode)
            {
                // Handle the Game Start command.
                case NetOpCode.StartGameCountdown:
                    OnGameStart(msg.ReadInt32());
                    break;

                case NetOpCode.AssignLocalObject:
                    OnAssignToLocalPlayer(msg.ReadInt32());
                    break;

                case NetOpCode.WelcomePacket:
                    {
                        Trace.WriteLine("Welcome packet received.");
                        int localPlayer = msg.ReadInt32();

                        // Create objects
                        int count = msg.ReadInt16();
                        for(int i = 0; i < count; i++)
                        {
                            EntityManager.CreateEntityFromMessage(msg);
                        }

                        // Assign local player
                        try
                        {
                            OnAssignToLocalPlayer(localPlayer);
                        }
                        catch
                        {
                        }
                        break;
                    }
                
                // Update an object
                case NetOpCode.UpdateObject:
                    EntityManager.UpdateEntityFromMessage(msg);
                    break;
                
                // Create an object
                case NetOpCode.CreateObject:
                    EntityManager.CreateEntityFromMessage(msg);
                    break;

                // Destroy an object
                case NetOpCode.DestroyObject:
                    EntityManager.Remove(msg.ReadInt32());
                    break;

                default:
                    Trace.WriteLine("Unexpected opcode.");
                    break;
            }
        }
    }
}
