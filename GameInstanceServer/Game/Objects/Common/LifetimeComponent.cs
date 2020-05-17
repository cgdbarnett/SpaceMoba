using System;

using Lidgren.Network;

using GameInstanceServer.Systems.ECS;

namespace GameInstanceServer.Game.Objects.Common
{
    /// <summary>
    /// Holds state for a components Lifetime.
    /// </summary>
    public class LifetimeComponent : IComponent
    {
        /// <summary>
        /// Parent entity.
        /// </summary>
        public Entity Entity;

        /// <summary>
        /// Remaining lifetime of entity.
        /// </summary>
        public int LifetimeRemaining;

        /// <summary>
        /// Gets the ComponentSystem for Lifetime.
        /// </summary>
        public ComponentSystemId ComponentSystem
        {
            get
            {
                return ComponentSystemId.LifetimeSystem;
            }
        }

        /// <summary>
        /// This is a serializable component.
        /// </summary>
        public bool Serializable
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// This is a serializable component.
        /// </summary>
        /// <param name="msg">Outgoing message.</param>
        public void Serialize(NetOutgoingMessage msg)
        {
            throw (new NotImplementedException());
        }
    }
}
