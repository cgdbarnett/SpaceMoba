// System libraries
using System;
using System.Diagnostics;
using System.Threading.Tasks;

using Myra.Graphics2D.UI;

// Xna (Monogame) libraries
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

// Game libraries
using SpaceMobaClient.GamePlay.Gui;
using SpaceMobaClient.GamePlay.Network;
using SpaceMobaClient.Systems.Gui;
using SpaceMobaClient.Systems.Network;
using SpaceMobaClient.Systems.Scenes;

namespace SpaceMobaClient.GamePlay.Scenes
{
    /// <summary>
    /// A test scene for developing the UI.
    /// </summary>
    public class UITestScene : IScene
    {
        // Iterators for scene.
        public IScene Next { get; set; }
        public IScene Previous { get; set; }

        // Resources for this scene.
        private readonly SpriteBatch SpriteBatch;

        // UI control manager:
        private TestUIControls UIControls;

        /// <summary>
        /// Creates a new LoadGame Scene.
        /// </summary>
        public UITestScene()
        {
            // Instantiate everything
            GraphicsDevice graphics =
                GameClient.GetGameClient().GetGraphicsDevice();

            SpriteBatch = new SpriteBatch(graphics);
        }

        /// <summary>
        /// Loads scene.
        /// </summary>
        /// <param name="handover">null</param>
        public void Load(object handover)
        {
            //UIControls = new TestUIControls(GameClient.GetGameClient());
            //GameClient.GetGameClient().Components.Add(UIControls);
            
            Desktop.Root = new LoginMenuGui(); ;
        }

        /// <summary>
        /// Reset state.
        /// </summary>
        public void Unload()
        {
            GameClient.GetGameClient().Components.Remove(UIControls);
            UIControls = null;
            Desktop.Root = null;
        }

        /// <summary>
        /// Draws a frame of the scene.
        /// </summary>
        /// <param name="gameTime">Game frame interval.</param>
        public void Draw(GameTime gameTime)
        {
            GameClient.GetGameClient().GraphicsDevice.Clear(Color.Black);
            Desktop.Render();
        }

        /// <summary>
        /// Update the scene -> moves to next scene once ready.
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {
        }
    }
}