// System libraries
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

// XNA (Monogame) libraries
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

// Game libraries
using SpaceMobaClient.Content.Gui;
using SpaceMobaClient.GamePlay;
using SpaceMobaClient.GamePlay.Objects;
using SpaceMobaClient.Systems.Gui;
using SpaceMobaClient.Systems.Scenes;
using SpaceMobaClient.Systems.Server;


namespace SpaceMobaClient.Content.Scenes
{
    /// <summary>
    /// Represents the states the InGame scene can be active in.
    /// </summary>
    public enum InGameState
    {
        Init,
        Loading,
        WaitingToStart,
        Countdown,
        InGame,
        GameOver
    }

    /// <summary>
    /// This scene is used when the player is in a game. This scene manages
    /// all the game objects, and inputs from the remote server, and from
    /// the player. It will then draw these objects to screen, and update
    /// the GUI.
    /// </summary>
    public class InGame : IScene
    {
        // Unique identifier for the scene.
        private readonly int Id;
        private InGameState CurrentInGameState;

        // Map of objects active in the scene.
        private Dictionary<int, IGameObject> GameObjectsInScene;

        // Graphics
        private SpriteBatch SpriteBatch;
        private Texture2D LoadingScreen, Background;

        // Scene controllers, Camera, GUI and LocalPlayer
        private Camera Camera;
        private InGameGui Gui;
        private LocalPlayer LocalPlayer;
        private IRemoteServer GameServer;

        // Scene timer
        private Stopwatch Timer;


        /// <summary>
        /// Constructs a new instance of the InGame scene.
        /// </summary>
        /// <param name="id">Unique identifier for this scene.</param>
        public InGame(int id)
        {
            // Get reference to graphics device from game client.
            GraphicsDevice graphics = 
                GameClient.GetGameClient().GetGraphicsDevice();

            // Copy unique id, and instantiate local data structures
            Id = id;
            CurrentInGameState = InGameState.Init;
            GameObjectsInScene = new Dictionary<int, IGameObject>();

            // Instantiate sprite batch
            SpriteBatch = new SpriteBatch(graphics);

            // Instantiate game camera
            Camera = new Camera(0, 0, graphics.Viewport.Width, 
                graphics.Viewport.Height);

            // Instantiate gui
            Gui = new InGameGui();

            // Instantiate remote server
            GameServer = new RemoteServer();
            GameServer.OnCreate += HandleCreateObject;
            GameServer.OnDestroy += HandleDestroyObject;
            GameServer.OnAssignToLocalPlayer += HandleAssignToLocalPlayer;
            GameServer.OnGameStart += HandleGameStart;

            // Create localplayer
            LocalPlayer = new LocalPlayer(GameServer);

            // Instantiate timer
            Timer = new Stopwatch();
        }

        /// <summary>
        /// Handles the event triggered by the remote server when a new object
        /// is created.
        /// </summary>
        /// <param name="obj">Object that was created.</param>
        private void HandleCreateObject(IGameObject obj)
        {
            if (GameObjectsInScene.ContainsKey(obj.GetId()))
            {
                // Update instead of create
                Ship update = (Ship)obj;
                Ship ship = (Ship)GameObjectsInScene[obj.GetId()];
                ship.SetPosition(update.GetPosition());
                ship.SetDirection(update.GetDirection());
                ship.SetMomentum(update.GetMomentum());
                ship.SetAngularMomentum(update.GetAngularMomentum());
            }
            else
            {
                GameObjectsInScene.Add(obj.GetId(), obj);
            }
        }

        /// <summary>
        /// Handles the event triggered by the remote server when an object
        /// is destroyed.
        /// </summary>
        /// <param name="obj">Object that was destroyed.</param>
        private void HandleDestroyObject(IGameObject obj)
        {
            try
            {
                GameObjectsInScene.Remove(obj.GetId());
            }
            catch
            {
            }
        }

        /// <summary>
        /// Handles the event triggered by the remote server when an object
        /// is assigned to be controlled by the local player.
        /// </summary>
        /// <param name="obj"></param>
        private void HandleAssignToLocalPlayer(int id)
        {
            try
            {
                IGameObject obj = GameObjectsInScene[id];
                LocalPlayer.SetLocalGameObject(obj);
            }
            catch
            {
                Trace.WriteLine("Error in InGame."
                    + "HandleAssignToLocalPlayer():");
                Trace.WriteLine("IGameObject does not exist.");
            }
        }

