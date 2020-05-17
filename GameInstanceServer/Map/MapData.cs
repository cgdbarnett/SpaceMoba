using System;

using Microsoft.Xna.Framework;

using GameInstanceServer.Game.Objects.Common;
using GameInstanceServer.Game.Objects.Resources;
using GameInstanceServer.Game.Objects.Ships;
using GameInstanceServer.Game.Teams;

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
        public static void SpawnWorld(Team teamA, Team teamB)
        {
            const int debriCount = 160;
            Random random = new Random();

            // BLACK HOLE OF DOOM
            Blackhole blackhole = new Blackhole(new Vector2(6000, 6000));

            // Spawn motherships
            Mothership mothershipA = new Mothership(6000, 1600, teamA);
            Mothership mothershipB = new Mothership(6000, 10400, teamB);

            // Fill the void with debri
            for(int i = 0; i < debriCount; i++)
            {
                float dir = (float)(random.NextDouble() * Math.PI * 2);
                float len = random.Next(2000, 5000);
                float xpos = 6000 + len * (float)Math.Cos(dir);
                float ypos = 6000 + len * (float)Math.Sin(dir);

                Debri debri = new Debri(new Vector2(xpos, ypos));
            }
        }
    }
}
