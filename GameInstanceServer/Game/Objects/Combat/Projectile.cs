using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;

using GameInstanceServer.Game.Objects.Common;
using GameInstanceServer.Game.Teams;
using GameInstanceServer.Game.World;
using GameInstanceServer.Systems.ECS;
using GameInstanceServer.Systems.Physics;

namespace GameInstanceServer.Game.Objects.Combat
{
    /// <summary>
    /// A projectile entity. (Line).
    /// </summary>
    public class Projectile : Entity
    {
        /// <summary>
        /// This is a serializable entity.
        /// </summary>
        public override bool Serializable => true;

        /// <summary>
        /// Gets the position component of this entity.
        /// </summary>
        public PositionComponent Position => (PositionComponent)Components[0];

        /// <summary>
        /// Gets the world component of this entity.
        /// </summary>
        public WorldComponent World => (WorldComponent)Components[2];

        /// <summary>
        /// Gets the lifetime component of this entity.
        /// </summary>
        public LifetimeComponent Lifetime => (LifetimeComponent)Components[3];

        public ProjectileComponent ProjectileComponent
            => (ProjectileComponent)Components[4];

        /// <summary>
        /// Creates a new instance of a projectile.
        /// </summary>
        /// <param name="x">Initial x position.</param>
        /// <param name="y">Initial y position.</param>
        /// <param name="direction">Initial direction.</param>
        /// <param name="team">Team who owns projectile.</param>
        public Projectile(float x, float y, float direction, Team team = null)
            : base(ECS.GetNextId())
        {
            // Todo(Chris): Get rid of magic numbers.
            Components = new IComponent[]
            {
                new PositionComponent()
                {
                    Position = new Vector2(x, y),
                    Direction = direction,
                    Momentum = new Vector2(
                        (float)Math.Cos(MathHelper.ToRadians(direction)),
                        (float)Math.Sin(MathHelper.ToRadians(direction))
                        ) * 1000,
                    AngularMomentum = 0,
                    CollisionMask = new CollisionMaskLine(
                        new Vector2(x, y), 20, direction
                        )
                },
                new AnimationComponent()
                {
                    Sprite = "Objects/Weapons/line_projectile"
                },
                new WorldComponent(),
                new LifetimeComponent()
                {
                    LifetimeRemaining = 1000
                },
                new ProjectileComponent()
                {
                    Team = team
                }
            };

            // Link components:
            World.Entity = this;
            World.PositionComponent = Position;
            Lifetime.Entity = this;
            ProjectileComponent.Entity = this;
            ProjectileComponent.Position = Position;
            ProjectileComponent.World = World;


            // Register components
            RegisterComponents();
        }
    }
}
