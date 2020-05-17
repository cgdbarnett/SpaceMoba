using System;
using System.Collections.Generic;
using System.Diagnostics;

using Microsoft.Xna.Framework;

using GameInstanceServer.Game.Teams;
using GameInstanceServer.Game.World;
using GameInstanceServer.Systems.ECS;

// Modify component system id from ECS for a new id.
namespace GameInstanceServer.Systems.ECS
{
    // Extend class
    public partial class ComponentSystemId
    {
        // Create Id for Projectile System.
        public static readonly ComponentSystemId ProjectileSystem
            = new ComponentSystemId();
    }
}

namespace GameInstanceServer.Game.Objects.Combat
{
    /// <summary>
    /// Runs the projectile components that check for collsions
    /// with combat objects.
    /// </summary>
    public class ProjectileSystem : ComponentSystem
    {
        /// <summary>
        /// Creates a ProjectileSystem.
        /// </summary>
        public ProjectileSystem()
        {
            Id = ComponentSystemId.ProjectileSystem;
            WantsUpdates = true;
        }

        /// <summary>
        /// Updates a projectile component.
        /// </summary>
        /// <param name="component">Component to update.</param>
        /// <param name="gameTime">Game frame interval.</param>
        protected override void UpdateComponent(
            IComponent component, TimeSpan gameTime
            )
        {
            ProjectileComponent projectile = (ProjectileComponent)component;

            // Update collision mask
            // Todo(Chris): Remove magic number, length.
            projectile.CollisionMask.Start = projectile.Position.Position;
            projectile.CollisionMask.End = new Vector2(
                (float)Math.Cos(MathHelper.ToRadians(projectile.Position.Direction)),
                (float)Math.Sin(MathHelper.ToRadians(projectile.Position.Direction))
                ) * 20 + projectile.CollisionMask.Start;

            // Get nearby entities
            List<Entity> entities = 
                projectile.World.Cell.GetNearbyObjects(
                    projectile.Position.Position
                    );
            foreach(Entity entity in entities)
            {
                IComponent combat = 
                    entity.GetComponent(ComponentSystemId.CombatSystem);
                if(combat != null)
                {
                    // Check for collision
                    if(
                        ((CombatComponent)combat)
                        .CollisionMask
                        .TestCollision(projectile.CollisionMask)
                        )
                    {
                        // Check team
                        IComponent team = 
                            entity.GetComponent(ComponentSystemId.TeamSystem);

                        if (team != null)
                        {
                            if (((TeamComponent)team).Team != projectile.Team)
                            {
                                // Apply damage
                                ApplyDamage((CombatComponent)combat);

                                // Destroy self, and stop loop.
                                projectile.Entity.UnregisterComponents();
                                break;
                            }
                        }
                        else
                        {
                            // Apply damage
                            ApplyDamage((CombatComponent)combat);

                            // Destroy self, and stop loop.
                            projectile.Entity.UnregisterComponents();
                            break;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Applies damage to the target.
        /// </summary>
        /// <param name="target">Target to apply damage to.</param>
        protected void ApplyDamage(CombatComponent target)
        {
            target.Health -= 20;
        }
    }
}
