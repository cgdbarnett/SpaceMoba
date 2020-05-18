using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace LoginServer.Instances
{
    /// <summary>
    /// Game Instance.
    /// </summary>
    public class GameInstance
    {
        // Process information.
        private readonly ProcessStartInfo StartInfo;
        private Process ServerProcess;

        /// <summary>
        /// Port to run server on.
        /// </summary>
        public int Port;

        /// <summary>
        /// Client tokens.
        /// </summary>
        public int[] Tokens;
        
        /// <summary>
        /// Returns whether this process is currently active.
        /// </summary>
        public bool Active
        {
            get;
            private set;
        }

        /// <summary>
        /// Fires when the game instance finishes.
        /// </summary>
        public event EventHandler OnProcessEnd;

        /// <summary>
        /// Creates a reusable GameInstance which manages the process
        /// for a game.
        /// </summary>
        public GameInstance()
        {
#if DEBUG
            StartInfo = new ProcessStartInfo("dotnet")
#else
            StartInfo = new ProcessStartInfo("GameInstanceServer.exe")
#endif
            {
                UseShellExecute = true,
#if DEBUG
                CreateNoWindow = false,
#else
                CreateNoWindow = true,
#endif
            };

            Port = 8080;
            Tokens = new int[Math.Max(Settings.MaxTeamSize * 2, 1)];
            Active = false;
        }

        /// <summary>
        /// Starts the game server. Starts a thread to wait for end
        /// of game.
        /// </summary>
        public void Start()
        {
            if (Active)
            {
                throw (new InvalidOperationException());
            }

            // Update arguments
#if DEBUG
            StartInfo.Arguments = "GameInstanceServer.dll " + Port.ToString();
#else
            StartInfo.Arguments = Port.ToString();
#endif
            StartInfo.Arguments += " " + (Settings.MaxTeamSize * 2).ToString();
            for (int i = 0; i < Settings.MaxTeamSize * 2; i++)
            {
                StartInfo.Arguments += " " + Tokens[i].ToString();
            }

            // Start process
            try
            {
                ServerProcess = Process.Start(StartInfo);
            }
            catch(Exception e)
            {
                Trace.WriteLine("Exception occured in GameInstance.Start():");
                Trace.WriteLine(e.ToString());
            }

            if(ServerProcess == null)
            {
                Trace.WriteLine("Error occured: ");
                Trace.WriteLine("GameInstanceServer not started.");
            }
            else
            {
                Active = true;
                Task.Run(() => WaitForProcessToEnd());
            }
        }

        /// <summary>
        /// Blocks thread until game is over.
        /// </summary>
        private void WaitForProcessToEnd()
        {
            ServerProcess.WaitForExit();

            Active = false;
            ServerProcess = null;

            try
            {
                OnProcessEnd.Invoke(this, null);
            }
            catch
            {
            }
        }
    }
}
