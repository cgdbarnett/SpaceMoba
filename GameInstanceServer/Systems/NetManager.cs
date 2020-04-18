using System;
using System.Collections.Generic;
using System.Text;

using Lidgren.Network;

namespace GameInstanceServer.Systems
{
    /// <summary>
    /// Manages the networking for the server.
    /// </summary>
    public class NetManager
    {
        private NetServer NetServer;

        /// <summary>
        /// Initiates the NetServer.
        /// </summary>
        /// <param name="port"></param>
        public NetManager(int port)
        {
            // Configure server
            NetPeerConfiguration config = new NetPeerConfiguration("smc20")
            {
                Port = port
            };
            config.EnableMessageType(NetIncomingMessageType.ConnectionApproval);

            // Start server
            NetServer = new NetServer(config);
            NetServer.Start();
        }

        /// <summary>
        /// Returns a list of the current incoming messages in queue.
        /// </summary>
        /// <returns>A list of incoming messages.</returns>
        public List<NetIncomingMessage> GetIncomingMessages()
        {
            List<NetIncomingMessage> messages = new List<NetIncomingMessage>();
            NetIncomingMessage msg;
            while((msg = NetServer.ReadMessage()) != null)
            {
                messages.Add(msg);
            }

            return messages;
        }
    }
}
