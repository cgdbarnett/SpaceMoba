using System;
using System.Collections.Generic;
using System.Diagnostics;

using Lidgren.Network;

using GameInstanceServer.Systems.ECS;
using GameInstanceServer.Game;

// Modify component system id from ECS for a new id.
namespace GameInstanceServer.Systems.ECS
{
    // Extend class
    public partial class ComponentSystemId
    {
        // Create Id for Position System.
        public static readonly ComponentSystemId NetworkingClientSystem
            = new ComponentSystemId();
    }
}

namespace GameInstanceServer.Systems.Networking
{
    /// <summary>
    /// Networking Client System. Runs the client components of networking.
    /// </summary>
    public class NetworkingClientSystem : ComponentSystem
    {
        /// <summary>
        /// Creates a NetworkingSystem.
        /// </summary>
        public NetworkingClientSystem() : base()
        {
            Id = ComponentSystemId.NetworkingClientSystem;
            WantsUpdates = true;
        }

        /// <summary>
        /// Update a client in the game.
        /// </summary>
        /// <param name="component">NetworkingClientComponent.</param>
        /// <param name="gameTime">Game frame interval.</param>
        protected override void UpdateComponent(
            IComponent component, TimeSpan gameTime
            )
        {
            NetworkingClientComponent client = (NetworkingClientComponent)component;

            // Flush outgoing messages
            while(client.OutgoingMessageQueue.Count > 0)
            {
                client.NetPeer.SendMessage(
                    client.OutgoingMessageQueue.Dequeue(),
                    client.NetConnection,
                    NetDeliveryMethod.UnreliableSequenced
                    );
            }

            // Flush incoming messages
            while(client.IncomingMessageQueue.Count > 0)
            {
                HandleMessage(client, client.IncomingMessageQueue.Dequeue());
            }
        }

        /// <summary>
        /// Handles an incoming message for this client.
        /// </summary>
        /// <param name="msg"></param>
        private void HandleMessage(
            NetworkingClientComponent client, NetIncomingMessage msg
            )
        {
            NetOpCode opcode = (NetOpCode)msg.ReadInt16();

            try
            {
                switch (opcode)
                {
                    // Handle the client is ready.
                    case NetOpCode.ClientIsReady:
                        {
                            Trace.WriteLine("Client is ready.");
                            client.Ready = true;

                            // Send welcome packet
                            NetOutgoingMessage welcome = 
                                PacketWriter.WelcomePacket(
                                    client.NetConnection, client.Entity
                                    );
                            client.OutgoingMessageQueue.Enqueue(welcome);
                        }
                        break;

                    // Handle input from the client.
                    case NetOpCode.UpdatePlayerInput:
                        {
                        }
                        break;

                    default:
                        Trace.WriteLine("Received unexpected opcode.");
                        break;
                }
            }
            catch
            {

            }
        } 
    }
}