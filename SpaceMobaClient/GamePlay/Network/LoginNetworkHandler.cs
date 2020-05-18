// System libraries
using System;
using System.Diagnostics;
using System.Threading;

// Networking libraries
using Lidgren.Network;

// Game libraries
using SpaceMobaClient.GamePlay.Components;
using SpaceMobaClient.GamePlay.Scenes;
using SpaceMobaClient.Systems.Objects;
using SpaceMobaClient.Systems.Network;
using SpaceMobaClient.Systems.Scenes;


namespace SpaceMobaClient.GamePlay.Network
{
    public class LoginNetworkHandler
    {
        private NetClient Connection;

        /// <summary>
        /// Attempts to connect to login server.
        /// </summary>
        public void Connect()
        {
            // Init GameServer
            Connection = new NetClient(
                new NetPeerConfiguration(Settings.AppIdentifier)
                );
            Connection.Start();

            // Begin connection
            NetOutgoingMessage hailMessage = Connection.CreateMessage();
            hailMessage.Write("hello");

            NetConnection connection = Connection.Connect(
                Settings.LoginHost, Settings.LoginPort, hailMessage
                );

            // Wait for connection
            try
            {
                WaitForConnection(connection);
            }
            catch (Exception e)
            {
                throw (e);
            }
        }

        /// <summary>
        /// Waits for a given connection to complete connecting.
        /// </summary>
        /// <param name="connection">Pending connection.</param>
        private void WaitForConnection(NetConnection connection)
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
        /// Checks for any incoming messages, and processes them.
        /// </summary>
        public void Update()
        {
            if(Connection != null)
            {
                if(
                    Connection.ConnectionStatus == 
                    NetConnectionStatus.Connected
                    )
                {
                    NetIncomingMessage msg = Connection.ReadMessage();
                    if (msg != null)
                    {
                        switch (msg.MessageType)
                        {
                            case NetIncomingMessageType.Data:
                                HandleData(msg);
                                break;

                            case NetIncomingMessageType.StatusChanged:
                                HandleStatusChange(
                                    msg.SenderConnection.Status
                                    );
                                break;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Handles incoming data.
        /// </summary>
        /// <param name="msg">Incoming message.</param>
        public void HandleData(NetIncomingMessage msg)
        {
            if(msg.ReadByte() == 1)
            {
                int port = msg.ReadInt32();
                int token = msg.ReadInt32();

                Thread.Sleep(2000);
                SceneManager.GotoScene<LoadGameScene>(
                    new object[] { Settings.LoginHost, port, token }
                    );

                Connection.Disconnect("bye");
            }
        }

        public void HandleStatusChange(NetConnectionStatus status)
        {
        }
    }
}
