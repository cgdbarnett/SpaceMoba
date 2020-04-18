using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using SpaceMobaClient.Content.Scenes;
using SpaceMobaClient.Systems.Scenes;

namespace SpaceMobaClient
{
    /// <summary>
    /// Implemented as a Singleton, this is the main entry point of the
    /// GameClient.
    /// </summary>
    public class GameClient : Game
    {
        // Singleton reference of GameClient
        private static GameClient Instance;

        // Monogame essential classes
        private GraphicsDeviceManager Graphics;
        
        /// <summary>
        /// 
        /// </summary>
        private GameClient()
        {
            Graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Returns a reference to the singleton instance of GameClient.
        /// </summary>
        /// <returns>Reference to GameClient</returns>
        public static GameClient GetGameClient()
        {
            if(Instance == null)
            {
                Instance = new GameClient();
            }
            return Instance;
        }

        /// <summary>
        /// Returns a reference to the ContentManager.
        /// </summary>
        /// <returns>ContentManager from Game.</returns>
        public ContentManager GetContentManager()
        {
            return Content;
        }

        /// <summary>
        /// Returns a reference to the GraphicsDevice.
        /// </summary>
        /// <returns>GraphicsDevice from Game.</returns>
        public GraphicsDevice GetGraphicsDevice()
        {
            return GraphicsDevice;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // Initialise game window
            Window.Title = "SpaceMobaClient";
            Graphics.PreferredBackBufferWidth = 1080;
            Graphics.PreferredBackBufferHeight = 720;
            Graphics.ApplyChanges();

            IsFixedTimeStep = false;
            IsMouseVisible = true;

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            InstantiateScenes();
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            SceneManager.GetSceneManager().Update(gameTime);
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            SceneManager.GetSceneManager().Draw(gameTime);
            base.Draw(gameTime);
        }

        /// <summary>
        /// This is called in LoadContent(), and creates the SceneList that the
        /// game will run from.
        /// </summary>
        /// <remarks>
        /// Every Scene you want in the game needs to be added to this list.
        /// </remarks>
        private void InstantiateScenes()
        {
            // Note: ID's need to be unique.
            List<IScene> scenes = new List<IScene>
            {
                new SplashScreen(0),
                //new MainMenu(1),
                new InGame(1)
            };

            SceneManager.GetSceneManager().SetSceneList(scenes);
        }
    }
}
