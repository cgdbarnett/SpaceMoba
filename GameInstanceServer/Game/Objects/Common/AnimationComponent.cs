using System;

using Lidgren.Network;

using GameInstanceServer.Systems.ECS;

namespace GameInstanceServer.Game.Objects.Common
{
    /// <summary>
    /// A component for sending animation information about an
    /// object to the client.
    /// </summary>
    public class AnimationComponent : IComponent
    {
        /// <summary>
        /// This is a serializable component.
        /// </summary>
        public bool Serializable => true;

        /// <summary>
        /// Id of animation system.
        /// </summary>
        public ComponentSystemId ComponentSystem 
            => ComponentSystemId.AnimationSystem;

        /// <summary>
        /// Resource name.
        /// </summary>
        public string Sprite;

        /// <summary>
        /// Serialize this component into an outgoing message.
        /// </summary>
        /// <param name="msg"></param>
        public void Serialize(NetOutgoingMessage msg)
        {
            msg.Write((byte)SerializableComponentId.Animation);
            msg.Write(Sprite);
        }
    }
}