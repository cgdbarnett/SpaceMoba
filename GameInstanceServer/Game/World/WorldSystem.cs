using System;
using GameInstanceServer.Systems.ECS;

// Modify component system id from ECS for a new id.
namespace GameInstanceServer.Systems.ECS
{
    // Extend class
    public partial class ComponentSystemId
    {
        // Create Id for Position System.
        public static readonly ComponentSystemId WorldSystem
            = new ComponentSystemId();
    }
}

namespace GameInstanceServer.Game.World
{
    /// <summary>
    /// World System. Runs the world grid for spatial mapping.
    /// </summary>
    public class WorldSystem : ComponentSystem
    {
        // Grid containing all objects.
        private readonly WorldGrid WorldGrid;

        // Size of cells in grid.
        private const int CellSize = 2000;

        /// <summary>
        /// Creates a WorldSystem.
        /// </summary>
        public WorldSystem() : base()
        {
            Id = ComponentSystemId.WorldSystem;
            WantsUpdates = true;

            // Create world grid
            WorldGrid = new WorldGrid(
                Map.MapData.Dimensions.X, Map.MapData.Dimensions.Y,
                CellSize, CellSize
                );
        }

        /// <summary>
        /// Insert the component into the WorldGrid.
        /// </summary>
        /// <param name="id">Entity id.</param>
        /// <param name="component">WorldComponent.</param>
        public override void RegisterComponent(int id, IComponent component)
        {
            base.RegisterComponent(id, component);

            try
            {
                WorldGrid.Add((WorldComponent)component);
            }
            catch
            {
                // Handle? or ignore
            }
        }

        /// <summary>
        /// Insert the component into the WorldGrid.
        /// </summary>
        /// <param name="id">Entity id.</param>
        /// <param name="component">WorldComponent.</param>
        public override void UnregisterComponent(int id)
        {
            WorldComponent component = (WorldComponent)Components[id];
            base.UnregisterComponent(id);

            try
            {
                WorldGrid.Remove(component);
            }
            catch
            {
                // Handle? or ignore
            }
        }

        /// <summary>
        /// Update a component within the grid.
        /// </summary>
        /// <param name="component">Component to update.</param>
        /// <param name="gameTime">Game frame interval.</param>
        protected override void UpdateComponent(IComponent component, TimeSpan gameTime)
        {
            WorldGrid.Update((WorldComponent)component);
        }
    }
}
