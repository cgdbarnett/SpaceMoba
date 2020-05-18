using System;

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
                combat.Entity.UnregisterComponents();
            }
        }
    }
}