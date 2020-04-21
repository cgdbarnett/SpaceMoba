// System libraries
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

// Xna (Monogame) libraries
using Microsoft.Xna.Framework.Graphics;

// Game libraries
using SpaceMobaClient.GamePlay;
using SpaceMobaClient.GamePlay.Objects;
using SpaceMobaClient.Systems.Gui;
using SpaceMobaClient.Systems.Scenes;
using SpaceMobaClient.Systems.Server;

namespace SpaceMobaClient.Content.Scenes
{
    /// <summary>
    /// The GameStart scene loads resources that are used by the level, and
    /// waits for the server to start the game.
    /// </summary>
    public class GameStart : IScene
    {
        // Unique identifier for the scene.
        private readonly int Id;

        // Graphics
        private SpriteBatch SpriteBatch;
        private Texture2D LoadingScreen;
        private GuiLabel Status;

        // Server state objects to send to InGame
        private ILocalPlayer LocalPlayer;
        private IRemoteServer GameServer;

        // Objects to send to InGame
        private Dictionary<int, IGameObject> ObjectsInScene;

        // State
        private enum GameStartState
        {
            Init,
            Loading,
            ClientIsReady,
            WaitingForOthers,
            Countdown,
            Error
        }
        private GameStartState State;

        // Timer
        private Stopwatch Timer;
        private int CountdownTime;

        /// <summary>
        /// Creates a new GameStart scene. The GameStart scene loads resources
        /// that are used by the level, and waits for the server to start the
        /// game.
        /// </summary>
        /// <param name="id">Unique Identifier of scene.</param>
        public GameStart(int id)
        {
            Id = id;
            State = GameStartState.Init;

            // Instantiate everything
            GraphicsDevice graphics = 
                GameClient.GetGameClient().GetGraphicsDevice();

            SpriteBatch = new SpriteBatch(graphics);
            ObjectsInScene = new Dictionary<int, IGameObject>();

            Timer = new Stopwatch();
        }

        /// <summary>
        /// Handles the ObjectCreate event from GameServer.
        /// </summary>
        /// <param name="obj">Obj to create.</param>
        private void HandleCreateObject(IGameObject obj)
        {
            try
            {
                ObjectsInScene.Add(obj.GetId(), obj);
            }
            catch(Exception e)
            {
                Trace.WriteLine("Error in GameStart.HandleCreateObject():");
                Trace.WriteLine(e.ToString());
            }
        }

        /// <summary>
        /// Handles the ObjectCreate event from GameServer.
        /// </summary>
        /// <param name="id">Removes an object.</param>
        private void HandleDestroyObject(int id)
        {
            try
            {
                ObjectsInScene.Remove(id);
            }
            catch (Exception e)
            {
                Trace.WriteLine("Error in GameStart.HandleDestroyObject():");
                Trace.WriteLine(e.ToString());
            }
        }

        /// <summary>
        /// Handles the ObjectCreate event from GameServer.
        /// </summary>
        /// <param name="id">Removes an object.</param>
        private void HandleAssignLocalPlayer(int id)
        {
            try
            {
                IGameObject obj = ObjectsInScene[id];
                LocalPlayer.SetLocalGameObject(obj);
            }
            catch(Exception e)
            {
                Trace.WriteLine("Error in GameStart."
                    + "HandleAssignToLocalPlayer():");
                Trace.WriteLine(e.ToString());
            }
        }

        /// <summary>
        /// Handles the event triggered by the remote server when the game
        /// start command is received.
        /// </summary>
        /// <param name="timer">Time til game starts (in milliseconds).</param>
        private void HandleGameStart(int timer)
        {
            if (State == GameStartState.WaitingForOthers)
            {
                State = GameStartState.Countdown;
                Timer.Reset();
                Timer.Start();
                CountdownTime = timer;
            }
            else
            {
                // Todo: Enable players to finish loading, and join countdown
                // when they're ready.
                Trace.WriteLine("Todo: Handle this exception. "
                    + "GameStart.HandleGameStart().");
                throw (new InvalidOperationException());
            }
        }

