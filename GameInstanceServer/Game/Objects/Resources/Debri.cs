using System;

using Microsoft.Xna.Framework;

using GameInstanceServer.Game.Objects.Combat;
using GameInstanceServer.Game.Objects.Common;
using GameInstanceServer.Game.World;
using GameInstanceServer.Systems.ECS;
using GameInstanceServer.Systems.Physics;

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
        /// Combat component of entity.
        /// </summary>
        public CombatComponent Combat => (CombatComponent)Components[4];

        /// <summary>
        /// Resource component of entity.
        /// </summary>
        public ResourceComponent Resources => (ResourceComponent)Components[5];

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
                    Direction = random.Next(0, 360),
                    CollisionMask = new CollisionMaskCircle(
                        spawn, 32
                        )
                },
                new AnimationComponent()
                {
                    Sprite = "Objects/Resources/Debri"
                },
                new AffectedByBlackholeComponent()
                {
                    Entity = this
                },
                new WorldComponent()
                {
                    Entity = this
                },
                new CombatComponent()
                {
                    Health = 1,
                    MaxHealth = 1,
                    Armour = 0,
                    MaxArmour = 0,
                    Entity = this
                },
                new ResourceComponent()
                {
                    Value = 50
                }
            };

            // Link as required
            ((AffectedByBlackholeComponent)Components[2]).Position = Position;
            ((WorldComponent)Components[3]).PositionComponent = Position;

            // Register components
            RegisterComponents();
        }

        /// <summary>
        /// Destroys the debris. If the cause of death is player attacking,
        /// then award them resources.
        /// </summary>
        /// <param name="reason">Reason for destroying.</param>
        public override void Destroy(
            DestroyReason reason = DestroyReason.Normal
            )
        {
            switch(reason)
            {
                case DestroyReason.CombatEvent:
                    // If the last entity has a resource component,
                    // the resource value of this entity should be passed to
                    // the attacker.
                    if (Combat.DamageLog.Count > 0)
                    {
                        Entity attacker = Combat
                            .DamageLog[Combat.DamageLog.Count - 1].Attacker;

                        IComponent attackerResources = attacker.GetComponent(
                                ComponentSystemId.ResourceSystem
                                );
                        if(attackerResources != null)
                        {
                            ((ResourceComponent)attackerResources).Value += 
                                Resources.Value;
                        }
                    }
                    break;
            }
            base.Destroy();
        }
    }
}
