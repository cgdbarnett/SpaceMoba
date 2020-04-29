using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Lidgren.Network;

using SpaceMobaClient.GamePlay;

namespace SpaceMobaClient.Systems.Objects
{
    /// <summary>
    /// A component of an Entity.
    /// </summary>
    public interface IComponent
    {
        /// <summary>
        /// Returns the reference to the parent entity.
        /// </summary>
        Entity Entity { get; }

        /// <summary>
        /// Id of component.
        /// </summary>
        ComponentId Id { get; }

        /// <summary>
        /// Flag indicating whether this component needs the update event.
        /// </summary>
        bool WantsUpdates { get; }

        /// <summary>
        /// Flag indicating whether this component needs the draw event.
        /// </summary>
        bool WantsDraws { get; }

        /// <summary>
        /// Logic for the update event.
        /// </summary>
        /// <param name="gameTime">Game frame interval.</param>
        void Update(GameTime gameTime);

        /// <summary>
        /// Draws this component.
        /// </summary>
        /// <param name="spriteBatch">Current sprite batch.</param>
        /// <param name="camera">Currently active camera.</param>
        void Draw(SpriteBatch spriteBatch, Camera camera);

        /// <summary>
        /// Unpacks an incoming message from the server into an IComponent.
        /// </summary>
        /// <param name="message">Incoming message.</param>
        void Deserialize(NetIncomingMessage message);

        /// <summary>
        /// Attempts to link to other essential components.
        /// </summary>
        void Link();
    }
}
