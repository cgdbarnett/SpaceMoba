// System libraries
using System;
using System.Diagnostics;
using System.Threading.Tasks;

using Myra.Graphics2D.UI;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

// Xna (Monogame) libraries
using Microsoft.Xna.Framework.Graphics;

// Game libraries
using SpaceMobaClient.GamePlay.Gui;
using SpaceMobaClient.GamePlay.Network;
using SpaceMobaClient.Systems.Network;
using SpaceMobaClient.Systems.Scenes;

namespace SpaceMobaClient.GamePlay.Scenes
{
    /// <summary>
    /// LoadGame scene preloads resources for a game, and connects to
    /// the Game Server.
    /// </summary>
    public class LoginScene : IScene
    {
        // Iterators for scene.
        public IScene Next { get; set; }
        public IScene Previous { get; set; }

        // Network handler for login.
        private LoginNetworkHandler NetworkHandler;

        // UI handler for login screen.
        private LoginMenuGui Gui;

        // Task for connection
        private Task ConnectTask;

        /// <summary>
        /// Creates a new LoadGame Scene.
        /// </summary>
        public LoginScene()
        {
            // Instantiate everything
            GraphicsDevice graphics =
                GameClient.GetGameClient().GetGraphicsDevice();
        }

        /// <summary>
        /// Begins loading resources for the game.
        /// </summary>
        /// <param name="handover">object[] { host, port, token }</param>
        public void Load(object handover)
        {
            try
            {

                // Instantiate network handler
                NetworkHandler = new LoginNetworkHandler();

                // Initiate Gui.
                Gui = new LoginMenuGui();
                Gui.OnLoginClicked += (s, a) => BeginLogin();
                Desktop.Root = Gui;
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
            Gui = null;
            NetworkHandler = null;
            Desktop.Root = null;
        }

        /// <summary>
        /// Draws a frame of the scene.
        /// </summary>
        /// <param name="gameTime">Game frame interval.</param>
        public void Draw(GameTime gameTime)
        {
            // Clear background, and draw GUI
            GameClient.GetGameClient().GraphicsDevice.Clear(Color.Black);
            Desktop.Render();
        }

        /// <summary>
        /// Update the scene -> moves to next scene once ready.
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {
            NetworkHandler.Update();
        }

        /// <summary>
        /// Handles logging into server.
        /// </summary>
        private void BeginLogin()
        {
            Gui.Enabled = false;
            // Begin asynchronously connecting
            // to server.
            ConnectTask = Task.Run(() =>
            {
                try
                {
                    NetworkHandler.Connect();
                }
                catch(Exception e)
                {
                    Trace.WriteLine("Exception in LoginScene.BeginLogin:");
                    Trace.WriteLine(e.ToString());

                    // Goto error scene.
                    SceneManager.GotoScene<ErrorScene>();
                }
            });
        }
    }
}
