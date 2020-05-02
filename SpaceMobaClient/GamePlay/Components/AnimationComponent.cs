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
    /// The animation component from the server, draws an entity
    /// in the world.
    /// </summary>
    public class AnimationComponent : IComponent
    {
        // State of this component.
        public Texture2D Sprite;
        public PositionComponent Position;

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
        public ComponentId Id => ComponentId.Animation;

        /// <summary>
        /// Flag indicating whether this component needs the update event.
        /// </summary>
        public bool WantsUpdates => false;

        /// <summary>
        /// Flag indicating whether this component needs the draw event.
        /// </summary>
        public bool WantsDraws => true;

        /// <summary>
        /// Creates a new AnimationComponent for entity parent.
        /// </summary>
        /// <param name="parent">Parent entity.</param>
        public AnimationComponent(Entity parent)
        {
            Entity = parent;
        }

        /// <summary>
        /// Logic for the update event.
        /// </summary>
        /// <param name="gameTime">Game frame interval.</param>
        public void Update(GameTime gameTime)
            => throw (new NotImplementedException());

        /// <summary>
        /// Draws this component.
        /// </summary>
        /// <param name="spriteBatch">Current sprite batch.</param>
        /// <param name="camera">Currently active camera.</param>
        public void Draw(SpriteBatch spriteBatch, Camera camera)
        {
            // WELCOME TO HIDEOUSNESS
            spriteBatch.Draw(
                Sprite,
                new Rectangle(
                    (int)Position.Position.X + camera.OffsetX,
                    (int)Position.Position.Y + camera.OffsetY,
                    Sprite.Width, Sprite.Height
                    ),
                Sprite.Bounds, Color.White,
                MathHelper.ToRadians(Position.Direction),
                new Vector2(Sprite.Width / 2, Sprite.Height / 2),
                SpriteEffects.None, 0);
        }

        /// <summary>
        /// Unpacks an incoming message from the server into a position.
        /// </summary>
        /// <param name="message">Incoming message.</param>
        public void Deserialize(NetIncomingMessage message)
        {
            // Get resource name, and try load
            try
            {
                Sprite = GameClient.GetGameClient().Content
                    .Load<Texture2D>(message.ReadString());
            }
            catch(Exception e)
            {
                Trace.WriteLine(
                    "Exception in AnimationComponent.Deserialize:"
                    );
                Trace.WriteLine(e.ToString());
            }
        }

        /// <summary>
        /// Attempts to link to the position component.
        /// </summary>
        public void Link()
        {
            IComponent component = Entity[ComponentId.Position];

            if(component != null)
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
