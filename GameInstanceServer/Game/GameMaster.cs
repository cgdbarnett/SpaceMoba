using System;
using System.Diagnostics;

namespace GameInstanceServer.Game
{
    /// <summary>
    /// Static game state.
    /// </summary>
    public static class GameMaster
    {
        /// <summary>
        /// Enumeration of possible game states.
        /// </summary>
        public enum GameState
        {
            Loading,
            WaitingForPlayers,
            Countdown,
            InGame,
            GameOver,
            Shutdown
        }

        /// <summary>
        /// Current game state.
        /// </summary>
        public static GameState State;

        // Event handlers
        public static event EventHandler<int> OnStartCountdown;
        public static event EventHandler<object> OnStartGame;
        public static event EventHandler<object> OnEndGame;

        // Public game timer
        private readonly static Stopwatch ServerTimer = new Stopwatch();
        public static long ElapsedMilliseconds
        {
            get
            {
                return ServerTimer.ElapsedMilliseconds;
            }
        }

        /// <summary>
        /// Starts the game master timer.
        /// </summary>
        public static void StartMasterTimer()
        {
            ServerTimer.Start();
        }

        /// <summary>
        /// Starts the countdown timer.
        /// </summary>
        /// <param name="timer">Time (in seconds) to count from.</param>
        public static void StartCountdown(int timer)
        {
            try
            {
                State = GameState.Countdown;
                OnStartCountdown.Invoke(null, timer);
            }
            catch
            {

            }
        }

        /// <summary>
        /// Starts the game.
        /// </summary>
        public static void StartGame()
        {
            try
            {
                State = GameState.InGame;
                OnStartGame.Invoke(null, null);
            }
            catch
            {

            }
        }

        /// <summary>
        /// Ends the game.
        /// </summary>
        public static void EndGame()
        {
            try
            {
                State = GameState.GameOver;
                OnEndGame.Invoke(null, null);
            }
            catch
            {

            }
        }
    }
}
