using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using SpaceMobaClient.GamePlay.Objects;
using SpaceMobaClient.Systems.IO;

namespace SpaceMobaClient.Systems.Server
{
    /// <summary>
    /// A "fake" implementation of a RemoteServer, to be used for
    /// testing.
    /// </summary>
    public class TestRemoteServer : IRemoteServer
    {
        // Event handlers
        public event ObjectEventHandler OnCreate;
        public event IntEventHandler OnDestroy;
        public event IntEventHandler OnAssignToLocalPlayer;
        public event IntEventHandler OnGameStart;

        private Ship LocalPlayerShip;

        /// <summary>
        /// Creates new instance of a test remote server.
        /// </summary>
        public TestRemoteServer()
        {

        }

        /// <summary>
        /// Imitates events that would occur when the client informs the
        /// server that it is ready to start playing.
        /// </summary>
        public void ClientIsReady()
        {
            // Trigger OnGameStart(Time to start in milliseconds)
            OnGameStart(6000);
        }

        /// <summary>
        /// Imitates events that would occur during connection to server.
        /// </summary>
        /// <param name="host"></param>
        /// <param name="port"></param>
        /// <param name="token"></param>
        public void Connect(string host, int port, int token)
        {
            // Spawn a new object, and assign it to local player.
            LocalPlayerShip = new Ship(0, GameClient.GetGameClient().
                GetContentManager().Load<Texture2D>
                ("Objects/Ships/GreenBeacon"), new Vector2(128, 128), 0);

            OnCreate(LocalPlayerShip);
            OnAssignToLocalPlayer(LocalPlayerShip.GetId());
        }

        /// <summary>
        /// Handles a change in input state from the local player.
        /// </summary>
        /// <param name="input">New input state.</param>
        public void UpdateInput(InputState input)
        {
            const float AngularForce = 120;
            const float LinearForce = 200;

            // Imitate server response
            Vector2 force = new Vector2();
            if (input.Forward) force.X += LinearForce;
            if (input.Backward) force.X -= LinearForce;
            if (input.Left) force.Y -= AngularForce;
            if (input.Right) force.Y += AngularForce;

            LocalPlayerShip.SetForce(force);
        }

        /// <summary>
        /// Handles imaginary incoming messages.
        /// </summary>
        public void HandleIncomingMessages()
        {

        }
    }
}
