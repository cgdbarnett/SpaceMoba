using System;

namespace SpaceMobaClient.GamePlay.Gui
{
    /// <summary>
    /// Login menu gui class.
    /// </summary>
	public partial class LoginMenuGui
	{
        /// <summary>
        /// Triggers when the client presses the login button.
        /// </summary>
        public event EventHandler OnLoginClicked;

        /// <summary>
        /// Creates an instance of the LoginMenuGui.
        /// </summary>
		public LoginMenuGui()
		{
            // Build UI
			BuildUI();

            // Hook up event listeners
            LoginButton.Click += (s, a) => OnLoginClicked(s, a);
		}
	}
}