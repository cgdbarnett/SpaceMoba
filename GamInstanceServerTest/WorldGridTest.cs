using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

using Microsoft.Xna.Framework;

using GameInstanceServer.Game.Objects.Common;
using GameInstanceServer.Game.World;
using GameInstanceServer.Systems.ECS;

namespace GamInstanceServerTest
{
    /// <summary>
    /// Suite of tests for WorldGrid.
    /// </summary>
    public class WorldGridTest
    {
        /// <summary>
        /// Tests WorldGrid.GetCellFromPoint()
        /// </summary>
        [Fact]
        public void GetCellFromPoint()
        {
            WorldGrid grid = new WorldGrid(10000, 10000, 1000, 1000);

            Assert.Equal(
                new Tuple<int, int>(6, 1),
                grid.GetCellFromPoint(new Point(5200, 120))
                );
            Assert.Equal(
                new Tuple<int, int>(1, 1),
                grid.GetCellFromPoint(new Point(100, 100))
                );
            Assert.Throws<ArgumentOutOfRangeException>(
                () => grid.GetCellFromPoint(new Point(-200, 200))
                );
        }

        /// <summary>
        /// Tests WorldGrid.GetObjectsInCellsAroundPoint()
        /// </summary>
        [Fact]
        public void GetObjectsInCellsAroundPoint()
        {
            WorldGrid grid = GetPopulatedWorldGrid();
            // Set A
            {
                int[] ids = new int[] { 4, 5, 6, 8, 9 };

                List<Entity> objs = grid.GetObjectsInCellsAroundPoint(
                    new Point(450, 450)
                    );
                objs.OrderBy(entity => entity.Id);

                Assert.Equal(ids.Length, objs.Count);
                for (int i = 0; i < ids.Length; i++)
                {
                    Assert.Equal(ids[i], objs[i].Id);
                }
            }

            // Set B
            {
                int[] ids = new int[] { 0, 1, 4, 5, 6 };

                List<Entity> objs = grid.GetObjectsInCellsAroundPoint(
                    new Point(300, 300)
                    );
                objs.OrderBy(entity => entity.Id);

                Assert.Equal(ids.Length, objs.Count);
                for (int i = 0; i < ids.Length; i++)
                {
                    Assert.Equal(ids[i], objs[i].Id);
                }
            }

            // Set C
            {
                int[] ids = new int[] { 0, 4 };

                List<Entity> objs = grid.GetObjectsInCellsAroundPoint(
                    new Point(100, 100)
                    );
                objs.OrderBy(entity => entity.Id);

                Assert.Equal(ids.Length, objs.Count);
                for (int i = 0; i < ids.Length; i++)
                {
                    Assert.Equal(ids[i], objs[i].Id);
                }
            }
        }

        /// <summary>
        /// Returns a populated world grid for testing.
        /// </summary>
        /// <returns></returns>
        private WorldGrid GetPopulatedWorldGrid()
        {
            WorldGrid grid = new WorldGrid(800, 800, 200, 200);

            // Populate
            float[] posX = new float[]
            {
                250, 450, 650, 750,
                210, 450,
                500,
                50, 250, 750
            };
            float[] posY = new float[]
            {
                50, 50, 50, 150,
                300, 300,
                500,
                700, 700, 700
            };
            for (int i = 0; i < posX.Length; i++)
            {
                WorldComponent comp = new WorldComponent();
                comp.Entity = new Entity(i);
                comp.PositionComponent = new PositionComponent();
                comp.PositionComponent.Position =
                    new Vector2(posX[i], posY[i]);

                grid.Add(comp);
            }

            return grid;
        }
    }
}