        /// <summary>
        /// Draws a single frame of the GameStart scene.
        /// </summary>
        /// <param name="gameTime">Game frame interval.</param>
        public void Draw(GameTime gameTime)
        {
            GraphicsDevice graphics = GameClient.GetGameClient().
                GetGraphicsDevice();
            graphics.Clear(Color.Black);

            SpriteBatch.Begin();

            switch (State)
            {
                // Draw loading screen
                case GameStartState.Loading:
                    DrawLoadingScreen(gameTime);
                    break;

                // Draw waiting for others screen
                case GameStartState.ClientIsReady:
                case GameStartState.WaitingForOthers:
                    DrawWaitingForOthers(gameTime);
                    break;

                // Draw countdown screen
                case GameStartState.Countdown:
                    DrawCountdownScreen(gameTime);
                    break;
                
                // Draw error message.
                case GameStartState.Error:
                    DrawErrorScreen(gameTime);
                    break;
            }

            SpriteBatch.End();
        }

        /// <summary>
        /// Draws the loading screen.
        /// </summary>
        /// <param name="gameTime">Game frame interval.</param>
        private void DrawLoadingScreen(GameTime gameTime)
        {
            GraphicsDevice graphics = GameClient.GetGameClient().
                GetGraphicsDevice();
            try
            {
                // Draw loading screen
                Status.SetText("Loading.");
                SpriteBatch.Draw(LoadingScreen, graphics.Viewport.Bounds,
                    Color.White);
                Status.Draw(gameTime, SpriteBatch);
            }
            catch
            {
            }
        }

        /// <summary>
        /// Draws the WaitingForOthers screen.
        /// </summary>
        /// <param name="gameTime">Game frame interval.</param>
        private void DrawWaitingForOthers(GameTime gameTime)
        {
            GraphicsDevice graphics = GameClient.GetGameClient().
                GetGraphicsDevice();
            try
            {
                // Draw WaitingForOthers screen
                Status.SetText("Waiting for players");
                SpriteBatch.Draw(LoadingScreen, graphics.Viewport.Bounds,
                    Color.White);
                Status.Draw(gameTime, SpriteBatch);
            }
            catch
            {
            }
        }

        /// <summary>
        /// Draws the countdown until the game starts.
        /// </summary>
        /// <param name="gameTime">Game frame interval.</param>
        private void DrawCountdownScreen(GameTime gameTime)
        {
            GraphicsDevice graphics = GameClient.GetGameClient().
                GetGraphicsDevice();
            try
            {
                Status.SetText(
                    "Starting Game in: " + ((int)
                    (CountdownTime / 1000 + 1 - Timer.Elapsed.TotalSeconds)
                    ).ToString() + " seconds"
                    );
                SpriteBatch.Draw(LoadingScreen, graphics.Viewport.Bounds,
                    Color.White);
                Status.Draw(gameTime, SpriteBatch);
            }
            catch
            {
            }
        }

        /// <summary>
        /// Draws the Error screen.
        /// </summary>
        /// <param name="gameTime">Game frame interval.</param>
        private void DrawErrorScreen(GameTime gameTime)
        {
            GraphicsDevice graphics = GameClient.GetGameClient().
                GetGraphicsDevice();
            try
            {
                // Draw WaitingForOthers screen
                Status.SetText("An error occured. Please restart.");
                SpriteBatch.Draw(LoadingScreen, graphics.Viewport.Bounds,
                    Color.White);
                Status.Draw(gameTime, SpriteBatch);
            }
            catch
            {
            }
        }

        /// <summary>
        /// Returns the unique Id of this scene.
        /// </summary>
        /// <returns>Id.</returns>
        public int GetId()
        {
            return Id;
        }

        /// <summary>
        /// Loads GameStart resources, and starts asynchronously loading
        /// resources for the InGame scene.
        /// </summary>
        /// <param name="handover">Null.</param>
        /// <exception cref="InvalidOperationException">
        /// GameStart.State must be in GameStartState.Init.
        /// </exception>
        public void Load(object handover)
        {
            // Make sure state is correct
            if(State != GameStartState.Init)
            {
                throw (new InvalidOperationException());
            }

            // Load resources required for loading screen
            State = GameStartState.Loading;
            ContentManager content =
                GameClient.GetGameClient().GetContentManager();
            LoadingScreen = 
                content.Load<Texture2D>("Backgrounds/title_screen");
            Status = new GuiLabel(
                16, 16, "Loading",
                content.Load<SpriteFont>("Fonts/Arial14"),
                Color.White
                );

            // Create global references
            GameServer = new RemoteServer();
            LocalPlayer = new LocalPlayer(GameServer);

            // Hook up event listeners
            GameServer.OnCreate += HandleCreateObject;
            GameServer.OnDestroy += HandleDestroyObject;
            GameServer.OnAssignToLocalPlayer += HandleAssignLocalPlayer;
            GameServer.OnGameStart += HandleGameStart;

            // Begin asynchronously loading game resources, connecting
            // to server, and a thread to update when these tasks are
            // complete.
            Task load = Task.Run(() => LoadGameResources());
            Task connect = Task.Run(() => ConnectToRemoteServer());
            Task.Run(() => EndLoad(load, connect));
        }
        
