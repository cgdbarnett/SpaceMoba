using System;
using System.Collections.Generic;

using Myra;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using SpaceMobaClient.GamePlay.Scenes;
using SpaceMobaClient.Systems.Network;
using SpaceMobaClient.Systems.Scenes;

namespace SpaceMobaClient
{
    /// <summary>
    /// Implemented as a Singleton, this is the main entry point of the
    /// GameClient. It also provides global state between scenes.
    /// </summary>
    public class BlackholeGame : Game
    {
        // Monogame essential classes
        private GraphicsDeviceManager Graphics;
        
        /// <summary>
        /// 
        /// </summary>
        public BlackholeGame()
        {
            Graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
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
            Window.Title = GameManager.WindowTitle;
            Graphics.PreferredBackBufferWidth = GameManager.WindowWidth;
            Graphics.PreferredBackBufferHeight = GameManager.WindowHeight;
            Graphics.IsFullScreen = GameManager.IsFullscreen;
            Graphics.ApplyChanges();

            // Initialise game step
            IsFixedTimeStep = false;
            TargetElapsedTime = new TimeSpan(150000);
            IsMouseVisible = true;
            
            // Initialise UI
            MyraEnvironment.Game = this;

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
            SceneManager.Update(gameTime);
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            SceneManager.Draw(gameTime);
            base.Draw(gameTime);
        }

        /// <summary>
        /// This is called in LoadContent(), and creates the SceneList that the
        /// game will run from.
        /// </summary>
        /// <remarks>
        /// Every Scene in the game needs to be added to this list.
        /// </remarks>
        private void InstantiateScenes()
        {
            // Order is important, is used for traversing scenes using Next and
            // Previous.
            List<IScene> scenes = new List<IScene>
            {
                new SplashScreenScene(),
                new LoginScene(),
                new LoadGameScene(),
                new WaitForPlayersScene(),
                new CountdownScene(),
                new InGameScene(),
                new ErrorScene(),
            };

            // Initialise scene manager.
            SceneManager.SetSceneList(scenes);
            SceneManager.GotoScene<SplashScreenScene>();
        }
    }
}
