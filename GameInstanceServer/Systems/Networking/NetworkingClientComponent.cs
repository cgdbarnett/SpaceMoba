using System;
using System.Collections.Generic;
using System.Text;

using Lidgren.Network;

using GameInstanceServer.Systems.ECS;
using GameInstanceServer.Game;

namespace GameInstanceServer.Systems.Networking
{
    /// <summary>
    /// This component gives incoming/outgoing networking state.
    /// </summary>
    public class NetworkingClientComponent : IComponent
    {
        public NetPeer NetPeer;
        public NetConnection NetConnection;
        public bool Active;
        public bool Ready;
        public Queue<NetIncomingMessage> IncomingMessageQueue;
        public Queue<NetOutgoingMessage> OutgoingMessageQueue;
        public PlayerEntity Entity;

        /// <summary>
        /// Get the ComponentSystemId for the Networking Client System.
        /// </summary>
        public ComponentSystemId ComponentSystem
        {
            get
            {
                return ComponentSystemId.NetworkingClientSystem;
            }
        }

        /// <summary>
        /// Creates a new NetworkingClientComponent.
        /// </summary>
        /// <param name="entity">Parent entity.</param>
        public NetworkingClientComponent(PlayerEntity entity)
        {
            Entity = entity;
            IncomingMessageQueue = new Queue<NetIncomingMessage>();
            OutgoingMessageQueue = new Queue<NetOutgoingMessage>();
        }
    }
}