using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;

using Lidgren.Network;

using GameInstanceServer.Game.Objects.Common;
using GameInstanceServer.Game.Objects.Resources;
using GameInstanceServer.Game.Teams;
using GameInstanceServer.Game.World;
using GameInstanceServer.Systems.ECS;

namespace GameInstanceServer.Game.Objects.Ships
{
    /// <summary>
    /// The mothership is the primary base for the game. Serving as the place
    /// where droids are made, and the primary objective to destroy of the 
    /// opposition.
    /// </summary>
    public class Mothership : Entity
    {
        /// <summary>
        /// This is a serializable object.
        /// </summary>
        public override bool Serializable => true;

        /// <summary>
        /// Position component of mothership.
        /// </summary>
        public PositionComponent Position => (PositionComponent)Components[0];

        /// <summary>
        /// World component of mothership.
        /// </summary>
        public WorldComponent World => (WorldComponent)Components[1];

        /// <summary>
        /// Team component of mothership.
        /// </summary>
        public TeamComponent Team => (TeamComponent)Components[4];

        /// <summary>
        /// Resources component of mothership.
        /// </summary>
        public ResourceComponent Resources => (ResourceComponent)Components[5];

        /// <summary>
        /// Creates a Mothership entity.
        /// </summary>
        public Mothership(int x, int y, Team team) : base(ECS.GetNextId())
        {
            // Register components.
            Components = new IComponent[]
            {
                new PositionComponent()
                {
                    Position = new Vector2(x, y),
                    Momentum = Blackhole.GetInitialMomentum(
                        new Vector2(x, y)
                        ),
                    Direction = 0,
                    AngularMomentum = 0
                },
                new WorldComponent(),
                new AnimationComponent()
                {
                    Sprite = "Objects/Ships/Mothership"
                },
                new AffectedByBlackholeComponent(),
                new TeamComponent()
                {
                    Team = team
                },
                new ResourceComponent()
                {
                    Value = 0
                }
            };

            // Link components
            World.Entity = this;
            World.PositionComponent = Position;
            ((AffectedByBlackholeComponent)Components[3]).Position = Position;
            Team.Team.Mothership = this;
        }
    }
}
