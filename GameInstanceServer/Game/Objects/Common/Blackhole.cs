using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;

using Lidgren.Network;

using GameInstanceServer.Game.World;
using GameInstanceServer.Systems.ECS;

namespace GameInstanceServer.Game.Objects.Common
{
    public class Blackhole : Entity
    {
        /// <summary>
        /// Position of the centre of the black hole.
        /// </summary>
        public static Vector2 Position
        {
            get;
            private set;
        }

        /// <summary>
        /// A combined mass and gravity constant value for calculating
        /// gravity of blackhole.
        /// </summary>
        private const float GravityConstant = 50000000f;

        /// <summary>
        /// Creates a blachole object.
        /// </summary>
        /// <param name="position">Spawn position of object.</param>
        public Blackhole(Vector2 position) : base(ECS.GetNextId())
        {
            // Register components
            Components = new IComponent[]
            {
                new AnimationComponent()
                {
                    Sprite = "Resources/Objects/Blackhole"
                },
                new PositionComponent()
                {
                    Position = new Vector2(position.X, position.Y),
                    Momentum = new Vector2(),
                    Direction = 0,
                    AngularMomentum = 0
                },
                new WorldComponent()
            };

            // Link components
            Position = ((PositionComponent)Components[1]).Position;
            ((WorldComponent)Components[2]).PositionComponent = 
                (PositionComponent)Components[1];
            ((WorldComponent)Components[2]).Entity = this;

            // Register components
            RegisterComponents();
        }

        /// <summary>
        /// Returns a float that represents how much momentum an orbitting
        /// object should decay per step.
        /// </summary>
        /// <param name="position">Position of object.</param>
        /// <returns>Decay ratio.</returns>
        public static float GetDecay(Vector2 position)
        {
            if(Position == null)
            {
                throw (new InvalidOperationException());
            }

            // Decay ratios
            const float farDecay = 0.000001f; // Decay from far away.
            const float midDecay = 0.0000015f; // Decay from mid point.
            const float nearDecay = 0.00005f; // Decay from nearby point.
            const float insideDecay = 0.005f; // Decay inside "atmosphere."

            // Boundary distances (all squared)
            const float insideBoundary = 1300f * 1300f;
            const float midBoundary = 3000f * 3000f;
            const float maxBoundary = 4400f * 4400f;

            // Note: objects can be further than max boundary, but the decay
            // ratio caps at that point.
            float distance = (position - Position).LengthSquared();
            if(distance < insideBoundary)
            {
                return MathHelper.Lerp(
                    nearDecay, insideDecay, 
                    MathHelper.Clamp(1 - distance / insideBoundary, 0f, 1f)
                    );
            }
            else if(distance < midBoundary)
            {
                return MathHelper.Lerp(
                    midDecay, nearDecay,
                    1 - (distance - insideBoundary) /
                    (midBoundary - insideBoundary)
                    );
            }
            else
            {
                return MathHelper.Lerp(
                    farDecay, midDecay,
                    MathHelper.Clamp(
                        1 - (distance - midBoundary) / 
                        (maxBoundary - midBoundary),
                        0, 1
                        )
                        );
            }
        }

        /// <summary>
        /// Returns the momentum an object needs to enter orbit of this
        /// blackhole from a given position.
        /// </summary>
        /// <param name="position">Centre of object.</param>
        /// <returns>Initial momentum.</returns>
        public static Vector2 GetInitialMomentum(Vector2 position)
        {
            if (Position == null)
            {
                throw (new InvalidOperationException());
            }

            // Get speed, 110px/s at 4400 px, and 2px/s per 150px.
            float distance = (position - Position).Length();
            float momentum = 110f + 2f / 150f * (4400f - distance);

            // Get direction tangent
            Vector2 direction = Position - position;
            direction.Normalize();

            // Rotate anticlockwise 90 degrees
            float roty = -direction.Y;
            direction.Y = direction.X;
            direction.X = roty;

            // Apply magnitude to direction
            return (110f + 2f / 150f * (4400f - distance)) * direction;
        }

        /// <summary>
        /// Returns the momentum to apply to an object due to the blackhole's
        /// gravity.
        /// </summary>
        /// <param name="position">Centre of object.</param>
        /// <returns>Gravity momentum.</returns>
        public static Vector2 GetMomentum(Vector2 position)
        {
            // Equation is constant / distance sqr'd,
            // in the direction of the blackhole.
            float distance = (position - Position).LengthSquared();
            float force = GravityConstant / distance;
            Vector2 direction = position - Position;
            direction.Normalize();
            direction.Y *= -1;
            direction.X *= -1;

            return direction * force;
        }
    }
}
