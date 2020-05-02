using Lidgren.Network;

namespace SpaceMobaClient.Systems.Network
{
    /// <summary>
    /// A network handler receives inputs from NetworkManager to trigger
    /// handling of data, and should be used to send outgoing messages
    /// to a net connection.
    /// </summary>
    public interface INetworkHandler
    {
        /// <summary>
        /// Handles incoming data.
        /// </summary>
        /// <param name="msg">Incoming message.</param>
        void HandleData(NetIncomingMessage msg);

        /// <summary>
        /// Handles a change in status.
        /// </summary>
        /// <param name="status">New status.</param>
        void HandleStatusChange(NetConnectionStatus status);
    }
}
