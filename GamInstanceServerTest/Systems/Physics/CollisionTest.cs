using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

using Microsoft.Xna.Framework;

using GameInstanceServer.Systems.Physics;

namespace GameInstanceServerTest.Systems.Physics
{
    public class CollisionTest
    {
        /// <summary>
        /// Tests Collision.TestCollisionLineCircle
        /// </summary>
        [Fact]
        public void TestCollisionLineCircle()
        {
            CollisionMaskLine line = new CollisionMaskLine(
                new Vector2(5, 20), new Vector2(20, 10)
                );
            CollisionMaskCircle circle = new CollisionMaskCircle(
                new Vector2(30, 30), 15);

            //Assert.True(Collision.TestCollisionLineCircle(circle, line));

            circle.Center.X = 50;

            Assert.False(Collision.TestCollisionLineCircle(circle, line));

            line.End.X = 50;
            line.End.Y = 30;
            Assert.True(Collision.TestCollisionLineCircle(circle, line));

            line.Start.X = 49;
            line.End.Y = 49;
            Assert.True(Collision.TestCollisionLineCircle(circle, line));
        }
    }
}