        /// <summary>
        /// Loads resources for the game. This method is asynchronous to
        /// the main thread of the game.
        /// </summary>
        private void LoadGameResources()
        {
            // TODO: Work out how to do this in the best manner.

            // Get reference to content manager, it will be used frequently.
            ContentManager content = GameClient.GetGameClient().Content;

            // Load background
            //Background = content.Load<Texture2D>("Backgrounds/starfield");

            // Load GUI
            //Gui.Load();
        }

        /// <summary>
        /// Connects to the remote server. This method is asynchronous to
        /// the main thread of the game.
        /// </summary>
        private void ConnectToRemoteServer()
        {
            try
            {
                MatchmakerServer matchmaker =
                    MatchmakerServer.GetMatchmakerServer();
                if (matchmaker.CurrentGameActive)
                {
                    GameServer.Connect(
                        matchmaker.CurrentGameHost,
                        matchmaker.CurrentGamePort,
                        matchmaker.CurrentGameToken
                        );
                }
                else
                {
                    throw (new Exception());
                }
            }
            catch
            {
                // Go to disconnected scene?
                State = GameStartState.Error;
            }
        }

        /// <summary>
        /// Waits for the game to load resources, and connect to the remote
        /// server. This thread will then update to ClientIsReady state.
        /// </summary>
        /// <param name="loadGameResources"></param>
        /// <param name="connectToRemoteServer"></param>
        private void EndLoad(Task loadGameResources, Task connectToRemoteServer)
        {
            loadGameResources.Wait();
            connectToRemoteServer.Wait();

            // ClientIsReady
            State = GameStartState.ClientIsReady;
        }

        /// <summary>
        /// Unloads resources used only for the GameStart screen.
        /// </summary>
        /// <remarks>
        /// Currently only resets state to Init.
        /// </remarks>
        public void Unload()
        {
            // Clear event handlers
            GameServer.OnCreate -= HandleCreateObject;
            GameServer.OnDestroy -= HandleDestroyObject;
            GameServer.OnAssignToLocalPlayer -= HandleAssignLocalPlayer;
            GameServer.OnGameStart -= HandleGameStart;

            // Stop the timer
            Timer.Stop();

            // Reset state
            LoadingScreen = null;
            LocalPlayer = null;
            GameServer = null;
            State = GameStartState.Init;
        }

        /// <summary>
        /// Runs a single update frame of the scene. Most states do not require
        /// logic.
        /// </summary>
        /// <param name="gameTime">Game frame interval.</param>
        public void Update(GameTime gameTime)
        {
            switch(State)
            {
                case GameStartState.ClientIsReady:
                    DoClientIsReady();
                    break;

                case GameStartState.WaitingForOthers:
                    DoWaitingForOthers();
                    break;

                case GameStartState.Countdown:
                    DoCountdown();
                    break;
            }
        }

        /// <summary>
        /// Handles when the game client is ready to enter the match.
        /// </summary>
        private void DoClientIsReady()
        {
            GameServer.HandleIncomingMessages();

            GameServer.ClientIsReady();
            State = GameStartState.WaitingForOthers;
        }

        /// <summary>
        /// Handles waiting for other clients to load.
        /// </summary>
        private void DoWaitingForOthers()
        {
            GameServer.HandleIncomingMessages();
        }

        /// <summary>
        /// Handles the countdown till game starts.
        /// </summary>
        private void DoCountdown()
        {
            GameServer.HandleIncomingMessages();

            // When the timer reaches the time sent by the server
            // (to do implement that), we move into game state.
            if (Timer.ElapsedMilliseconds > 6000)
            {
                object[] handover = new object[]
                {
                    LocalPlayer,
                    GameServer,
                    ObjectsInScene.Values
                };

                // Move to next scene.
                SceneManager.GetSceneManager().GotoNextScene(handover);
            }
        }
    }
}
