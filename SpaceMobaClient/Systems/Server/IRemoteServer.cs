using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SpaceMobaClient.GamePlay.Objects;
using SpaceMobaClient.Systems.IO;

namespace SpaceMobaClient.Systems.Server
{
    public delegate void ObjectEventHandler(IGameObject obj);
    public delegate void IntEventHandler(int num);

    /// <summary>
    /// Implementing an interface for remote servers for creating both a
    /// real remote server, and a fake one for testing.
    /// </summary>
    public interface IRemoteServer
    {
        /// <summary>
        /// Triggers when the server issues the command to start the game.
        /// </summary>
        event IntEventHandler OnGameStart;

        /// <summary>
        /// Attempts to connect to the remote game server.
        /// </summary>
        /// <param name="host">IP address of host.</param>
        /// <param name="port">Port of host.</param>
        /// <param name="token">Token id for client.</param>
        void Connect(string host, int port, int token);

        /// <summary>
        /// Sends a message to the game server to let it know the client
        /// has loaded resources and is ready to start.
        /// </summary>
        void ClientIsReady();

        /// <summary>
        /// Sends a message to the game server to update the clients input
        /// state.
        /// </summary>
        /// <param name="input">Current input state.</param>
        void UpdateInput(InputState input);

        /// <summary>
        /// Handles incoming messages from the game server, and generates
        /// the relevant events.
        /// </summary>
        void HandleIncomingMessages();
    }
}
