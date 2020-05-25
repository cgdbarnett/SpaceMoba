using System;
using System.Diagnostics;
using System.Security.Cryptography;

using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceMobaClient
{
    /// <summary>
    /// Provides a wrapper for the Game() of the client.
    /// </summary>
    public static class GameManager
    {
        /// <summary>
        /// Reference to the GameClient.
        /// </summary>
        private static BlackholeGame Game;

        /// <summary>
        /// Constants for the game.
        /// </summary>
        #region Constants

        //////////////////
        /// NETWORKING ///
        //////////////////
        
        // App Identifier is based off the MD5 hash of
        // a buffer containing "SMCBH" (Space Moba Client Black Hole).
        private static readonly byte[] AppByteCode = new byte[] {
            (byte)'S', (byte)'M', (byte)'C', (byte)'B', (byte)'H',
            };

        /// <summary>
        /// Unique identification of Application.
        /// </summary>
        public static string AppIdentifier =
            MD5.Create().ComputeHash(AppByteCode).ToString();

        /// <summary>
        /// Port the login server listens on for new connections.
        /// </summary>
        public const int LoginPort = 8080;

        /// <summary>
        /// Host of the login server.
        /// </summary>
#if DEBUG
        public static readonly string LoginHost = "127.0.0.1";
#else
        public static readonly string LoginHost = "3.14.141.85";
#endif

        ///////////////////
        /// GAME WINDOW ///
        ///////////////////

        /// <summary>
        /// The title on the game window.
        /// </summary>
        public const string WindowTitle = "Blackhole";
        
        /// <summary>
        /// Width of the game window.
        /// </summary>
        public const int WindowWidth = 1920;

        /// <summary>
        /// Height of the game window.
        /// </summary>
        public const int WindowHeight = 1080;

        /// <summary>
        /// Flags whether the game is run in fullscreen.
        /// </summary>
#if DEBUG
        public const bool IsFullscreen = false;
#else
        public const bool IsFullscreen = true;
#endif

        #endregion

        /// <summary>
        /// Gets the ContentManager of the game.
        /// </summary>
        public static ContentManager Content
        {
            get
            {
                if(Game == null)
                {
                    throw (new InvalidOperationException());
                }
                return Game.Content;
            }
        }

        /// <summary>
        /// Gets the GraphicsDevice of the game.
        /// </summary>
        public static GraphicsDevice GraphicsDevice
        {
            get
            {
                if(Game == null)
                {
                    throw (new InvalidOperationException());
                }
                return Game.GraphicsDevice;
            }
        }

        /// <summary>
        /// Starts and runs the game client.
        /// </summary>
        public static void Run()
        {
            try
            {
                using (Game = new BlackholeGame())
                {
                    Game.Run();
                }
            }
            catch(Exception e)
            {
                Trace.WriteLine("Unhandled Exception occured: " + e.Message);
            }
        }
    }
}
