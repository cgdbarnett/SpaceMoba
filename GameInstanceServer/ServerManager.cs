// System libaries
using System;
using System.Diagnostics;
using System.Threading;

// Game libraries
using GameInstanceServer.Game;
using GameInstanceServer.Game.Objects.Common;
using GameInstanceServer.Game.World;
using GameInstanceServer.Systems.ECS;
using GameInstanceServer.Systems.Networking;

namespace GameInstanceServer
{
    /// <summary>
    /// Manages the game server.
    /// </summary>
    public class ServerManager
    {
        // Id used for lidgren to identify game.
        private const string LidgrenAppId = "smc20";

        // State of game server
        private Stopwatch Timer, FrameTimer;

        /// <summary>
        /// Returns whether the server is currently active.
        /// </summary>
        public bool Running
        {
            get
            {
                return GameMaster.State != GameMaster.GameState.Shutdown;
            }
        }

        /// <summary>
        /// Creates a new server manager, which will listen for clients
        /// on a given port.
        /// </summary>
        /// <param name="port">Port to listen on.</param>
        /// <param name="tokens">User tokens for connection.</param>
        public ServerManager(int port, int[] tokens)
        {
            GameMaster.State = GameMaster.GameState.Loading;

            // Register ECS systems
            Trace.WriteLine("Registering ECS Systems.");
            Trace.Indent();

            // Networking
            ECS.RegisterSystem(new NetworkingSystem());
            ECS.RegisterSystem(new NetworkingClientSystem());

            // World partioning
            ECS.RegisterSystem(new WorldSystem());

            // Game systems
            ECS.RegisterSystem(new PositionSystem());
            ECS.RegisterSystem(new AnimationSystem());
            ECS.RegisterSystem(new BlachholeSystem());

            // Create networking component without an entity
            ECS.RegisterComponentToSystem(
                ComponentSystemId.NetworkingSystem, ECS.GetNextId(),
                new NetworkingComponent(port, tokens)
                );

            Trace.Unindent();
            Trace.WriteLine("ECS Registered.");

            // Register to State event handlers
            GameMaster.OnStartCountdown += HandleOnCountdown;
            GameMaster.OnStartGame += HandleOnGameStart;
            GameMaster.OnEndGame += HandleOnGameEnd;


            // Start timers
            Timer = new Stopwatch();
            Timer.Start();
            FrameTimer = new Stopwatch();
            FrameTimer.Start();

            GameMaster.State = GameMaster.GameState.WaitingForPlayers;
        }

        /// <summary>
        /// Runs a frame of the game server.
        /// </summary>
        public void Run()
        {
            // Target frequency
            const int targetFrequency = 15;
            const int targetMilliseconds = 1000 / targetFrequency;
            TimeSpan frameTime = new TimeSpan(FrameTimer.ElapsedTicks);

            FrameTimer.Restart();

            // Update ECS system.
            ECS.Update(frameTime);

            // Update game state
            switch(GameMaster.State)
            {
                case GameMaster.GameState.WaitingForPlayers:
                    // Check timer hasn't exceeded max wait time
                    if(Timer.Elapsed.TotalSeconds >= 
                        Settings.MaxWaitingTime)
                    {
                        GameMaster.StartCountdown(
                            Settings.CountdownTime
                            );
                    }
                    break;

                case GameMaster.GameState.Countdown:
                    // Check timer hasn't exceeded countdown time
                    if (Timer.Elapsed.TotalSeconds >=
                        Settings.CountdownTime)
                    {
                        GameMaster.StartGame();
                    }
                    break;

                case GameMaster.GameState.InGame:
                    //
                    break;

                case GameMaster.GameState.GameOver:
                    // Check timer hasn't exceeded end game time
                    if (Timer.Elapsed.TotalSeconds >=
                        Settings.EndgameTime)
                    {
                        GameMaster.State = GameMaster.GameState.Shutdown;
                    }
                    break;
            }

            // Wait for timer to reach target time
            while(FrameTimer.ElapsedMilliseconds < targetMilliseconds)
            {
                Thread.Sleep(1);
            }
        }

        /// <summary>
        /// Handles the OnCountdown event.
        /// </summary>
        /// <param name="sender">Usually null.</param>
        /// <param name="time">Time in seconds for countdown.</param>
        private void HandleOnCountdown(object sender, int time)
        {
            Trace.WriteLine("Starting Countdown.");
            Timer.Restart();
        }

        /// <summary>
        /// Handles the OnStartGame event.
        /// </summary>
        /// <param name="sender">Usually null.</param>
        /// <param name="args">Usually null.</param>
        private void HandleOnGameStart(object sender, object args)
        {
            Trace.WriteLine("Starting Game.");
            Timer.Restart();
        }

        /// <summary>
        /// Handles the OnEndGame event.
        /// </summary>
        /// <param name="sender">Usually null.</param>
        /// <param name="args">Usually null.</param>
        private void HandleOnGameEnd(object sender, object args)
        {
            Trace.WriteLine("Ending Game.");
            Timer.Restart();
        }
    }
}
