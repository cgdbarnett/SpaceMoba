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

            Position.Position += delta * Gravity;
        }

        /// <summary>
        /// Draws this component.
        /// </summary>
        /// <param name="spriteBatch">Current sprite batch.</param>
        /// <param name="camera">Currently active camera.</param>
        public void Draw(SpriteBatch spriteBatch, Camera camera) =>
            throw (new NotImplementedException());

        /// <summary>
        /// Unpacks an incoming message from the server into a position.
        /// </summary>
        /// <param name="message">Incoming message.</param>
        public void Deserialize(NetIncomingMessage message)
        {
            Gravity.X = message.ReadFloat();
            Gravity.Y = message.ReadFloat();
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
    }
}
