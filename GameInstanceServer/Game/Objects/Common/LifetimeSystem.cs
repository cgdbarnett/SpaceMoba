using System;
using System.Collections.Generic;
using System.Text;

using GameInstanceServer.Systems.ECS;

// Modify component system id from ECS for a new id.
namespace GameInstanceServer.Systems.ECS
{
    // Extend class
    public partial class ComponentSystemId
    {
        // Create Id for Resource System.
        public static readonly ComponentSystemId LifetimeSystem
            = new ComponentSystemId();
    }
}

namespace GameInstanceServer.Game.Objects.Common
{
    /// <summary>
    /// Manages entities that have a limited lifespan.
    /// </summary>
    public class LifetimeSystem : ComponentSystem
    {
        /// <summary>
        /// Creates a LifetimeSystem.
        /// </summary>
        public LifetimeSystem()
        {
            Id = ComponentSystemId.LifetimeSystem;
            WantsUpdates = true;
        }

        /// <summary>
        /// Updates a lifetime component.
        /// </summary>
        /// <param name="component">Component to update.</param>
        /// <param name="gameTime">Game frame interval.</param>
        protected override void UpdateComponent(
            IComponent component, TimeSpan gameTime
            )
        {
            LifetimeComponent lifetime = (LifetimeComponent)component;
            lifetime.LifetimeRemaining -= (int)gameTime.TotalMilliseconds;

            if(lifetime.LifetimeRemaining <= 0)
            {
                // DESTROY
                lifetime.Entity.UnregisterComponents();
            }
        }
    }
}
