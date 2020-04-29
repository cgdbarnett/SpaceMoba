using System;
using Xunit;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Lidgren.Network;

using SpaceMobaClient.GamePlay;
using SpaceMobaClient.Systems.Objects;

namespace ClientTest.Systems.Objects
{
    /// <summary>
    /// Tests the EntityManager class.
    /// </summary>
    public class EntityManagerTest
    {
        /// <summary>
        /// Tests if removing an entity during the update
        /// process behaves as expected.
        /// </summary>
        [Fact]
        public void RemoveEntityWhileUpdating()
        {
            EntityManager.Clear();

            Entity test = new Entity(1);
            test.AddOrUpdateComponent(
                new TestComponent(0)
                {
                    Value = 20,
                    Entity = test
                }
                );

            EntityManager.Add(new Entity(0));
            EntityManager.Add(test);
            EntityManager.Add(new Entity(2));

            EntityManager.Update(null);
            EntityManager.Update(null);

            Assert.False(EntityManager.Contains(1));
        }

        /// <summary>
        /// Tests if adding an entity during the update
        /// process behaves as expected.
        /// </summary>
        [Fact]
        public void AddEntityWhileUpdating()
        {
            EntityManager.Clear();

            Entity test = new Entity(1);
            test.AddOrUpdateComponent(
                new TestComponent(0)
                {
                    Value = 30,
                    Entity = test
                }
                );

            EntityManager.Add(new Entity(0));
            EntityManager.Add(test);
            EntityManager.Add(new Entity(2));

            EntityManager.Update(null);
            EntityManager.Update(null);

            Assert.True(EntityManager.Contains(3));
        }

        /// <summary>
        /// Tests adding and removing an entity from the manager.
        /// </summary>
        [Fact]
        public void AddRemoveEntity()
        {
            EntityManager.Clear();
            EntityManager.Add(new Entity(0));
            EntityManager.Add(new Entity(1));
            EntityManager.Add(new Entity(2));
            EntityManager.Update(null);

            Assert.True(EntityManager.Contains(1));
            EntityManager.Remove(1);
            EntityManager.Update(null);
            Assert.False(EntityManager.Contains(1));
        }
    }
}
