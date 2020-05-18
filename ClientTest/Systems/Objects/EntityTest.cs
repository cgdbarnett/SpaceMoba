using System;
using Xunit;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Lidgren.Network;

using SpaceMobaClient.GamePlay;
using SpaceMobaClient.Systems.Objects;

namespace ClientTest.Systems.Objects
{
    /// <summary>
    /// A testing only implementation of IComponent.
    /// Set the id with the constructor, and update
    /// iterators the field value.
    /// </summary>
    public class TestComponent : IComponent
    {
        public int Value = 0;

        public byte Id
        {
            get;
            private set;
        }

        public bool WantsUpdates => true;

        public bool WantsDraws => false;

        public Entity Entity
        {
            get;
            set;
        }

        ComponentId IComponent.Id => (ComponentId)Id;

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            throw new NotImplementedException();
        }

        public void Update(GameTime gameTime)
        {
            Value++;

            // Destroy entity if the value reaches 21 exactly.
            if(Value == 21)
            {
                EntityManager.Remove(Entity);
            }

            // Creates an entity if the value reaches 31 exactly.
            if(Value == 31)
            {
                EntityManager.Add(new Entity(3));
            }
        }

        public void Draw(SpriteBatch spriteBatch, Camera camera)
        {
            throw new NotImplementedException();
        }

        public void Deserialize(NetIncomingMessage message)
        {
            throw new NotImplementedException();
        }

        public void Link()
        {
            throw new NotImplementedException();
        }

        public TestComponent(byte id)
        {
            Id = id;
        }
    }

    /// <summary>
    /// Tests the Entity class.
    /// </summary>
    public class EntityTest
    {
        private Camera camera = new Camera(0, 0, 1920, 1080);

        /// <summary>
        /// Tests the indexer returns the correct entity.
        /// </summary>
        [Fact]
        public void Indexer()
        {
            // Set up test by adding a component to an entity.
            Entity entity = new Entity(0);
            TestComponent component = new TestComponent(1);
            entity.AddOrUpdateComponent(component);
            entity.AddOrUpdateComponent(new TestComponent(0));

            // Index entity by id, and test
            IComponent test1 = entity[(ComponentId)1];
            IComponent test2 = entity[(ComponentId)0];

            Assert.NotNull(test1);
            Assert.NotNull(test2);
            Assert.NotSame(test1, test2);
            Assert.Equal(1, (byte)test1.Id);
            Assert.Equal(0, (byte)test2.Id);
            Assert.Same(component, test1);
        }

        /// <summary>
        /// Tests that adding a component with the same id as an existing
        /// component performs an update.
        /// </summary>
        [Fact]
        public void AddOrUpdate()
        {
            // Set up test by adding a component to an entity.
            Entity entity = new Entity(0);
            TestComponent component = new TestComponent(1);
            entity.AddOrUpdateComponent(component);
            entity.AddOrUpdateComponent(new TestComponent(0));

            // Get Value of index 1:
            int value = ((TestComponent)entity[(ComponentId)1]).Value;

            // Create new test component, and add to entity
            TestComponent componentNew = new TestComponent(1)
            {
                Value = 10
            };
            entity.AddOrUpdateComponent(componentNew);

            // Index new component, and get value
            IComponent test = entity[(ComponentId)1];
            int valueNew = ((TestComponent)test).Value;

            // Run tests
            Assert.NotSame(component, componentNew);
            Assert.NotSame(component, test);
            Assert.Same(componentNew, test);
            Assert.NotEqual(value, valueNew);
            Assert.Equal(10, valueNew);
        }

        /// <summary>
        /// Tests the Update method of an entity.
        /// </summary>
        [Fact]
        public void Update()
        {
            // Set up test by adding a component to an entity.
            Entity entity = new Entity(0);
            TestComponent component = new TestComponent(1)
            {
                Value = 10
            };
            entity.AddOrUpdateComponent(component);
            entity.AddOrUpdateComponent(new TestComponent(0));

            int value0Before = ((TestComponent)entity[(ComponentId)0]).Value;
            int value1Before = ((TestComponent)entity[(ComponentId)1]).Value;

            // No methods use the game time, so null is okay.
            entity.Update(null, camera, Mouse.GetState());

            int value0After = ((TestComponent)entity[(ComponentId)0]).Value;
            int value1After = ((TestComponent)entity[(ComponentId)1]).Value;

            // Check to expected values
            Assert.Equal(0, value0Before);
            Assert.Equal(10, value1Before);
            Assert.Equal(1, value0After);
            Assert.Equal(11, value1After);
        }
    }
}
