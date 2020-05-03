using Lidgren.Network;

using SpaceMobaClient.Systems.IO;
using SpaceMobaClient.Systems.Network;

namespace SpaceMobaClient.GamePlay.Network
{
    /// <summary>
    /// A collection of methods to generate packets to send
    /// data to the server.
    /// </summary>
    public static class PacketWriter
    {
        /// <summary>
        /// Creates the packet to send when the client is ready
        /// to enter the game.
        /// </summary>
        /// <returns>Outgoing message.</returns>
        public static NetOutgoingMessage ClientIsReady()
        {
            NetOutgoingMessage packet = 
                NetworkManager.Connection.CreateMessage();

            packet.Write((short)NetOpCode.ClientIsReady);
            return packet;
        }

        /// <summary>
        /// Creates the packet to send when updating player input.
        /// </summary>
        /// <param name="input">New input state.</param>
        /// <returns>Outgoing message.</returns>
        public static NetOutgoingMessage UpdatePlayerInput(InputState input)
        {
            NetOutgoingMessage packet =
                NetworkManager.Connection.CreateMessage();

            packet.Write((short)NetOpCode.UpdatePlayerInput);

            byte xx = 1;
            byte yy = 1;
            byte ss = 1;
            if (input.Forward) xx++;
            if (input.Backward) xx--;
            if (input.Left) yy--;
            if (input.Right) yy++;
            if (input.StrafLeft) ss--;
            if (input.StrafRight) ss++;

            packet.Write(xx);
            packet.Write(ss);
            packet.Write(yy);
            packet.Write(input.Attack);

            return packet;
        }
    }
}