        /// <summary>
        /// Handles the event triggered by the remote server when the game
        /// start command is received.
        /// </summary>
        /// <param name="timer">Time til game starts (in milliseconds).</param>
        private void HandleGameStart(int timer)
        {
            if(CurrentInGameState == InGameState.WaitingToStart)
            {
                CurrentInGameState = InGameState.Countdown;
                Timer.Reset();
                Timer.Start();

                // To do, track timer rather than just assuming it will be 6000
            }
            else
            {
                throw (new InvalidOperationException());
            }
        }

        /// <summary>
        /// Draws a single frame of the scene.
        /// </summary>
        /// <param name="gameTime">Interval of frame.</param>
        public void Draw(GameTime gameTime)
        {
            // Clear frame
            GraphicsDevice graphics = GameClient.GetGameClient().
                GetGraphicsDevice();
            graphics.Clear(Color.Black);
            
            // Start sprite batch
            SpriteBatch.Begin();

            // If game is loaded, draw scene
            // otherwise draw loading screen.
            switch(CurrentInGameState)
            {
                case InGameState.Loading:
                    // Draw loading screen
                    DrawLoadingScreen();
                    break;

                case InGameState.WaitingToStart:
                    // Draw loading screen
                    DrawLoadingScreen();
                    break;

                case InGameState.Countdown:
                    // Draw countdown
                    DrawCountdownScreen();
                    break;

                case InGameState.InGame:
                    // Draw game
                    DrawGame(gameTime);
                    break;

                case InGameState.GameOver:
                    // Todo: Draw results screen
                    break;

                default:
                    // Do not handle
                    break;
            }

            SpriteBatch.End();
        }

        /// <summary>
        /// Draws the loading screen.
        /// </summary>
        private void DrawLoadingScreen()
        {
            GraphicsDevice graphics = GameClient.GetGameClient().
                GetGraphicsDevice();
            try
            {
                // Draw loading screen
                SpriteBatch.Draw(LoadingScreen, graphics.Viewport.Bounds,
                    Color.White);
            }
            catch
            {
            }
        }

        /// <summary>
        /// Draws the countdown until the game starts.
        /// </summary>
        private void DrawCountdownScreen()
        {
            GraphicsDevice graphics = GameClient.GetGameClient().
                GetGraphicsDevice();
            try
            {
                // Todo: BEAUTIFY
                // Draw label for timer as a countdown
                ((GuiLabel)Gui.GetComponent("timer")).
                    SetText("Game Starts in: " + 
                    (6.0 - Timer.Elapsed.TotalSeconds).ToString());
                Gui.Draw(null, SpriteBatch);
            }
            catch
            {
            }
        }

        /// <summary>
        /// Draws a frame of the game.
        /// </summary>
        /// <param name="gameTime">Game frame interval.</param>
        private void DrawGame(GameTime gameTime)
        {
            GraphicsDevice graphics = GameClient.GetGameClient().
                GetGraphicsDevice();

            try
            {
                // Update camera
                Point playersPosition =
                    LocalPlayer.GetLocalGameObjectPosition().ToPoint();
                Camera.CenterOnTarget(playersPosition.X, playersPosition.Y);
            }
            catch
            {
            }

            // Draw scene:
            // 1) Draw Background
            // 2) Draw Objects
            // 3) Draw GUI

            DrawBackground(SpriteBatch, Background, Camera.Bounds,
                Camera.Position);

            // Loop through all objects, and draw to screen
            foreach (IGameObject obj in GameObjectsInScene.Values)
            {
                try
                {
                    if (obj.IsVisible())
                    {
                        obj.Draw(SpriteBatch, Camera);
                    }
                }
                catch
                {
                }
            }

            Gui.Draw(gameTime, SpriteBatch);
        }

        /// <summary>
        /// Returns the unique identifier of this scene.
        /// </summary>
        /// <returns>Unique identifier of the scene.</returns>
        public int GetId()
        {
            return Id;
        }

        /// <summary>
        /// Begins loading the resources for this game level. This is done
        /// asynchronously, so this function will load the loading screen
        /// into memory, and then begin the asynchronous method to complete
        /// the process.
        /// </summary>
        public void Load()
        {
            CurrentInGameState = InGameState.Loading;
            LoadingScreen = GameClient.GetGameClient().
                Content.Load<Texture2D>("Backgrounds/loading");
            Camera.SetPosition(0, 0);

            // Begin asynchronously loading. Do not await this, as we want
            // the main thread to continue executing.
            Task.Factory.StartNew(() => EndLoad());
        }

