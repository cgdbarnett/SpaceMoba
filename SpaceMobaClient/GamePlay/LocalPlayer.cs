// System libraries
using System;

// XNA (Monogame) libraries
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

// Game libraries
using SpaceMobaClient.GamePlay.Objects;
using SpaceMobaClient.Systems.IO;
using SpaceMobaClient.Systems.Server;

namespace SpaceMobaClient.GamePlay
{
    /// <summary>
    /// LocalPlayer handles player input, and drawing GUI.
    /// </summary>
    public class LocalPlayer : ILocalPlayer
    {
        private IGameObject LocalGameObject;
        private InputState PreviousInputState;
        private IRemoteServer GameServer;

        /// <summary>
        /// LocalPlayer handles player input, and GUI drawing.
        /// </summary>
        public LocalPlayer(IRemoteServer server)
        {
            LocalGameObject = null;
            GameServer = server;

            // Init input state
            PreviousInputState = new InputState
            {
                Backward = false,
                Forward = false,
                Left = false,
                Right = false
            };
        }

        /// <summary>
        /// LocalPlayer handles player input, and GUI drawing.
        /// </summary>
        public LocalPlayer(IGameObject localGameObject)
        {
            LocalGameObject = localGameObject;
        }

        /// <summary>
        /// Sets the current local game object.
        /// </summary>
        /// <param name="localGameObject">New object to represent
        /// the local player.</param>
        public void SetLocalGameObject(IGameObject localGameObject)
        {
            LocalGameObject = localGameObject;
        }

        /// <summary>
        /// Gets a reference to the current local game object.
        /// Note, this should never be null, but it can be changed.
        /// </summary>
        /// <returns>A reference to the local game object.</returns>
        /// <exception cref="ObjectDisposedException">
        /// Thrown when the localgameobject reference becomes null.
        /// </exception>
        public IGameObject GetLocalGameObject()
        {
            if(LocalGameObject != null)
            {
                return LocalGameObject;
            }
            else
            {
                throw (new ObjectDisposedException("LocalGameObject"));
            }
        }

        /// <summary>
        /// Returns the current position of the local game object.
        /// </summary>
        /// <returns>Returns the vector2 position.</returns>
        public Vector2 GetLocalGameObjectPosition()
        {
            return LocalGameObject.GetPosition();
        }

        /// <summary>
        /// Executes input handling for the local player.
        /// </summary>
        /// <param name="gameTime">Game frame interval.</param>
        public void Update(GameTime gameTime)
        {
            // Poll keyboard for current state
            KeyboardState state = Keyboard.GetState();

            // Todo: Key mapping
            InputState currentInputState = new InputState
            {
                Backward = state.IsKeyDown(Keys.S),
                Forward = state.IsKeyDown(Keys.W),
                Left = state.IsKeyDown(Keys.A),
                Right = state.IsKeyDown(Keys.D)
            };

            // Check if any input state has changed
            if(currentInputState != PreviousInputState)
            {
                // It has, so we should send the new state to the server.
                GameServer.UpdateInput(currentInputState);
                PreviousInputState = currentInputState;
            }
        }
    }
}
