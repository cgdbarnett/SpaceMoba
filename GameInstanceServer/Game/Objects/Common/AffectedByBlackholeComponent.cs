using System;

using Microsoft.Xna.Framework;

using Lidgren.Network;

using GameInstanceServer.Systems.ECS;

namespace GameInstanceServer.Game.Objects.Common
{
    public class AffectedByBlackholeComponent : IComponent
    {
        // Positional and blackhole fields.
        public PositionComponent Position;
        public Vector2 Gravity;
        public Entity Entity;

        /// <summary>
        /// Gets the ComponentSystem for AffectedByBlackholeComponent.
        /// </summary>
        public ComponentSystemId ComponentSystem
        {
            get
            {
                return ComponentSystemId.BlackholeSystem;
            }
        }

        /// <summary>
        /// This is not a serializable component.
        /// </summary>
        public bool Serializable => true;

        /// <summary>
        /// This is not a serializable component.
        /// </summary>
        /// <param name="msg"></param>
        public void Serialize(NetOutgoingMessage msg)
        {
            msg.Write((byte)SerializableComponentId.AffectedByBlackhole);
        }
    }
}
