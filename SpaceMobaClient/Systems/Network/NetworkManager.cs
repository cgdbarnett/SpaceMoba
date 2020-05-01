using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

using Lidgren.Network;

namespace SpaceMobaClient.Systems.Network
{
    /// <summary>
    /// Manages connections to servers.
    /// </summary>
    public static class NetworkManager
    {
        /// <summary>
        /// GUID for app. (Currently just some test string).
        /// </summary>
        private const string AppIdentifier = "smc20";

        /// <summary>
        /// NetClient connection to server.
        /// </summary>
        public static NetClient Connection { get; private set; }

        /// <summary>
        /// Gets whether the network manager is currently connected.
        /// </summary>
        public static bool IsConnected
        {
            get
            {
                if(Connection != null)
                {
                    return Connection.ConnectionStatus == 
                        NetConnectionStatus.Connected;
                }
                return false;
            }
        }

        // Current input handler.
        private static INetworkInputHandler InputHandler;

        /// <summary>
        /// Connect to remote server. This is a blocking
        /// operation. Can be used for either Game or Lobby
        /// servers, as long as appropriate token is provided.
        /// </summary>
        /// <param name="host">IP address of server.</param>
        /// <param name="port">Port number of server.</param>
        /// <param name="token">Token id for client.</param>
        public static void Connect(
            string host, int port, int token, INetworkInputHandler handler
            )
        {
            InputHandler = handler;

            // Init GameServer
            Connection = new NetClient(
                new NetPeerConfiguration(AppIdentifier)
                );
            Connection.Start();

            // Begin connection
            NetOutgoingMessage hailMessage = Connection.CreateMessage();
            hailMessage.Write(token);
            NetConnection connection = Connection.Connect(
                host, port, hailMessage
                );

            // Wait for connection
            try
            {
                WaitForConnection(connection);
            }
            catch(Exception e)
            {
                throw (e);
            }
        }

        /// <summary>
        /// Disconnects, and clears up resources used for the connection to the
        /// game server. This is a blocking call.
        /// </summary>
        public static void Disconnect()
        {
            Connection.Shutdown("bye");

            // Wait for disconnect
            try
            {
                WaitForDisconnection();
            }
            catch(Exception e)
            {
                Trace.WriteLine("Exception in NetworkManager.Disconnect():");
                Trace.WriteLine(e.ToString());
            }
            finally
            {
                Connection = null;
                InputHandler = null;
            }
        }

        /// <summary>
        /// Waits for the client to disconnect.
        /// </summary>
        private static void WaitForDisconnection()
        {
            while(
                Connection.ConnectionStatus != NetConnectionStatus.Disconnected
                )
            {
                Thread.Sleep(10);
            }
        }

        /// <summary>
        /// Waits for a given connection to complete connecting.
        /// </summary>
        /// <param name="connection">Pending connection.</param>
        private static void WaitForConnection(NetConnection connection)
        {
            // Wait for connection to end.
            bool approved = false;
            while (!approved)
            {
                NetIncomingMessage approvalMessage = Connection.ReadMessage();
                if (approvalMessage != null)
                {
                    switch (approvalMessage.MessageType)
                    {
                        case NetIncomingMessageType.Data:
                            Trace.WriteLine("Unexpected data received.");
                            break;

                        case NetIncomingMessageType.StatusChanged:
                            switch (connection.Status)
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
        /// Handles incoming messages from the game server.
        /// </summary>
        public static void HandleIncomingMessages()
        {
            List<NetIncomingMessage> messages = new List<NetIncomingMessage>();
            Connection.ReadMessages(messages);

            foreach (NetIncomingMessage msg in messages)
            {
                try
                {
                    switch (msg.MessageType)
                    {
                        case NetIncomingMessageType.StatusChanged:
                            InputHandler.HandleStatusChange(
                                Connection.ConnectionStatus
                                );
                            break;

                        case NetIncomingMessageType.Data:
                            InputHandler.HandleData(msg);
                            break;
                    }
                }
                catch (Exception e)
                {
                    Trace.WriteLine("Error occured in NetworkManager."
                        + "HandleIncomingMessages():");
                    Trace.WriteLine(e);
                }
            }
        }
    }
}
