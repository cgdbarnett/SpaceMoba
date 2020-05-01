// System libraries
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

// Xna (Monogame) libraries
using Microsoft.Xna.Framework.Graphics;

// Game libraries
using SpaceMobaClient.Systems.Gui;
using SpaceMobaClient.Systems.Network;
using SpaceMobaClient.Systems.Scenes;

namespace SpaceMobaClient.Content.Scenes
{
    /// <summary>
    /// LoadGame scene preloads resources for a game, and connects to
    /// the Game Server.
    /// </summary>
    public class LoadGameScene : IScene
    {
        // Iterators for scene.
        public IScene Next { get; set; }
        public IScene Previous { get; set; }

        // Resources for this scene.
        private readonly SpriteBatch SpriteBatch;
        private Texture2D LoadingScreen;
        private GuiLabel StatusLabel;

        private Task LoadTask, ConnectTask;

        // Network handler for Game.
        private INetworkInputHandler InputHandler;

        /// <summary>
        /// Creates a new LoadGame Scene.
        /// </summary>
        public LoadGameScene()
        {
            // Instantiate everything
            GraphicsDevice graphics =
                GameClient.GetGameClient().GetGraphicsDevice();

            SpriteBatch = new SpriteBatch(graphics);
        }

        /// <summary>
        /// Begins loading resources for the game.
        /// </summary>
        /// <param name="handover">object[] { host, port, token }</param>
        public void Load(object handover)
        {
            try
            {
                // Get server info from handover
                object[] inputs = (object[])handover;
                string host = (string)inputs[0];
                int port = (int)inputs[1];
                int token = (int)inputs[2];

                // Load resources required for loading screen
                ContentManager content =
                    GameClient.GetGameClient().GetContentManager();
                LoadingScreen =
                    content.Load<Texture2D>("Backgrounds/title_screen");
                StatusLabel = new GuiLabel(
                    16, 16, "Loading",
                    content.Load<SpriteFont>("Fonts/Arial14"),
                    Color.White
                    );

                // Begin asynchronously loading game resources, connecting
                // to server.
                LoadTask = Task.Run(() => LoadGameResources());
                ConnectTask = Task.Run(() =>
                {
                    try
                    {
                        NetworkManager.Connect(
                            host, port, token, InputHandler
                            );
                    }
                    catch(Exception e)
                    {
                        Trace.WriteLine("Exception in LoadGameScene.Load:");
                        Trace.WriteLine(e.ToString());

                        // Goto error scene.
                        SceneManager.GotoScene<ErrorScene>();
                    }
                });
            }
            catch(Exception e)
            {
                Trace.WriteLine("Exception in LoadGameScene.Load():");
                Trace.WriteLine(e.ToString());

                // Goto error scene.
                SceneManager.GotoScene<ErrorScene>();
            }
        }

        /// <summary>
        /// Reset state.
        /// </summary>
        public void Unload()
        {
            LoadingScreen = null;
            ConnectTask = null;
            LoadTask = null;
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
                SpriteBatch.Begin();
                // Draw loading screen
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
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {
            if (LoadTask != null && ConnectTask != null)
            {
                if (LoadTask.IsCompleted && ConnectTask.IsCompleted)
                {
                    if (NetworkManager.IsConnected)
                    {
                        // Goto next scene
                        SceneManager.GotoScene<WaitForPlayersScene>(
                            InputHandler
                            );
                    }
                    else
                    {
                        // Goto error scene.
                        SceneManager.GotoScene<ErrorScene>();
                    }
                }
            }
        }

        /// <summary>
        /// Preloads resources to be used in the game.
        /// </summary>
        private void LoadGameResources()
        {
            // Todo: Implement.
        }
    }
}
