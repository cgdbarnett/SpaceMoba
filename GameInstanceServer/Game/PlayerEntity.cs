using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;

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
        /// Gets the engine component for the player.
        /// </summary>
        public EngineComponent Engine => (EngineComponent)Components[4];

        /// <summary>
        /// Creates a new player entity.
        /// </summary>
        public PlayerEntity() : base(ECS.GetNextId())
        {
            // Create components
            Components = new IComponent[]
            {
                new NetworkingClientComponent(this),
                new PositionComponent()
                {
                    Position = new Vector2(),
                    Momentum = new Vector2(),
                    AngularMomentum = 0,
                    Direction = 0
                },
                new WorldComponent(),
                new AnimationComponent()
                {
                    Sprite = "Objects/Ships/GreenBeacon"
                },
                new EngineComponent()
                {
                    InputForce = new Vector2()
                }
            };

            // Link components as required
            Client.Entity = this;
            World.PositionComponent = Position;
            World.Entity = this;
            Engine.Position = Position;
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
