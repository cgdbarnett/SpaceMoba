using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace LoginServer.Instances
{
    /// <summary>
    /// Manages a pool of GameInstances.
    /// </summary>
    public class InstancePool
    {
        // Constants for instance pools.
        private const int MaxInstances = 8;
        private const int PortBase = 8081;

        // Pools for instances.
        private List<GameInstance> ActiveGameInstance, IdleGameInstance;

        /// <summary>
        /// Creates a new instance pool.
        /// </summary>
        public InstancePool()
        {
            ActiveGameInstance = new List<GameInstance>(MaxInstances);
            IdleGameInstance = new List<GameInstance>(MaxInstances);

            // Only IdleGameInstances get initiated
            for (int i = 0; i < MaxInstances; i++)
            {
                GameInstance game = new GameInstance()
                {
                    Port = PortBase + i
                };
                game.OnProcessEnd += (s, a) => HandleEndGame(s);

                IdleGameInstance.Add(game);
            }
        }

        /// <summary>
        /// Returns whether there is a game available.
        /// </summary>
        /// <returns>Whether a game is available.</returns>
        public bool GameAvailable()
        {
            return IdleGameInstance.Count > 0;
        }

        /// <summary>
        /// Starts a new game.
        /// </summary>
        /// <param name="tokens">Client tokens for game.</param>
        /// <returns>Port of game.</returns>
        public int StartGame(int[] tokens)
        {
            if(!GameAvailable())
            {
                throw (new Exception("No game available in InstancePool."));
            }

            GameInstance game = IdleGameInstance[IdleGameInstance.Count - 1];

            // Move game instance from idle game to active game.
            lock(ActiveGameInstance)
            {
                lock(IdleGameInstance)
                {
                    IdleGameInstance.RemoveAt(IdleGameInstance.Count - 1);
                    ActiveGameInstance.Add(game);
                }
            }

            game.Tokens = tokens;
            game.Start();

            // Log
            Trace.WriteLine("Game Started on :" + game.Port.ToString());

            return game.Port;
        }

        /// <summary>
        /// Event handler for when a game ends.
        /// </summary>
        /// <param name="sender">Reference to GameInstance.</param>
        private void HandleEndGame(object sender)
        {
            GameInstance game = (GameInstance)sender;
            Trace.WriteLine("Game Ended on :" + game.Port.ToString());

            // Move game instance from active to idle
            lock (ActiveGameInstance)
            {
                lock (IdleGameInstance)
                {
                    ActiveGameInstance.Remove(game);
                    IdleGameInstance.Add(game);
                }
            }
        }
    }
}
