using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;

namespace SpaceMobaClient.Systems.Scenes
{
    /// <summary>
    /// SceneManager is a Singleton instance that runs scenes in game.
    /// </summary>
    public static class SceneManager
    {
        // List of references to scenes within this game
        private static Dictionary<Type, IScene> SceneMap
            = new Dictionary<Type, IScene>();
        private static IScene ActiveSceneIndex = null;
        private static IScene GotoSceneIndex = null;
        private static object Handover;

        /// <summary>
        /// Sets the SceneList the SceneManager will use.
        /// </summary>
        /// <param name="sceneList">List of Scenes for this game.</param>
        /// <exception cref="ArgumentException">
        /// Thrown when sceneList is null, an empty list or a scene with
        /// a duplicate Id exists.</exception>
        /// <remarks>Will always default ActiveScene to Index 0.</remarks>
        public static void SetSceneList(List<IScene> sceneList)
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
            for(int i = 0; i < sceneList.Count; i++)
            {
                if (SceneMap.ContainsKey(sceneList[i].GetType()))
                {
                    throw (new ArgumentException());
                }
                SceneMap.Add(sceneList[i].GetType(), sceneList[i]);

                // Link scenes
                if (i > 0)
                {
                    sceneList[i].Previous = sceneList[i - 1];
                }
                else
                {
                    sceneList[i].Previous = null;
                }

                if(i < sceneList.Count - 1)
                {
                    sceneList[i].Next = sceneList[i + 1];
                }
                else
                {
                    sceneList[i].Next = null;
                }
            }

            // Default ActiveScene
            ActiveSceneIndex = null;
            GotoSceneIndex = null;
        }

        /// <summary>
        /// Returns the reference to the currently active scene.
        /// </summary>
        /// <returns>Reference to the IScene currently active.</returns>
        /// <exception cref="IndexOutOfRangeException">Thrown when the
        /// currently active scene does not exist within the SceneMap.
        /// </exception>
        public static IScene GetActiveScene()
        {
            try
            {
                return ActiveSceneIndex;
            }
            catch
            {
                throw (new IndexOutOfRangeException());
            }
        }

        /// <summary>
        /// Calls the update method of the currently active Scene.
        /// </summary>
        public static void Update(GameTime gameTime)
        {
            // Load into new Scene if required.
            if(ActiveSceneIndex != GotoSceneIndex)
            {
                if (ActiveSceneIndex != null)
                {
                    ActiveSceneIndex.Unload();
                }

                ActiveSceneIndex = GotoSceneIndex;
                ActiveSceneIndex.Load(Handover);
            }

            // Don't try to update if scene doesn't exist.
            if(ActiveSceneIndex != null)
            {
                ActiveSceneIndex.Update(gameTime);
            }
        }

        /// <summary>
        /// Calls the draw method of the currently active Scene.
        /// </summary>
        public static void Draw(GameTime gameTime)
        {
            // Don't try to draw if scene doesn't exist.
            if (ActiveSceneIndex != null)
            {
                ActiveSceneIndex.Draw(gameTime);
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
        public static void GotoScene(Type scene, object handover = null)
        {
            if(SceneMap.ContainsKey(scene))
            {
                GotoSceneIndex = SceneMap[scene];
                Handover = handover;
            }
            else
            {
                throw (new ArgumentException());
            }
        }

        /// <summary>
        /// Instructs the SceneManager to change the active scene to the
        /// specified scene. This will only trigger after the current update
        /// is completed.
        /// </summary>
        /// <param name="scene">IScene to go to.</param>
        public static void GotoScene(IScene scene, object handover = null)
        {
            GotoScene(scene.GetType(), handover);
        }

        /// <summary>
        /// Instructs the SceneManager to change the active scene to the
        /// specified scene. This will only trigger after the current update
        /// is completed.
        /// </summary>
        /// <typeparam name="T">Type of scene.</typeparam>
        public static void GotoScene<T>(object handover = null) where T : IScene
        {
            GotoScene(typeof(T), handover);
        }

        /// <summary>
        /// Instructs the SceneManager to change the active scene to the next
        /// scene chronologically. This will only trigger after the current
        /// update is completed.
        /// </summary>
        /// <param name="handover">Handover state passed to next scene.</param>
        public static void GotoNextScene(object handover = null)
        {
            if(SceneMap.ContainsKey(ActiveSceneIndex.Next.GetType()))
            {
                GotoSceneIndex = ActiveSceneIndex.Next;
            }
            else
            {
                GotoSceneIndex = null;
            }
            Handover = handover;
        }
    }
}
