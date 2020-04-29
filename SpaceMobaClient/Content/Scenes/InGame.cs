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
    /// This scene is used when the player is in a game. This scene manages
    /// all the game objects, and inputs from the remote server, and from
    /// the player. It will then draw these objects to screen, and update
    /// the GUI.
    /// </summary>
    public class InGame : IScene
    {
        // Unique identifier for the scene.
        private readonly int Id;

        // State
        private enum InGameState
        {
            Init,
            InGame,
            GameOver,
            Error
        }
        private InGameState State;

        // Map of objects active in the scene.
        private Dictionary<int, IGameObject> GameObjectsInScene;

        // Graphics
        private SpriteBatch SpriteBatch;
        private Texture2D Background;

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
            State = InGameState.Init;
            GameObjectsInScene = new Dictionary<int, IGameObject>();

            // Instantiate sprite batch
            SpriteBatch = new SpriteBatch(graphics);

            // Instantiate game camera
            Camera = new Camera(0, 0, graphics.Viewport.Width, 
                graphics.Viewport.Height);

            // Instantiate gui
            Gui = new InGameGui();

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
                ship.SetForce(update.GetForce(), update.GetAngularForce());
            }
            else
            {
                if(obj.GetId() == LocalPlayer.GetLocalGameObject().GetId())
                {
                    LocalPlayer.SetLocalGameObject(obj);
                }
                GameObjectsInScene.Add(obj.GetId(), obj);
            }
        }

        /// <summary>
        /// Handles the event triggered by the remote server when an object
        /// is destroyed.
        /// </summary>
        /// <param name="obj">Object that was destroyed.</param>
        private void HandleDestroyObject(int id)
        {
            try
            {
                GameObjectsInScene.Remove(id);
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
            throw (new InvalidOperationException());
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
            switch(State)
            {
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
        /// Copies resources from handover into memory.
        /// </summary>
        /// <param name="handover">
        /// object[] { LocalPlayer, RemoteServer, ICollection<IGameObject> }
        /// </param>
        public void Load(object handover)
        {
            try
            {
                if (State != InGameState.Init)
                {
                    throw (new InvalidOperationException());
                }

                // Deserialize handover
                object[] handoverArray = (object[])handover;
                LocalPlayer = (LocalPlayer)handoverArray[0];
                GameServer = (IRemoteServer)handoverArray[1];
                Dictionary<int, IGameObject>.ValueCollection initialObjects =
                    (Dictionary<int, IGameObject>.ValueCollection)
                    handoverArray[2];

                // Insert objects into map
                foreach (IGameObject obj in initialObjects)
                {
                    try
                    {
                        GameObjectsInScene.Add(obj.GetId(), obj);
                    }
                    catch (Exception e)
                    {
                        Trace.WriteLine(e.ToString());
                    }
                }

                // Register event handlers
                GameServer.OnCreate += HandleCreateObject;
                GameServer.OnDestroy += HandleDestroyObject;
                GameServer.OnAssignToLocalPlayer += HandleAssignToLocalPlayer;
                GameServer.OnGameStart += HandleGameStart;

                //// Temp code ////
                Background = GameClient.GetGameClient().GetContentManager()
                    .Load<Texture2D>("Backgrounds/starfield");
                Gui = new InGameGui();
                Gui.Load();
                ///////////////////

                // Move forward state
                Timer.Start();
                State = InGameState.InGame;
            }
            catch(Exception e)
            {
                Trace.WriteLine("Error in InGame.Load():");
                Trace.WriteLine(e.ToString());

                State = InGameState.Error;
            }
        }

        /// <summary>
        /// Unloads resources used by this game level.
        /// </summary>
        public void Unload()
        {
            // Dispose of structures that are passed to scene in handover.
            GameServer = null;
            LocalPlayer = null;

            // Unload resources
            GameClient.GetGameClient().GetContentManager().Unload();
            GameObjectsInScene.Clear();

            // Reset timer, and return state to init
            Timer.Stop();
            Timer.Reset();
            State = InGameState.Init;
        }

        /// <summary>
        /// Executes a single frame of the game level.
        /// </summary>
        /// <param name="gameTime">Gameupdate interval.</param>
        public void Update(GameTime gameTime)
        {
            switch (State)
            {
                case InGameState.InGame:
                    GameServer.HandleIncomingMessages();
                    try
                    {
                        // First run any scene level logic required.
                        LocalPlayer.Update(gameTime);

                        // Update Gui
                        UpdateGui();
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

        /// <summary>
        /// Updates the GUI values.
        /// </summary>
        private void UpdateGui()
        {
            ((GuiLabel)Gui.GetComponent("timer")).
                            SetText(((int)Timer.Elapsed.TotalSeconds).ToString());
            ((GuiHealthbar)Gui.GetComponent("shield")).
                SetValue(0.5f);
        }
    }
}
