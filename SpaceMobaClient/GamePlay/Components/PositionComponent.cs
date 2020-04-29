using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Lidgren.Network;

using SpaceMobaClient.Systems.Objects;

namespace SpaceMobaClient.GamePlay.Components
{
    /// <summary>
    /// The position component from the server, gives an entity
    /// a position within the world.
    /// </summary>
    public class PositionComponent : IComponent
    {
        // State of this component.
        public Vector2 Position;
        public Vector2 Momentum;
        public float Direction;
        public float AngularMomentum;

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
        public ComponentId Id => ComponentId.Position;

        /// <summary>
        /// Flag indicating whether this component needs the update event.
        /// </summary>
        public bool WantsUpdates => true;

        /// <summary>
        /// Flag indicating whether this component needs the draw event.
        /// </summary>
        public bool WantsDraws => false;

        /// <summary>
        /// Creates a new PositionComponent for entity parent.
        /// </summary>
        /// <param name="parent">Parent entity.</param>
        public PositionComponent(Entity parent)
        {
            Entity = parent;
        }

        /// <summary>
        /// Logic for the update event.
        /// </summary>
        /// <param name="gameTime">Game frame interval.</param>
        public void Update(GameTime gameTime)
        {
            // Calculate delta time from the frame interval. (Delta time is in
            // seconds).
            float deltaTime =
                (float)gameTime.ElapsedGameTime.Milliseconds / 1000;

            // Update direction
            Direction += AngularMomentum * deltaTime;

            // Update position
            Position += Momentum * deltaTime;
        }

        /// <summary>
        /// Draws this component.
        /// </summary>
        /// <param name="spriteBatch">Current sprite batch.</param>
        /// <param name="camera">Currently active camera.</param>
        public void Draw(SpriteBatch spriteBatch, Camera camera)
            => throw (new NotImplementedException());

        /// <summary>
        /// Unpacks an incoming message from the server into a position.
        /// </summary>
        /// <param name="message">Incoming message.</param>
        public void Deserialize(NetIncomingMessage message)
        {
            Position.X = message.ReadFloat();
            Position.Y = message.ReadFloat();
            Momentum.X = message.ReadFloat();
            Momentum.Y = message.ReadFloat();
            Direction = message.ReadFloat();
            AngularMomentum = message.ReadFloat();
        }

        /// <summary>
        /// This component does not rely on other components.
        /// </summary>
        public void Link()
        {

        }
    }
}
