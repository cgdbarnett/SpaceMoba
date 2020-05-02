// System libraries
using System.Diagnostics;

// Networking libraries
using Lidgren.Network;

// Game libraries
using SpaceMobaClient.Systems.Objects;
using SpaceMobaClient.Systems.Network;

namespace SpaceMobaClient.GamePlay.Network
{
    /// <summary>
    /// Handles inputs from the remote game server.
    /// </summary>
    public class GameNetworkHandler : INetworkHandler
    {
        /// <summary>
        /// Possible states of the network.
        /// </summary>
        public enum GameStates
        {
            Disconnected,
            Connecting,
            Connected,
            Waiting,
            Countdown,
            Game,
            GameOver
        }

        /// <summary>
        /// Current state.
        /// </summary>
        public GameStates State
        {
            get;
            private set;
        }

        /// <summary>
        /// Creates an instance of the GameInputHandler.
        /// </summary>
        public GameNetworkHandler()
        {
            State = GameStates.Connecting;
        }

        /// <summary>
        /// Handles incoming data from the remote server.
        /// </summary>
        /// <param name="msg">Incoming message.</param>
        public void HandleData(NetIncomingMessage msg)
        {
            // Handle opcode
            switch((NetOpCode)msg.ReadInt16())
            {
                case NetOpCode.StartGameCountdown:
                    State = GameStates.Countdown;
                    break;

                case NetOpCode.WelcomePacket:
                    {
                        Trace.WriteLine("Welcome packet received.");
                        int localPlayer = msg.ReadInt32();

                        // Create objects
                        int count = msg.ReadInt16();
                        for (int i = 0; i < count; i++)
                        {
                            Entity entity = EntityManager.CreateEntityFromMessage(msg);
                            if(entity.Id == localPlayer)
                            {
                                LocalPlayer.Entity = entity;
                            }
                        }
                    }
                    break;

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
            }
        }

        /// <summary>
        /// Handles a change in connection status to the
        /// remote server.
        /// </summary>
        /// <param name="status">New Status.</param>
        public void HandleStatusChange(NetConnectionStatus status)
        {
            switch(status)
            {
                case NetConnectionStatus.Connected:
                    if(State == GameStates.Connecting)
                    {
                        State = GameStates.Connected;
                    }
                    break;

                case NetConnectionStatus.Disconnected:
                    State = GameStates.Disconnected;
                    break;
            }
        }
        
        /// <summary>
        /// Send an outgoing message to the remote server.
        /// </summary>
        /// <param name="msg">Message to send.</param>
        public void SendMessage(NetOutgoingMessage msg)
        {
            NetworkManager.Connection.SendMessage(
                msg, NetDeliveryMethod.UnreliableSequenced
                );
        }
    }
}
