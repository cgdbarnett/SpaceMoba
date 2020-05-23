using System;
using System.Diagnostics;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using SpaceMobaClient.Systems.Scenes;

namespace SpaceMobaClient.GamePlay.Scenes
{
    /// <summary>
    /// The SplashScreen Scene shows an image on the screen for 5 seconds,
    /// then goes to the next scene.
    /// </summary>
    public class SplashScreenScene : IScene
    {
        // Image to be drawn as a SplashScreen.
        private Texture2D Image;
        private Rectangle ImageDestination;

        // Stopwatch for timing duration of SplashScreen
        private Stopwatch Timer;

        // Sprite Batch
        private SpriteBatch SpriteBatch;

        // State indicator, when content is loaded / unloaded.
        private bool Ready;

        // Scene iterators
        public IScene Next { get; set; }
        public IScene Previous { get; set; }

        /// <summary>
        /// Creats a splashscreen scene that renders the game logo.
        /// </summary>
        public SplashScreenScene()
        {
            SpriteBatch = new SpriteBatch(GameClient.GetGameClient().
                GetGraphicsDevice());
            ImageDestination = GameClient.GetGameClient().GetGraphicsDevice().
                Viewport.Bounds;
            Timer = new Stopwatch();
            Ready = false;
        }

        /// <summary>
        /// Loads the Image used for the splash screen into memory.
        /// </summary>
        /// <param name="handover">Null.</param>
        public void Load(object handover)
        {
            Image = GameClient.GetGameClient().GetContentManager().
                Load<Texture2D>("Backgrounds/title_screen");
            Timer.Start();
            Ready = true;
        }

        /// <summary>
        /// Unloads resources used for the splash screen from memory.
        /// </summary>
        /// <param name="content">ContentManager from Game.Content</param>
        public void Unload()
        {
            GameClient.GetGameClient().GetContentManager().Unload();
            Timer.Stop();
            Ready = false;
        }

        /// <summary>
        /// Draws the SplashScreen to screen.
        /// </summary>
        public void Draw(GameTime gameTime)
        {
            if(Ready)
            {
                GameClient.GetGameClient().GraphicsDevice.Clear(Color.Black);
                SpriteBatch.Begin();
                SpriteBatch.Draw(Image, ImageDestination, Color.White);
                SpriteBatch.End();
            }
        }

        /// <summary>
        /// Runs an update of the scene.
        /// </summary>
        /// <param name="gameTime">GameTime of Game.</param>
        public void Update(GameTime gameTime)
        {
            // When elapsed time reaches 2 seconds, goto next scene.
            if(Timer.ElapsedMilliseconds > 2000)
            {
                SceneManager.GotoNextScene();
            }
        }
    }
}
