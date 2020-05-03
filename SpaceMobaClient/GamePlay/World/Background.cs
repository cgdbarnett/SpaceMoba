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
        private readonly Texture2D[] Textures;

        /// <summary>
        /// Creates a background.
        /// </summary>
        public Background()
        {
            Textures = new Texture2D[6];
            Textures[0] = GameClient.GetGameClient().Content
                .Load<Texture2D>("Backgrounds/starfield_deep");
            Textures[1] = GameClient.GetGameClient().Content
                .Load<Texture2D>("Backgrounds/starfield_mid");
            Textures[2] = GameClient.GetGameClient().Content
                .Load<Texture2D>("Backgrounds/starfield_mid2");
            Textures[3] = GameClient.GetGameClient().Content
                .Load<Texture2D>("Backgrounds/starfield_near");
            Textures[4] = GameClient.GetGameClient().Content
                .Load<Texture2D>("Backgrounds/starfield_near");
            Textures[5] = GameClient.GetGameClient().Content
                .Load<Texture2D>("Backgrounds/starfield_top");
        }

        /// <summary>
        /// Draws the background to the current spritebatch.
        /// </summary>
        /// <param name="spriteBatch">Current spritebatch.</param>
        /// <param name="camera">Active camera.</param>
        public void DrawBottom(SpriteBatch spriteBatch, Camera camera)
        {
            float[] layerRate = new float[]
            {
                0.15f,
                0.2f,
                0.3f,
                0.5f,
                1.0f
            };

            // Draw the 5 bottom layers of parallax
            for (int i = 0; i < 5; i++)
            {
                int offsetX, offsetY;

                offsetX = (int)(layerRate[i] * camera.Position.X);
                offsetY = (int)(layerRate[i] * camera.Position.Y);
                DrawLayer(spriteBatch, Textures[i], camera, offsetX, offsetY);
            }
        }

        public void DrawTop(SpriteBatch spriteBatch, Camera camera)
        {
            // Draw the top layer of parallax
            int offsetX, offsetY;

            offsetX = (int)(1.5 * camera.Position.X);
            offsetY = (int)(1.5 * camera.Position.Y);
            DrawLayer(spriteBatch, Textures[5], camera, offsetX, offsetY);
        }

        /// <summary>
        /// Draws a layer of the Parallax background.
        /// </summary>
        /// <param name="spriteBatch">Current sprite batch.</param>
        /// <param name="texture">Texture to draw.</param>
        /// <param name="camera">Active camera.</param>
        /// <param name="offsetX">Layer offset x.</param>
        /// <param name="offsetY">Layer offset y.</param>
        private void DrawLayer(
            SpriteBatch spriteBatch, Texture2D texture, Camera camera,
            int offsetX, int offsetY
            )
        {
            // Loop offset if it is larger than the background dimensions
            if (offsetX >= texture.Width) offsetX %= texture.Width;
            if (offsetY >= texture.Height) offsetY %= texture.Height;

            // Draw texture repeating rectangle for backgorund
            Point currentOffset = new Point(offsetX, offsetY);
            for (int yy = 0; yy < camera.Bounds.Height;
                yy += texture.Height - currentOffset.Y)
            {
                if (yy > 0) currentOffset.Y = 0;

                currentOffset.X = offsetX;
                for (int xx = 0; xx < camera.Bounds.Width;
                    xx += texture.Width - currentOffset.X)
                {
                    if (xx > 0) currentOffset.X = 0;

                    int width = Math.Min(texture.Width - currentOffset.X,
                        camera.Bounds.Width - xx);
                    int height = Math.Min(texture.Height - currentOffset.Y,
                        camera.Bounds.Height - yy);
                    Rectangle destination = new Rectangle(xx + camera.Bounds.X,
                        yy + camera.Bounds.Y, width, height);
                    Rectangle source = new Rectangle(currentOffset.X,
                        currentOffset.Y, width, height);
                    spriteBatch.Draw(texture, destination, source,
                        Color.White);
                }
            }
        }
    }
}
