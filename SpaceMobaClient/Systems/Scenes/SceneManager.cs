using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace SpaceMobaClient.Systems.Scenes
{
    /// <summary>
    /// SceneManager is a Singleton instance that runs scenes in game.
    /// </summary>
    public class SceneManager
    {
        // Single reference to SceneManager instance
        private static SceneManager Instance;

        // List of references to scenes within this game
        private Dictionary<int, IScene> SceneMap;
        private int ActiveSceneIndex;
        private int GotoSceneIndex;

        /// <summary>
        /// Private constructor so that this can only be instantiated from a
        /// private method.
        /// </summary>
        private SceneManager()
        {
            SceneMap = new Dictionary<int, IScene>();
            ActiveSceneIndex = 0;
            GotoSceneIndex = 0;
        }
        
        /// <summary>
        /// Returns the reference to the singleton, or instatiates the 
        /// singleton if it does not already exist.
        /// </summary>
        /// <returns>Reference to SceneManager</returns>
        public static SceneManager GetSceneManager()
        {
            if(Instance == null)
            {
                Instance = new SceneManager();
            }

            return Instance;
        }

        /// <summary>
        /// Sets the SceneList the SceneManager will use.
        /// </summary>
        /// <param name="sceneList">List of Scenes for this game.</param>
        /// <exception cref="ArgumentException">
        /// Thrown when sceneList is null, an empty list or a scene with
        /// a duplicate Id exists.</exception>
        /// <remarks>Will always default ActiveScene to Index 0.</remarks>
        public void SetSceneList(List<IScene> sceneList)
        {
            // Validate input
            if(sceneList == null)
            {
                throw (new ArgumentException());
            }
            if(sceneList.Count <= 0)
            {
                throw (new ArgumentException());
            }
            
            // Copy scene list to SceneMap.
            SceneMap.Clear();
            foreach(IScene scene in sceneList)
            {
                if(SceneMap.ContainsKey(scene.GetId()))
                {
                    throw (new ArgumentException());
                }
                SceneMap.Add(scene.GetId(), scene);
            }

            // Default ActiveScene
            ActiveSceneIndex = -1;
            GotoSceneIndex = 0;
        }

        /// <summary>
        /// Returns the reference to the currently active scene.
        /// </summary>
        /// <returns>Reference to the IScene currently active.</returns>
        /// <exception cref="IndexOutOfRangeException">Thrown when the
        /// currently active scene does not exist within the SceneMap.
        /// </exception>
        public IScene GetActiveScene()
        {
            try
            {
                return SceneMap[ActiveSceneIndex];
            }
            catch
            {
                throw (new IndexOutOfRangeException());
            }
        }

        /// <summary>
        /// Calls the update method of the currently active Scene.
        /// </summary>
        public void Update(GameTime gameTime)
        {
            // Load into new Scene if required.
            if(ActiveSceneIndex != GotoSceneIndex)
            {
                if(SceneMap.ContainsKey(ActiveSceneIndex))
                {
                    SceneMap[ActiveSceneIndex].Unload();
                }

                ActiveSceneIndex = GotoSceneIndex;
                SceneMap[ActiveSceneIndex].Load();
            }

            // Don't try to update if scene doesn't exist.
            if(SceneMap.ContainsKey(ActiveSceneIndex))
            {
                SceneMap[ActiveSceneIndex].Update(gameTime);
            }
        }

        /// <summary>
        /// Calls the draw method of the currently active Scene.
        /// </summary>
        public void Draw(GameTime gameTime)
        {
            // Don't try to draw if scene doesn't exist.
            if (SceneMap.ContainsKey(ActiveSceneIndex))
            {
                SceneMap[ActiveSceneIndex].Draw(gameTime);
            }
        }

        /// <summary>
        /// Instructs the SceneManager to change the active scene to the
        /// specified scene. This will only trigger after the current update
        /// is completed.
        /// </summary>
        /// <param name="scene">IScene reference to go to.</param>
        /// <exception cref="ArgumentException">Thrown when the scene
        /// referenced is not contained within the SceneList.</exception>
        public void GotoScene(IScene scene)
        {
            if(SceneMap.ContainsKey(scene.GetId()))
            {
                GotoSceneIndex = scene.GetId();
            }
            else
            {
                throw (new ArgumentException());
            }
        }

        /// <summary>
        /// Instructs the SceneManager to change the active scene to the next
        /// scene chronologically. This will only trigger after the current
        /// update is completed.
        /// </summary>
        public void GotoNextScene()
        {
            if(SceneMap.ContainsKey(ActiveSceneIndex + 1))
            {
                GotoSceneIndex = ActiveSceneIndex + 1;
            }
            else
            {
                GotoSceneIndex = 0;
            }
        }
    }
}
