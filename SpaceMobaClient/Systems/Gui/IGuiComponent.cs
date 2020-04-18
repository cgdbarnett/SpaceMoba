using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceMobaClient.Systems.Gui
{
    /// <summary>
    /// A GuiComponent that is managed by the current Gui class.
    /// </summary>
    public interface IGuiComponent
    {
        /// <summary>
        /// Draws the Gui element to screen.
        /// </summary>
        /// <param name="gameTime">Game frame interval.</param>
        /// <param name="spriteBatch">The current sprite batch.</param>
        void Draw(GameTime gameTime, SpriteBatch spriteBatch);
    }
}
