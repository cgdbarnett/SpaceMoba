using System;
using System.Diagnostics;
using System.Threading.Tasks;

using Lidgren.Network;

namespace LoginServer.Networking
{
    /// <summary>
    /// Provides networking management to the remote clients.
    /// </summary>
    public class NetworkManager
    {
        // Server NetPeer.
        private readonly NetServer Server;

        /// <summary>
        /// Triggers when a new client connects to the server.
        /// </summary>
        public event EventHandler<object> OnClientConnect;

        /// <summary>
        /// Triggers when a client disconnects from the server.
        /// </summary>
        public event EventHandler<object> OnClientDisconnect;
        
        /// <summary>
        /// Triggers when a client receives a message.
        /// </summary>
        public event EventHandler<object> OnClientMessageReceived;

        /// <summary>
        /// Create an instance of NetworkManager. This will start
        /// listening on the port specified in Settings.
        /// </summary>
        public NetworkManager()
        {
            Trace.WriteLine("Starting Network.");
            Trace.Indent();

            try
            {
                // Config:
                NetPeerConfiguration serverConfig =
                    new NetPeerConfiguration(Settings.AppIdentifier)
                    {
                        Port = Settings.ListenPort,
                        MaximumConnections = Settings.MaximumConnections,
                        AcceptIncomingConnections = true
                    };
                serverConfig.EnableMessageType(
                    NetIncomingMessageType.ConnectionApproval
                    );

                // Initiate and start server.
                Server = new NetServer(serverConfig);
                Server.Start();

                // Start a new task to handle incoming messages.
                Task.Run(() => Update());
            }
            catch(Exception e)
            {
                Trace.WriteLine("Exception in NetworkManager():");
                Trace.WriteLine(e.ToString());
            }

            Trace.Unindent();
            Trace.WriteLine("End Network.");
        }

        /// <summary>
        /// Loops indefinitely, waiting for incoming messages, then
        /// processing them.
        /// </summary>
        public void Update()
        {
            while (true)
            {
                // Wait for incoming message
                Server.MessageReceivedEvent.WaitOne();

                // Read message, handle, and loop
                try
                {
                    NetIncomingMessage msg = Server.ReadMessage();
                    HandleIncomingMessage(msg);
                }
                catch (Exception e)
                {
                    Trace.WriteLine("Exception in NetworkManager.Update():");
                    Trace.WriteLine(e.ToString());
                }
            }
        }

        /// <summary>
        /// Handle an incoming message from a remote client.
        /// </summary>
        /// <param name="msg">Incoming message.</param>
        private void HandleIncomingMessage(NetIncomingMessage msg)
        {
            switch(msg.MessageType)
            {
                case NetIncomingMessageType.ConnectionApproval:
                    HandleIncomingConnection(msg);
                    break;

                case NetIncomingMessageType.StatusChanged:
                    switch(msg.SenderConnection.Status)
                    {
                        case NetConnectionStatus.Disconnected:
                            if(OnClientDisconnect != null)
                            {
                                OnClientDisconnect.Invoke(
                                    msg.SenderConnection, null);
                            }
                            break;

                        case NetConnectionStatus.Connected:
                            if(OnClientConnect != null)
                            {
                                OnClientConnect.Invoke(
                                    msg.SenderConnection, null
                                    );
                            }
                            break;
                    }
                    break;

                case NetIncomingMessageType.Data:
                    if(OnClientMessageReceived != null)
                    {
                        OnClientMessageReceived.Invoke(
                            msg.SenderConnection, msg
                            );
                    }
                    break;
            }
        }

        /// <summary>
        /// Handles approving (or denying) an incoming connection.
        /// </summary>
        /// <param name="msg">Incoming message.</param>
        private void HandleIncomingConnection(NetIncomingMessage msg)
        {
            // Temporary: Accept all.
            // Todo: Link to database to approve client credentials.

            msg.SenderConnection.Approve();
        }
    }
}
