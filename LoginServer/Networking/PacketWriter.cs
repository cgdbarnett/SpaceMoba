using System;
using System.Collections.Generic;
using System.Text;

using Lidgren.Network;

namespace LoginServer.Networking
{
    /// <summary>
    /// Writes packets to send to clients.
    /// </summary>
    public static class PacketWriter
    {
        /// <summary>
        /// Returns a message to send to the client instructing it
        /// to join the game server.
        /// </summary>
        /// <param name="target">Target connection.</param>
        /// <param name="port">Port of server.</param>
        /// <param name="token">Token of client.</param>
        /// <returns>Message to send to client.</returns>
        public static NetOutgoingMessage JoinGame(
            NetConnection target, int port, int token
            )
        {
            NetOutgoingMessage msg = target.Peer.CreateMessage();
            msg.Write((byte)1);
            msg.Write(port);
            msg.Write(token);

            return msg;
        }
    }
}
