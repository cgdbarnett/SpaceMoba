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
        /// Next scene in list.
        /// </summary>
        IScene Next { get; set; }

        /// <summary>
        /// Previous scene in list.
        /// </summary>
        IScene Previous { get; set; }

        /// <summary>
        /// Instructs the IScene to load into memory any resources
        /// it requires. Reference to ContentManager included
        /// if required.
        /// </summary>
        /// <param name="handover">Handover state from previous scene.</param>
        void Load(object handover);

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
