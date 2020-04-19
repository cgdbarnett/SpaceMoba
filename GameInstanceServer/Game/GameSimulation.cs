// System libaries
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

// XNA (Monogame) libraries. This is the .Core libraries.
using Microsoft.Xna.Framework;

// Game libraries.
using GameInstanceServer.Game.Objects;
using GameInstanceServer.Game.World;

namespace GameInstanceServer.Game
{
    /// <summary>
    /// A singleton class that runs asynchronously. This class runs the game
    /// simulation.
    /// </summary>
    public class GameSimulation
    {
        // Singleton instance reference.
        private static GameSimulation Instance;

        // Thread that runs the game loop.
        private Thread GameSimulationThread;
        private DateTime LastUpdate;

        // Data structure that holds the current map of active objects
        private ConcurrentDictionary<int, IGameObject> ObjectsInGame;
        private WorldGrid WorldGrid;

        // Next unique id for objects.
        private int NextAvailableId;

        // Flag used by the thread to control the infinite loop.
        private bool Running;

        /// <summary>
        /// Private constructor for the singleton.
        /// </summary>
        private GameSimulation()
        {
            ObjectsInGame = new ConcurrentDictionary<int, IGameObject>();
            WorldGrid = new WorldGrid(10000, 10000, 1000, 1000); // TODO MAGIC NUMBERS
            NextAvailableId = 0;
            Running = false;
            LastUpdate = DateTime.Now;

            // Prepare thread, but do not start it.
            GameSimulationThread = new Thread(
                new ThreadStart(() => GameSimulationLoop()));
        }

        /// <summary>
        /// Returns a reference to the singleton reference of GameSimulation.
        /// </summary>
        /// <returns>Reference to singleton instance.</returns>
        public static GameSimulation GetGameSimulation()
        {
            if(Instance == null)
            {
                Instance = new GameSimulation();
            }
            return Instance;
        }

        /// <summary>
        /// Returns the next Id for objects to use.
        /// </summary>
        /// <returns>Unique int identifier.</returns>
        public int CreateNewUniqueId()
        {
            return NextAvailableId++;
        }
        
        /// <summary>
        /// Starts the GameSimulation running asychrnously.
        /// </summary>
        public void Start()
        {
            Running = true;
            GameSimulationThread.Start();
        }


        /// <summary>
        /// Stops the game simulation.
        /// </summary>
        public void Stop()
        {
            Running = false;
            GameSimulationThread.Join();
        }

        /// <summary>
        /// Adds an object to the game simulation.
        /// </summary>
        /// <param name="obj">Object to add.</param>
        public void AddObject(IGameObject obj)
        {
            try
            {
                lock (ObjectsInGame)
                {
                    lock (WorldGrid)
                    {
                        ObjectsInGame.TryAdd(obj.GetId(), obj);
                        WorldGrid.Add(obj);
                    }
                }
            }
            catch(Exception e)
            {
                Trace.WriteLine("Exception in GameSimulation.AddObject:");
                Trace.WriteLine(e.Message);
            }
        }

        /// <summary>
        /// Removes an object from the game simulation.
        /// </summary>
        /// <param name="obj">Object to remove.</param>
        public void RemoveObject(IGameObject obj)
        {
            try
            {
                lock (ObjectsInGame)
                {
                    lock (WorldGrid)
                    {
                        WorldGrid.Remove(obj);
                        ObjectsInGame.TryRemove(obj.GetId(), out obj);
                    }
                }
            }
            catch (Exception e)
            {
                Trace.WriteLine("Exception in GameSimulation.RemoveObject:");
                Trace.WriteLine(e.Message);
            }
        }

        /// <summary>
        /// Returns a reference to the objects with a given id.
        /// </summary>
        /// <param name="id">Id of object.</param>
        /// <returns>Reference to object.</returns>
        public IGameObject GetObject(int id)
        {
            lock(ObjectsInGame)
            {
                return ObjectsInGame[id];
            }
        }

        /// <summary>
        /// Returns a new list of all objects in the game.
        /// </summary>
        /// <returns>List of objects in game.</returns>
        public List<IGameObject> GetAllObjects()
        {
            lock(ObjectsInGame)
            {
                return new List<IGameObject>(ObjectsInGame.Values);
            }
        }

        /// <summary>
        /// Returns a list of all objects in the cells around a
        /// given position.
        /// </summary>
        /// <param name="position">Reference point.</param>
        /// <returns>List of nearby objects.</returns>
        public List<IGameObject> GetNearbyObjects(Point position)
        {
            lock (WorldGrid)
            {
                return WorldGrid.GetObjectsInCellsAroundPoint(position);
            }
        }

        /// <summary>
        /// Returns a list (which can be empty) of all objects currently
        /// colliding with a given object.
        /// </summary>
        /// <param name="obj">Reference object.</param>
        /// <returns>List of colliding objects.</returns>
        public List<IGameObject> GetCollidingObjects(IGameObject obj)
        {
            if(!(obj is CollidableObject))
            {
                throw (new ArgumentException());
            }

            List<IGameObject> objects = new List<IGameObject>();
            List<IGameObject> nearbyObjects = 
                GetNearbyObjects(obj.GetPosition());

            foreach(IGameObject testObj in nearbyObjects)
            {
                if (testObj != obj)
                {
                    if (testObj is CollidableObject)
                    {
                        if (((CollidableObject)obj).Intersects((CollidableObject)testObj))
                        {
                            objects.Add(testObj);
                        }
                    }
                }
            }

            return objects;
        }

        /// <summary>
        /// Runs the game loop.
        /// </summary>
        private void GameSimulationLoop()
        {
            while(Running)
            {
                try
                {
                    // Execute game loop
                    TimeSpan gameTime = DateTime.Now - LastUpdate;
                    LastUpdate = DateTime.Now;

                    lock (ObjectsInGame)
                    {
                        lock (WorldGrid)
                        {
                            foreach (IGameObject obj in ObjectsInGame.Values)
                            {
                                try
                                {
                                    obj.Update(gameTime);
                                    WorldGrid.Update(obj);
                                }
                                catch
                                {
                                }
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    Trace.WriteLine("Exception in GameSimulation" +
                        ".GameSimulationLoop:");
                    Trace.WriteLine(e.Message);
                }
                finally
                {
                    Thread.Sleep(15);
                }
            }

            lock (ObjectsInGame)
            {
                lock (WorldGrid)
                {
                    // Dispose all objects
                    ObjectsInGame.Clear();
                    WorldGrid.Clear();
                }
            }
        }
    }
}
