using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceMobaClient.Systems.Gui
{
    /// <summary>
    /// An interface for drawing the GUI. Using an interface as there are
    /// plans to use existing GUI libraries for the development of the GUI.
    /// </summary>
    public interface IGui
    {
        /// <summary>
        /// Draws the GUI to the screen.
        /// </summary>
        /// <param name="gameTime">Game frame interval.</param>
        /// <param name="spriteBatch">Current sprite batch.</param>
        void Draw(GameTime gameTime, SpriteBatch spriteBatch);
    }
}
