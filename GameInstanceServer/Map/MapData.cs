using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;

using GameInstanceServer.Game.Objects.Common;
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
            // BLACK HOLE OF DOOM
            Blackhole blackhole = new Blackhole(new Vector2(6000, 6000));
            blackhole.RegisterComponents();

            // Mothership of lessor doom
            Mothership mothership = new Mothership();
            mothership.RegisterComponents();
        }
    }
}
