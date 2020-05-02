// System libraries
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

// Xna (Monogame) libraries
using Microsoft.Xna.Framework.Graphics;

// Game libraries
using SpaceMobaClient.Systems.Gui;
using SpaceMobaClient.Systems.Scenes;

namespace SpaceMobaClient.GamePlay.Scenes
{
    /// <summary>
    /// Displayed when an unexpected error occurs that 
    /// prevents the game from continuing.
    /// </summary>
    public class ErrorScene : IScene
    {
        // Iterators for scene.
        public IScene Next { get; set; }
        public IScene Previous { get; set; }

        // Resources for this scene.
        private readonly SpriteBatch SpriteBatch;
        private Texture2D LoadingScreen;
        private GuiLabel StatusLabel;

        /// <summary>
        /// Creates a new Error Scene.
        /// </summary>
        public ErrorScene()
        {
            // Instantiate everything
            GraphicsDevice graphics =
                GameClient.GetGameClient().GetGraphicsDevice();

            SpriteBatch = new SpriteBatch(graphics);
        }

        /// <summary>
        /// Loads error message screen.
        /// </summary>
        /// <param name="handover">null</param>
        public void Load(object handover)
        {
            // Load resources required for loading screen
            ContentManager content =
                GameClient.GetGameClient().GetContentManager();
            LoadingScreen =
                content.Load<Texture2D>("Backgrounds/title_screen");
            StatusLabel = new GuiLabel(
                16, 16, "Unexpected error occurred.",
                content.Load<SpriteFont>("Fonts/Arial14"),
                Color.White
                );
        }

        /// <summary>
        /// Reset state.
        /// </summary>
        public void Unload()
        {
            LoadingScreen = null;
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
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {
        }
    }
}
