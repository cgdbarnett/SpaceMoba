using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceMobaClient.Systems.Gui
{
    /// <summary>
    /// A Gui element that allows a user to draw a string to screen.
    /// </summary>
    public class GuiHealthbar : IGuiComponent
    {
        private float Value;
        private Rectangle Bounds;
        private Texture2D Background;
        private Texture2D Foreground;


        /// <summary>
        /// Creates a new GuiHealthbar.
        /// </summary>
        /// <param name="x">X coordinate (top left).</param>
        /// <param name="y">Y coordinate (top left).</param>.
        /// <param name="width">Width of bar.</param>
        /// <param name="height">Height of bar.</param>
        /// <param name="background">Background sprite.</param>
        /// <param name="foreground">Foreground sprite.</param>
        /// <param name="value">Value 0-1.0 of bar.</param>
        public GuiHealthbar(int x, int y, int width, int height,
            Texture2D background, Texture2D foreground, float value)
        {
            Bounds = new Rectangle(x, y, width, height);
            Background = background;
            Foreground = foreground;
            Value = value;
        }

        /// <summary>
        /// Sets the position of the healthbar (top left).
        /// </summary>
        /// <param name="x">X coordinate.</param>
        /// <param name="y">Y coordinate.</param>
        public void SetPosition(int x, int y)
        {
            Bounds.Location = new Point(x, y);
        }

        /// <summary>
        /// Set the value of the healthbar. 0 -> 1.
        /// </summary>
        /// <param name="value">Value 0 -> 1 of bar.</param>
        public void SetValue(float value)
        {
            Value = value;
        }

        /// <summary>
        /// Draws the healthbar to screen.
        /// </summary>
        /// <param name="gameTime">Game frame interval.</param>
        /// <param name="spriteBatch">Current sprite batch.</param>
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Background, Bounds, Color.White);

            Rectangle adjustedBounds = new Rectangle(
                Bounds.X + 2, Bounds.Y,
                (int)((Bounds.Width - 4) * Value), Bounds.Height
                );
            spriteBatch.Draw(Foreground, adjustedBounds, Color.White);
        }
    }
}
