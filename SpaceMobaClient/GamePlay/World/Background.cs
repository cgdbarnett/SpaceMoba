using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceMobaClient.GamePlay.World
{
    /// <summary>
    /// Handles drawing of the background in game.
    /// </summary>
    public class Background
    {
        private readonly Texture2D Texture;

        /// <summary>
        /// Creates a background.
        /// </summary>
        public Background()
        {
            Texture = GameClient.GetGameClient().Content
                .Load<Texture2D>("Backgrounds/starfield");
        }

        /// <summary>
        /// Draws the background to the current spritebatch.
        /// </summary>
        /// <param name="spriteBatch">Current spritebatch.</param>
        /// <param name="camera">Active camera.</param>
        public void Draw(SpriteBatch spriteBatch, Camera camera)
        {
            // Loop offset if it is larger than the background dimensions
            int offsetX = camera.Position.X;
            int offsetY = camera.Position.Y;
            if (offsetX >= Texture.Width) offsetX %= Texture.Width;
            if (offsetY >= Texture.Height) offsetY %= Texture.Height;

            // Draw texture repeating rectangle for backgorund
            Point currentOffset = new Point(offsetX, offsetY);
            for (int yy = 0; yy < camera.Bounds.Height;
                yy += Texture.Height - currentOffset.Y)
            {
                if (yy > 0) currentOffset.Y = 0;

                currentOffset.X = offsetX;
                for (int xx = 0; xx < camera.Bounds.Width;
                    xx += Texture.Width - currentOffset.X)
                {
                    if (xx > 0) currentOffset.X = 0;

                    int width = Math.Min(Texture.Width - currentOffset.X,
                        camera.Bounds.Width - xx);
                    int height = Math.Min(Texture.Height - currentOffset.Y,
                        camera.Bounds.Height - yy);
                    Rectangle destination = new Rectangle(xx + camera.Bounds.X,
                        yy + camera.Bounds.Y, width, height);
                    Rectangle source = new Rectangle(currentOffset.X,
                        currentOffset.Y, width, height);
                    spriteBatch.Draw(Texture, destination, source,
                        Color.White);
                }
            }
        }
    }
}
