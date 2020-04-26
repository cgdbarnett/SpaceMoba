using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;

using GameInstanceServer.Game;
using GameInstanceServer.Game.Objects.Ships;
using GameInstanceServer.Systems;

namespace GameInstanceServer.Map
{
    /// <summary>
    /// Singleton instance that loads map data into game.
    /// </summary>
    public static class MapData
    {
        public static readonly Point Dimensions = new Point(12000, 12000);

        /// <summary>
        /// Spawns all objects that should exist when the game starts.
        /// </summary>
        public static void SpawnWorld()
        {
            // Random droid that just chills
            /*Ship droid = new Ship(GameSimulation.GetGameSimulation()
                .CreateNewUniqueId());
            droid.SetPosition(new Point(5500, 200));

            GameSimulation.GetGameSimulation().AddObject(droid);*/
        }

        /// <summary>
        /// Spawns the player object for a given client.
        /// </summary>
        /// <param name="client">Client.</param>
        public static void SpawnPlayerObject(Client client)
        {
            // Create player ship object
            /*PlayerShip newShip = new PlayerShip(
                        GameSimulation.GetGameSimulation().CreateNewUniqueId()
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
            GameSimulation.GetGameSimulation().AddObject(newShip);

            // Assign to player
            client.GameObject = newShip;*/
        }
    }
}
