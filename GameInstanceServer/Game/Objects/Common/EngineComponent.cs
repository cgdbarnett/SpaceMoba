using System;

using Microsoft.Xna.Framework;

using Lidgren.Network;

using GameInstanceServer.Systems.ECS;

namespace GameInstanceServer.Game.Objects.Common
{
    /// <summary>
    /// An engine component holds state for the Engine System.
    /// </summary>
    public class EngineComponent : IComponent
    {
        /// <summary>
        /// Returns the Engine System id.
        /// </summary>
        public ComponentSystemId ComponentSystem 
            => ComponentSystemId.EngineSystem;

        // State
        public PositionComponent Position;
        public Vector2 Force;

        /// <summary>
        /// This is a serializable object.
        /// </summary>
        public bool Serializable => false;

        /// <summary>
        /// Serializes the component into a message.
        /// </summary>
        /// <param name="msg"></param>
        public void Serialize(NetOutgoingMessage msg)
        {
            throw new NotImplementedException();
        }
    }
}
