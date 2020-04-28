using System;

using Microsoft.Xna.Framework;

using GameInstanceServer.Game.Objects.Common;
using GameInstanceServer.Game.World;
using GameInstanceServer.Systems.ECS;

namespace GameInstanceServer.Game.Objects.Resources
{
    /// <summary>
    /// Debri resource that yields low value resources, but high quantities
    /// exist in the world.
    /// </summary>
    public class Debri : Entity
    {
        /// <summary>
        /// This is a serializable entity.
        /// </summary>
        public override bool Serializable => true;

        /// <summary>
        /// Position component of entity.
        /// </summary>
        public PositionComponent Position => (PositionComponent)Components[0];

        /// <summary>
        /// Creates a new instance of a debri entity.
        /// </summary>
        /// <param name="spawn">Spawn location.</param>
        public Debri(Vector2 spawn) : base(ECS.GetNextId())
        {
            Random random = new Random();

            // Components
            Components = new IComponent[]
            {
                new PositionComponent()
                {
                    Position = spawn,
                    Momentum = Blackhole.GetInitialMomentum(spawn),
                    AngularMomentum = random.Next(-40, 40),
                    Direction = random.Next(0, 360)
                },
                new AnimationComponent()
                {
                    Sprite = "Objects/Resources/Debri"
                },
                new AffectedByBlackholeComponent(),
                new WorldComponent()
                {
                    Entity = this
                }
            };

            // Link as required
            ((AffectedByBlackholeComponent)Components[2]).Position = Position;
            ((WorldComponent)Components[3]).PositionComponent = Position;
        }
    }
}
