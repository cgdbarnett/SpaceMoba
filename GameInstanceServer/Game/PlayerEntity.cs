using System;
using System.Collections.Generic;
using System.Text;

using GameInstanceServer.Game.Objects.Common;
using GameInstanceServer.Game.World;
using GameInstanceServer.Systems.ECS;
using GameInstanceServer.Systems.Networking;

namespace GameInstanceServer.Game
{
    /// <summary>
    /// A client controlled player entity.
    /// </summary>
    public class PlayerEntity : Entity
    {
        /// <summary>
        /// Serialize this entity.
        /// </summary>
        public override bool Serializable => true;

        /// <summary>
        /// Gets the networking component for the player.
        /// </summary>
        public NetworkingClientComponent Client
        {
            get
            {
                return (NetworkingClientComponent)Components[0];
            }
        }

        /// <summary>
        /// Gets the position component for the player.
        /// </summary>
        public PositionComponent Position
        {
            get
            {
                return (PositionComponent)Components[1];
            }
        }

        /// <summary>
        /// Gets the world component for the player.
        /// </summary>
        public WorldComponent World
        {
            get
            {
                return (WorldComponent)Components[2];
            }
        }

        /// <summary>
        /// Gets the animation component for the player.
        /// </summary>
        public AnimationComponent Animation
        {
            get
            {
                return (AnimationComponent)Components[3];
            }
        }

        /// <summary>
        /// Creates a new player entity.
        /// </summary>
        public PlayerEntity() : base(ECS.GetNextId())
        {
            // Create components
            Components = new IComponent[]
            {
                new NetworkingClientComponent(this),
                new PositionComponent(),
                new WorldComponent(),
                new AnimationComponent(),
                new AffectedByBlackholeComponent()
            };

            // Link components as required
            Client.Entity = this;
            World.PositionComponent = Position;
            World.Entity = this;
            Animation.Sprite = "Objects/Ships/GreenBeacon";
            ((AffectedByBlackholeComponent)Components[4]).Position = Position;
        }

        /// <summary>
        /// Creates a new player, but copies game data from old object.
        /// </summary>
        /// <param name="oldPlayer">Previous iterate of same player.</param>
        public PlayerEntity(PlayerEntity oldPlayer) : base(ECS.GetNextId())
        {
            throw (new NotImplementedException());
        }
    }
}
