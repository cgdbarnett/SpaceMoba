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
        public Vector2 InputForce;
        public float InputAngularForce;
        public Vector2 EngineForce;
        public float EngineAngularForce;

        /// <summary>
        /// Returns the linear force of the engine.
        /// </summary>
        public Vector2 Force
        {
            get
            {
                return InputForce.X * new Vector2(
                    (float)Math.Cos(
                        MathHelper.ToRadians(Position.Direction)
                        ),
                    (float)Math.Sin(
                        MathHelper.ToRadians(Position.Direction)
                        )
                    )
                    + InputForce.Y * new Vector2(
                        (float)Math.Cos(
                            MathHelper.ToRadians(Position.Direction + 90)
                            ),
                        (float)Math.Sin(
                            MathHelper.ToRadians(Position.Direction + 90)
                            )
                        );
            }
        }

        /// <summary>
        /// This is a serializable object.
        /// </summary>
        public bool Serializable => true;

        /// <summary>
        /// Serializes the component into a message.
        /// </summary>
        /// <param name="msg"></param>
        public void Serialize(NetOutgoingMessage msg)
        {
            msg.Write((byte)SerializableComponentId.Engine);
            msg.Write(Force.X);
            msg.Write(Force.Y);
            msg.Write(InputAngularForce);
        }
    }
}
