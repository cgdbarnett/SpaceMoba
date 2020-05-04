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
        // Positional component
        private Vector2 LastPosition;
        private Vector2 NewPosition;
        public Vector2 Position
        {
            get
            {
                return (NewPosition - LastPosition) * LerpValue + LastPosition;
            }
            set
            {
                NewPosition += value - Position;
            }
        }

        // Momentum component
        public Vector2 Momentum;

        // Direction component
        public float Direction;

        // AngularMomentum component
        public float AngularMomentum;

        // Lerping values
        private const float TargetTime = 500;
        private long LastUpdate;
        private float LerpValue
        {
            get
            {
                return Math.Min((DateTime.Now.Ticks - LastUpdate) 
                    / 10000f / TargetTime, 1.0f);
            }
        }

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
            LastPosition = new Vector2();
            NewPosition = new Vector2();
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
            NewPosition += Momentum * deltaTime;
            LastPosition += Momentum * deltaTime;
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
            LastPosition.X = Position.X;
            LastPosition.Y = Position.Y;
            LastUpdate = DateTime.Now.Ticks;

            NewPosition.X = message.ReadFloat();
            NewPosition.Y = message.ReadFloat();
            Momentum.X = message.ReadFloat();
            Momentum.Y = message.ReadFloat();
            Direction = message.ReadFloat();
            AngularMomentum = message.ReadFloat();

            if((NewPosition - LastPosition).LengthSquared() > 50 * 50)
            {
                LastPosition = NewPosition;
            }
        }

        /// <summary>
        /// This component does not rely on other components.
        /// </summary>
        public void Link()
        {

        }
    }
}
