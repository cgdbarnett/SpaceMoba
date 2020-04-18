// XNA (Monogame) libraries
using Microsoft.Xna.Framework;

// Game libraries
using SpaceMobaClient.GamePlay.Objects;

namespace SpaceMobaClient.GamePlay
{
    /// <summary>
    /// An interface that represents a LocalPlayer, that can be locally played
    /// or remotely played (eg spectating).
    /// </summary>
    public interface ILocalPlayer
    {
        /// <summary>
        /// Returns a reference to the GameObject that is currently locally
        /// played.
        /// </summary>
        /// <returns>Reference to local GameObject.</returns>
        IGameObject GetLocalGameObject();

        /// <summary>
        /// Returns a vector representing the local players current position.
        /// </summary>
        /// <returns>A vector2 containing the players position.</returns>
        Vector2 GetLocalGameObjectPosition();

        /// <summary>
        /// Runs a single frame of update logic for the local player. This
        /// might include input handling or server overrule corrections.
        /// </summary>
        /// <param name="gameTime">Game frame interval.</param>
        void Update(GameTime gameTime);
    }
}
