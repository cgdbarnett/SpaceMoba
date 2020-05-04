using System.Diagnostics;

// System libraries
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

// Xna (Monogame) libraries
using Microsoft.Xna.Framework.Graphics;

// Game libraries
using SpaceMobaClient.GamePlay.Network;
using SpaceMobaClient.Systems.Gui;
using SpaceMobaClient.Systems.Network;
using SpaceMobaClient.Systems.Scenes;
using SpaceMobaClient.Systems.Sound;

namespace SpaceMobaClient.GamePlay.Scenes
{
    /// <summary>
    /// The countdown scene displays the countdown until a game starts.
    /// </summary>
    public class CountdownScene : IScene
    {
        // Iterators for scene.
        public IScene Next { get; set; }
        public IScene Previous { get; set; }

        // Resources for this scene.
        private readonly SpriteBatch SpriteBatch;
        private Texture2D LoadingScreen;
        private GuiLabel StatusLabel;

        // Timer
        private readonly Stopwatch Timer;
        private int Time;

        // Network
        private GameNetworkHandler NetworkHandler;

        /// <summary>
        /// Creates an instance of the countdown scene.
        /// </summary>
        public CountdownScene()
        {
            // Instantiate everything
            GraphicsDevice graphics =
                GameClient.GetGameClient().GetGraphicsDevice();

            SpriteBatch = new SpriteBatch(graphics);
            Timer = new Stopwatch();
        }

        /// <summary>
        /// Begins countdown for the game.
        /// </summary>
        /// <param name="handover">int time</param>
        public void Load(object handover)
        {
            NetworkHandler = (GameNetworkHandler)handover;
            Time = 6000; // Todo: Rework this yet again.

            // Load resources required for loading screen
            ContentManager content =
                GameClient.GetGameClient().GetContentManager();
            LoadingScreen =
                content.Load<Texture2D>("Backgrounds/title_screen");
            StatusLabel = new GuiLabel(
                16, 16, "Beginning Countdown",
                content.Load<SpriteFont>("Fonts/Arial14"),
                Color.White
                );

            // Start countdown
            Timer.Reset();
            Timer.Start();
        }

        /// <summary>
        /// Reset state.
        /// </summary>
        public void Unload()
        {
            LoadingScreen = null;
            Timer.Stop();
        }

        /// <summary>
        /// Draws a frame of the scene.
        /// </summary>
        /// <param name="gameTime">Game frame interval.</param>
        public void Draw(GameTime gameTime)
        {
            GraphicsDevice graphics = GameClient.GetGameClient()
                .GetGraphicsDevice();
            try
            {
                StatusLabel.SetText("Game starts in " + 
                    ((Time - Timer.ElapsedMilliseconds) / 1000 + 1).ToString()
                    + "s");

                SpriteBatch.Begin();
                // Draw countdown screen
                SpriteBatch.Draw(LoadingScreen, graphics.Viewport.Bounds,
                    Color.White);
                StatusLabel.Draw(gameTime, SpriteBatch);
                SpriteBatch.End();
            }
            catch
            {
            }
        }

        /// <summary>
        /// Update the scene -> moves to next scene once ready.
        /// </summary>
        /// <param name="gameTime">Game frame interval.</param>
        public void Update(GameTime gameTime)
        {
            // Update networking
            NetworkManager.HandleIncomingMessages();

            if(Timer.ElapsedMilliseconds >= Time)
            {
                SceneManager.GotoScene<InGameScene>(NetworkHandler);
            }
        }
    }
}
