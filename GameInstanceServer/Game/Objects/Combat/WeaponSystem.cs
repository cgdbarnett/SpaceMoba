using System;
using System.Diagnostics;

using GameInstanceServer.Game.Teams;
using GameInstanceServer.Systems.ECS;

// Modify component system id from ECS for a new id.
namespace GameInstanceServer.Systems.ECS
{
    // Extend class
    public partial class ComponentSystemId
    {
        // Create Id for Resource System.
        public static readonly ComponentSystemId WeaponSystem
            = new ComponentSystemId();
    }
}

namespace GameInstanceServer.Game.Objects.Combat
{
    /// <summary>
    /// Weapon system.
    /// </summary>
    public class WeaponSystem : ComponentSystem
    {
        /// <summary>
        /// Creates a WeaponSystem.
        /// </summary>
        public WeaponSystem()
        {
            Id = ComponentSystemId.WeaponSystem;
            WantsUpdates = true;
        }

        /// <summary>
        /// Updates a component within the system.
        /// </summary>
        /// <param name="component">Component to update.</param>
        /// <param name="gameTime">Game frame interval.</param>
        protected override void UpdateComponent(
            IComponent component, TimeSpan gameTime
            )
        {
            WeaponComponent weapon = (WeaponComponent)component;

            if(weapon.Trigger)
            {
                if(weapon.CurrentCooldown <= 0)
                {
                    // LETS FIRE
                    Projectile pulse = new Projectile(
                        weapon.Position.Position.X, weapon.Position.Position.Y
                        , weapon.Position.Direction, weapon.Team.Team
                        );
                    weapon.CurrentCooldown = weapon.Cooldown;
                }
            }

            weapon.CurrentCooldown -= (int)gameTime.TotalMilliseconds;
        }
    }
}