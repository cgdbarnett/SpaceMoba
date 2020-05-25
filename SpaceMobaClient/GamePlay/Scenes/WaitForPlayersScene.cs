// System libraries
using System.Diagnostics;

// Xna (Monogame) libraries
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

// Game libraries
using SpaceMobaClient.GamePlay.Network;
using SpaceMobaClient.Systems.Gui;
using SpaceMobaClient.Systems.Network;
using SpaceMobaClient.Systems.Scenes;

namespace SpaceMobaClient.GamePlay.Scenes
{
    public class WaitForPlayersScene : IScene
    {
        // Iterators for scene.
        public IScene Next { get; set; }
        public IScene Previous { get; set; }

        // Resources for this scene.
        private readonly SpriteBatch SpriteBatch;
        private Texture2D LoadingScreen;
        private GuiLabel StatusLabel;

        // Network handler.
        private GameNetworkHandler NetworkHandler;

        public WaitForPlayersScene()
        {
            // Instantiate everything
            GraphicsDevice graphics =
                GameManager.GraphicsDevice;

            SpriteBatch = new SpriteBatch(graphics);
        }

        /// <summary>
        /// Begins loading resources for the game.
        /// </summary>
        /// <param name="handover">NetworkHandler</param>
        public void Load(object handover)
        {
            // Load resources required for loading screen
            ContentManager content =
                GameManager.Content;
            LoadingScreen =
                content.Load<Texture2D>("Backgrounds/title_screen");
            StatusLabel = new GuiLabel(
                16, 16, "Waiting for players",
                content.Load<SpriteFont>("Fonts/Arial14"),
                Color.White
                );

            // Send client is ready message.
            NetworkHandler = (GameNetworkHandler)handover;
            NetworkHandler.SendMessage(PacketWriter.ClientIsReady());
        }

        /// <summary>
        /// Reset state.
        /// </summary>
        public void Unload()
        {
            LoadingScreen = null;
            NetworkHandler = null;
        }

        /// <summary>
        /// Draws a frame of the scene.
        /// </summary>
        /// <param name="gameTime">Game frame interval.</param>
        public void Draw(GameTime gameTime)
        {
            GraphicsDevice graphics = GameManager
                .GraphicsDevice;
            try
            {
                SpriteBatch.Begin();
                // Draw waiting screen
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
        /// <param name="gameTime">Game Frame Interval.</param>
        public void Update(GameTime gameTime)
        {
            // Update networking
            NetworkManager.HandleIncomingMessages();

            // Check state
            if(NetworkHandler.State == GameNetworkHandler.GameStates.Countdown)
            {
                // Goto countdown scene
                SceneManager.GotoScene<CountdownScene>(NetworkHandler);
            }
            else if(NetworkHandler.State == GameNetworkHandler.GameStates.Game)
            {
                // Goto game scene
                SceneManager.GotoScene<InGameScene>(NetworkHandler);
            }
            else if(NetworkHandler.State == GameNetworkHandler.GameStates.Disconnected
                || NetworkHandler.State == GameNetworkHandler.GameStates.GameOver
                )
            {
                // Goto error scene
                Trace.WriteLine("Error in WaitForPlayersScene.Update():");
                Trace.WriteLine("Network in invalid state.");
                SceneManager.GotoScene<ErrorScene>();
            }
        }
    }
}
