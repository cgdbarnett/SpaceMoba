using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceMobaClient.Systems.Gui
{
    /// <summary>
    /// A Gui element that allows a user to draw a string to screen.
    /// </summary>
    public class GuiLabel : IGuiComponent
    {
        private string Text;
        private Point Position;
        private Color Color;
        private SpriteFont Font;

        /// <summary>
        /// Creates a new GuiLabel.
        /// </summary>
        /// <param name="x">X coordinate (top left).</param>
        /// <param name="y">Y coordinate (top left).</param>
        /// <param name="text">String to draw.</param>
        /// <param name="font">SpriteFont resource to draw.</param>
        /// <param name="color">Color to draw text in.</param>
        public GuiLabel(int x, int y, string text, SpriteFont font, 
            Color color)
        {
            Text = text;
            Position = new Point(x, y);
            Font = font;
            Color = color;
        }

        /// <summary>
        /// Sets all the properties of the label.
        /// </summary>
        /// <param name="x">X coordinate (top left).</param>
        /// <param name="y">Y coordinate (top left).</param>
        /// <param name="text">String to draw.</param>
        /// <param name="font">SpriteFont resource to draw.</param>
        /// <param name="color">Color to draw text in.</param>
        public void Set(int x, int y, string text, SpriteFont font,
            Color color)
        {
            Text = text;
            Position = new Point(x, y);
            Font = font;
            Color = color;
        }

        /// <summary>
        /// Sets the text shown by the label.
        /// </summary>
        /// <param name="text">Text to draw.</param>
        public void SetText(string text)
        {
            Text = text;
        }

        /// <summary>
        /// Sets the position of the label (top left).
        /// </summary>
        /// <param name="x">X coordinate.</param>
        /// <param name="y">Y coordinate.</param>
        public void SetPosition(int x, int y)
        {
            Position = new Point(x, y);
        }

        /// <summary>
        /// Sets the font used to draw the label.
        /// </summary>
        /// <param name="font">Font (must be loaded).</param>
        public void SetFont(SpriteFont font)
        {
            Font = font;
        }

        /// <summary>
        /// Sets the color to draw the label with.
        /// </summary>
        /// <param name="color">Color to draw text.</param>
        public void SetColor(Color color)
        {
            Color = color;
        }

        /// <summary>
        /// Draws the label to screen.
        /// </summary>
        /// <param name="gameTime">Game frame interval.</param>
        /// <param name="spriteBatch">Current sprite batch.</param>
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(Font, Text, Position.ToVector2(), Color);
        }
    }
}
