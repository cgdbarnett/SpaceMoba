using System;

using Microsoft.Xna.Framework;

using Lidgren.Network;

using GameInstanceServer.Systems.ECS;
using GameInstanceServer.Systems.Physics;

namespace GameInstanceServer.Game.Objects.Common
{
    /// <summary>
    /// Position component.
    /// </summary>
    public class PositionComponent : IComponent
    {
        // Positional data.
        public Vector2 Position;
        public Vector2 Momentum;
        public float Direction;
        public float AngularMomentum;

        /// <summary>
        /// CollisionMask of entity.
        /// </summary>
        public CollisionMask CollisionMask;

        /// <summary>
        /// Gets the ComponentSystem for Positioned objects.
        /// </summary>
        public ComponentSystemId ComponentSystem
        {
            get
            {
                return ComponentSystemId.PositionSystem;
            }
        }

        /// <summary>
        /// This is a serializable component.
        /// </summary>
        public bool Serializable
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// Serializes position into a out going msg.
        /// </summary>
        /// <param name="msg">Outgoing message.</param>
        public void Serialize(NetOutgoingMessage msg)
        {
            // Id | X | Y | MoX | MoY | Direction | AngMo
            msg.Write((byte)SerializableComponentId.Position);
            msg.Write(Position.X);
            msg.Write(Position.Y);
            msg.Write(Momentum.X);
            msg.Write(Momentum.Y);
            msg.Write(Direction);
            msg.Write(AngularMomentum);
        }
    }
}
