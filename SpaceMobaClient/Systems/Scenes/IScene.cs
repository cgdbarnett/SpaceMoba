using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace SpaceMobaClient.Systems.Scenes
{
    /// <summary>
    /// Interface for Scenes. Scenes represent a state within the game.
    /// For example, you might use a MenuScene, and a Level1Scene.
    /// </summary>
    public interface IScene
    {
        /// <summary>
        /// Returns the unique ID of this IScene.
        /// </summary>
        /// <returns>Unique identifier for the scene.</returns>
        int GetId();

        /// <summary>
        /// Instructs the IScene to load into memory any resources
        /// it requires. Reference to ContentManager included
        /// if required.
        /// </summary>
        /// <param name="content"></param>
        void Load();

        /// <summary>
        /// Instructs the IScene to unload any resources it has loaded
        /// into memory.
        /// </summary>
        void Unload();

        /// <summary>
        /// Runs a single frame update on the IScene.
        /// </summary>
        void Update(GameTime gameTime);

        /// <summary>
        /// Draws a single from of the IScene.
        /// </summary>
        void Draw(GameTime gameTime);
    }
}
