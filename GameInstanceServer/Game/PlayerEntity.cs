using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;

using GameInstanceServer.Game.Objects.Combat;
using GameInstanceServer.Game.Objects.Common;
using GameInstanceServer.Game.Objects.Resources;
using GameInstanceServer.Game.Objects.Ships;
using GameInstanceServer.Game.Teams;
using GameInstanceServer.Game.World;
using GameInstanceServer.Systems.ECS;
using GameInstanceServer.Systems.Networking;
using GameInstanceServer.Systems.Physics;

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
        /// Gets the ship limiting component for the player.
        /// </summary>
        public ShipLimiterComponent ShipLimiter => 
            (ShipLimiterComponent)Components[5];

        /// <summary>
        /// Gets the team component for this player.
        /// </summary>
        public TeamComponent Team => (TeamComponent)Components[6];

        /// <summary>
        /// Gets the combat component for this player.
        /// </summary>
        public CombatComponent Combat => (CombatComponent)Components[7];

        /// <summary>
        /// Gets the weapon component for this player.
        /// </summary>
        public WeaponComponent Weapon => (WeaponComponent)Components[8];

        /// <summary>
        /// Gets the resource component for this player.
        /// </summary>
        public ResourceComponent Resources => (ResourceComponent)Components[9];

        /// <summary>
        /// Creates a new player entity.
        /// </summary>
        public PlayerEntity(Team team) : base(ECS.GetNextId())
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
                },
                new ShipLimiterComponent()
                {
                    HandlingRank = 0,
                    SpeedRank = 0
                },
                new TeamComponent()
                {
                    Team = team
                },
                new CombatComponent()
                {
                    Health = 100,
                    MaxHealth = 100,
                    Armour = 100,
                    MaxArmour = 100,
                    CollisionMask = new CollisionMaskCircle(new Vector2(), 128),
                    Entity = this
                },
                new WeaponComponent()
                {
                    Cooldown = 200,
                    CurrentCooldown = 0,
                    Direction = 0f,
                    Trigger = false
                },
                new ResourceComponent()
                {
                    Value = 0
                }
            };

            // Link components as required
            Client.Entity = this;
            World.PositionComponent = Position;
            World.Entity = this;
            Engine.Position = Position;
            ShipLimiter.Position = Position;
            ShipLimiter.Engine = Engine;
            Weapon.Position = Position;
            Weapon.Team = Team;
            Combat.Position = Position;

            // Register to team
            Team.Team.RegisterPlayer(this);

            // Spawn according to team respawn
            Position.Position = Team.Team.Mothership.Position.Position
                + new Vector2(100, 0);
            Position.Momentum = 
                Blackhole.GetInitialMomentum(Position.Position);
            Position.Direction = (new Random()).Next(0, 180);
            Position.AngularMomentum = (new Random()).Next(-20, 20);

            // Register components
            RegisterComponents();
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