        /// <summary>
        /// Asynchronously loads resources for this game level. When completed,
        /// this should update the Ready flag.
        /// </summary>
        private void EndLoad()
        {
            // Asynchronously start connecting to remote server.
            Task connect = new Task(() => ConnectToRemoteServer());
            connect.Start();

            // Get reference to content manager, it will be used frequently.
            ContentManager content = GameClient.GetGameClient().Content;

            // Load background
            Background = content.Load<Texture2D>("Backgrounds/starfield");

            // Load GUI
            Gui.Load();

            // Wait for connecting to finish. Thread sleeping because it keeps
            // trying to send the ClientIsReady packet too early. Dunno why.
            connect.Wait();
            Thread.Sleep(2000);

            // Update state and inform server this client is ready
            CurrentInGameState = InGameState.WaitingToStart;
            GameServer.ClientIsReady();
        }

        /// <summary>
        /// Connects to remote server for this game.
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
                GameClient.GetGameClient().Exit();
            }
        }

        /// <summary>
        /// Unloads resources used by this game level.
        /// </summary>
        public void Unload()
        {
            Timer.Stop();
            CurrentInGameState = InGameState.Init;
            GameClient.GetGameClient().GetContentManager().Unload();
            GameObjectsInScene.Clear();
        }

        /// <summary>
        /// Executes a single frame of the game level.
        /// </summary>
        /// <param name="gameTime">Gameupdate interval.</param>
        public void Update(GameTime gameTime)
        {
            GameServer.HandleIncomingMessages();

            switch (CurrentInGameState)
            {
                case InGameState.Countdown:
                    // When the timer reaches the time sent by the server
                    // (to do implement that), we move into game state.
                    if (Timer.ElapsedMilliseconds > 6000)
                    {
                        // Change state to ingame
                        CurrentInGameState = InGameState.InGame;
                        Timer.Restart();
                    }
                    break;

                case InGameState.InGame:
                    try
                    {
                        // First run any scene level logic required.
                        LocalPlayer.Update(gameTime);

                        // Update Gui
                        ((GuiLabel)Gui.GetComponent("timer")).
                            SetText(Timer.Elapsed.TotalSeconds.ToString());
                    }
                    catch
                    {
                    }

                    // Now loop through all game objects and execute their update
                    // methods.
                    foreach(IGameObject obj in GameObjectsInScene.Values)
                    {
                        try
                        {
                            obj.Update(gameTime);
                        }
                        catch
                        {
                        }
                    }
                    break;
            }
        }

        /// <summary>
        /// Draws the repeating background of the game world.
        /// </summary>
        /// <param name="spriteBatch">Current sprite batch.</param>
        /// <param name="bounds">Bounds of camera.</param>
        /// <param name="offset">Offset of localplayer.</param>
        private void DrawBackground(SpriteBatch spriteBatch, 
            Texture2D background, Rectangle bounds, Point offset)
        {
            // Loop offset if it is larger than the background dimensions
            if (offset.X >= background.Width) offset.X %= background.Width;
            if (offset.Y >= background.Height) offset.Y %= background.Height;

            // Draw texture repeating rectangle for backgorund
            Point currentOffset = new Point(offset.X, offset.Y);
            for (int yy = 0; yy < bounds.Height; 
                yy += background.Height - currentOffset.Y)
            {
                if (yy > 0) currentOffset.Y = 0;

                currentOffset.X = offset.X;
                for (int xx = 0; xx < bounds.Width; 
                    xx += background.Width - currentOffset.X)
                {
                    if (xx > 0) currentOffset.X = 0;

                    int width = Math.Min(background.Width - currentOffset.X, 
                        bounds.Width - xx);
                    int height = Math.Min(background.Height - currentOffset.Y, 
                        bounds.Height - yy);
                    Rectangle destination = new Rectangle(xx + bounds.X, 
                        yy + bounds.Y, width, height);
                    Rectangle source = new Rectangle(currentOffset.X, 
                        currentOffset.Y, width, height);
                    spriteBatch.Draw(background, destination, source, 
                        Color.White);
                }
            }
        }
    }
}
