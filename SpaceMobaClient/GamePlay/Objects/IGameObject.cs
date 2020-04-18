using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceMobaClient.GamePlay.Objects
{
    /// <summary>
    /// An interface for all game objects.
    /// </summary>
    public interface IGameObject
    {
        /// <summary>
        /// Returns the unique identifier assigned to this object.
        /// </summary>
        /// <returns>Unique id of this object.</returns>
        int GetId();

        /// <summary>
        /// Returns whether this object should be drawn to screen.
        /// </summary>
        /// <returns>Whether to draw this object.</returns>
        bool IsVisible();

        /// <summary>
        /// Called by GameClient to draw the object to screen.
        /// </summary>
        /// <param name="spriteBatch">Current sprite batch.</param>
        void Draw(SpriteBatch spriteBatch, Camera camera);

        /// <summary>
        /// Called by GameClient to execute logic of object.
        /// </summary>
        /// <param name="gameTime">Game frame interval.</param>
        void Update(GameTime gameTime);

        /// <summary>
        /// Returns the current position of the object.
        /// </summary>
        /// <returns>Vector2 of the current position.</returns>
        Vector2 GetPosition();
    }
}
