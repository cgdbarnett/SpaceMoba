using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;

using GameInstanceServer.Game;
using GameInstanceServer.Game.Objects;
using GameInstanceServer.Systems;

namespace GameInstanceServer.Map
{
    /// <summary>
    /// Singleton instance that loads map data into game.
    /// </summary>
    public class MapData
    {
        // Private singleton reference to MapData instance.
        private static MapData Instance;

        private GameSimulation GameSimulation;

        /// <summary>
        /// Private constructor of singleton.
        /// </summary>
        private MapData()
        {
            GameSimulation = GameSimulation.GetGameSimulation();
        }

        /// <summary>
        /// Returns a reference to the MapData instance.
        /// </summary>
        /// <returns>MapData reference.</returns>
        public static MapData GetMapData()
        {
            if(Instance == null)
            {
                Instance = new MapData();
            }

            return Instance;
        }

        /// <summary>
        /// Spawns all objects that should exist when the game starts.
        /// </summary>
        public void SpawnWorld()
        {
            // Random droid that just chills
            Ship droid = new Ship(GameSimulation.CreateNewUniqueId());
            droid.SetPosition(new Point(5500, 200));

            GameSimulation.AddObject(droid);
        }

        /// <summary>
        /// Spawns the player object for a given client.
        /// </summary>
        /// <param name="client">Client.</param>
        public void SpawnPlayerObject(Client client)
        {
            // Create player ship object
            PlayerShip newShip = new PlayerShip(
                        GameSimulation.CreateNewUniqueId()
                        );

            // Move ship into position
            switch(client.Team)
            {
                // North spawn point
                case 0:
                    newShip.SetPosition(
                        new Point(5000, 100)
                        );
                    break;

                // South spawn point
                case 1:
                    newShip.SetPosition(
                        new Point(5000, 9900)
                        );
                    break;
            }

            // Add to game simulation
            GameSimulation.AddObject(newShip);

            // Assign to player
            client.GameObject = newShip;
        }
    }
}
