using System;
using System.Diagnostics;

using GameInstanceServer.Systems.ECS;

// Modify component system id from ECS for a new id.
namespace GameInstanceServer.Systems.ECS
{
    // Extend class
    public partial class ComponentSystemId
    {
        // Create Id for Resource System.
        public static readonly ComponentSystemId CombatSystem
            = new ComponentSystemId();
    }
}

namespace GameInstanceServer.Game.Objects.Combat
{
    /// <summary>
    /// Combat system.
    /// </summary>
    public class CombatSystem : ComponentSystem
    {
        /// <summary>
        /// Creates a CombatSystem.
        /// </summary>
        public CombatSystem()
        {
            Id = ComponentSystemId.CombatSystem;
            WantsUpdates = true;
        }

        /// <summary>
        /// Applies damage to a component.
        /// </summary>
        /// <param name="component">CombatComponent to attack.</param>
        /// <param name="damage">Damage to deal.</param>
        /// <param name="attacker">Optional: Attacking entity.</param>
        public static void ApplyDamage(
            CombatComponent component, int damage, Entity attacker = null
            )
        {
            // Record into log
            Damage entry = new Damage()
            {
                Value = damage,
                Attacker = attacker
            };

            if (component.DamageLog.Count == 3)
            {
                component.DamageLog.RemoveAt(0);
            }
            component.DamageLog.Add(entry);

            // Apply to shield first
            if (component.Armour > 0)
            {
                component.Armour -= damage;
                if(component.Armour < 0)
                {
                    damage = -component.Armour;
                }
                else
                {
                    damage = 0;
                }
            }
            if(damage > 0)
            {
                component.Health -= damage;
            }
        }

        /// <summary>
        /// Update a component of the system.
        /// </summary>
        /// <param name="component">Component to update.</param>
        /// <param name="gameTime">Game frame interval.</param>
        protected override void UpdateComponent(
            IComponent component, TimeSpan gameTime
            )
        {
            CombatComponent combat = (CombatComponent)component;

            if(combat.Health <= 0)
            {
                combat.Entity.Destroy(Entity.DestroyReason.CombatEvent);
            }
        }
    }
}