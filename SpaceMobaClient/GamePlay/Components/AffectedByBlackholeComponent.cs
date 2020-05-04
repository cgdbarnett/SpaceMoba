// System libraries
using System;
using System.Diagnostics;

// Xna (Monogame) libraries
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

// Lidgren libraries
using Lidgren.Network;

// Game libraries
using SpaceMobaClient.Systems.Objects;

namespace SpaceMobaClient.GamePlay.Components
{
    /// <summary>
    /// Applies the force of gravity to entities affected by the blackhole.
    /// </summary>
    public class AffectedByBlackholeComponent : IComponent
    {
        /// <summary>
        /// A combined mass and gravity constant value for calculating
        /// gravity of blackhole.
        /// </summary>
        private const float GravityConstant = 50000000f;

        /// <summary>
        /// Position of the centre of the black hole.
        /// </summary>
        private static readonly Vector2 BlackholePosition =
            new Vector2(6000, 6000);

        // State of this component
        public PositionComponent Position;
        public Vector2 Gravity;

        /// <summary>
        /// Returns the reference to the parent entity.
        /// </summary>
        public Entity Entity
        {
            get;
            private set;
        }

        /// <summary>
        /// Id of component.
        /// </summary>
        public ComponentId Id => ComponentId.AffectedByBlackhole;

        /// <summary>
        /// Flag indicating whether this component needs the update event.
        /// </summary>
        public bool WantsUpdates => true;

        /// <summary>
        /// Flag indicating whether this component needs the draw event.
        /// </summary>
        public bool WantsDraws => false;

        /// <summary>
        /// Creates a new AffectedByBlackholeComponent.
        /// </summary>
        /// <param name="entity">Entity to link to this component.</param>
        public AffectedByBlackholeComponent(Entity entity)
        {
            Entity = entity;
        }

        /// <summary>
        /// Logic for the update event.
        /// </summary>
        /// <param name="gameTime">Game frame interval.</param>
        public void Update(GameTime gameTime)
        {
            float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Update position, then decay momentum
            Position.Position += 
                delta * GetMomentum(Position.Position);
            Position.Momentum -= 
                Position.Momentum * GetDecay(Position.Position);
        }

        /// <summary>
        /// Draws this component.
        /// </summary>
        /// <param name="spriteBatch">Current sprite batch.</param>
        /// <param name="camera">Currently active camera.</param>
        public void Draw(SpriteBatch spriteBatch, Camera camera) =>
            throw (new NotImplementedException());

        /// <summary>
        /// Nothing to deserialize!
        /// </summary>
        /// <param name="message">Incoming message.</param>
        public void Deserialize(NetIncomingMessage message)
        {
        }

        /// <summary>
        /// Attempts to link to the position component.
        /// </summary>
        public void Link()
        {
            IComponent component = Entity[ComponentId.Position];

            if (component != null)
            {
                Position = (PositionComponent)component;
            }
            else
            {
                Trace.WriteLine("Error in AnimationComponent.Link:");
                Trace.WriteLine(
                    "Parent entity does not have a PositionComponent."
                    );
                throw (new InvalidOperationException());
            }
        }

        /// <summary>
        /// Returns a float that represents how much momentum an orbitting
        /// object should decay per step.
        /// </summary>
        /// <param name="position">Position of object.</param>
        /// <returns>Decay ratio.</returns>
        public static float GetDecay(Vector2 position)
        {
            if (BlackholePosition == null)
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
            float distance = (position - BlackholePosition).LengthSquared();
            if (distance < insideBoundary)
            {
                return MathHelper.Lerp(
                    nearDecay, insideDecay,
                    MathHelper.Clamp(1 - distance / insideBoundary, 0f, 1f)
                    );
            }
            else if (distance < midBoundary)
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
        /// Returns the momentum to apply to an object due to the blackhole's
        /// gravity.
        /// </summary>
        /// <param name="position">Centre of object.</param>
        /// <returns>Gravity momentum.</returns>
        public static Vector2 GetMomentum(Vector2 position)
        {
            // Equation is constant / distance sqr'd,
            // in the direction of the blackhole.
            float distance = (position - BlackholePosition).LengthSquared();
            float force = GravityConstant / distance;
            Vector2 direction = position - BlackholePosition;
            direction.Normalize();
            direction.Y *= -1;
            direction.X *= -1;

            return direction * force;
        }
    }
}
