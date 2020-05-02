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
    public class EngineComponent : IComponent
    {
        // State of this component
        public PositionComponent Position;
        public Vector2 Force;
        public float AngularForce;

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
        public ComponentId Id => ComponentId.Engine;

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
        public EngineComponent(Entity entity)
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

            Position.Momentum += delta * Force;
            Position.AngularMomentum += delta * AngularForce;
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
            Force.X = message.ReadFloat();
            Force.Y = message.ReadFloat();
            AngularForce = message.ReadFloat();
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
