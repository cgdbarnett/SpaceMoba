using System.Collections.Generic;
using GameInstanceServer.Systems.ECS;
using Microsoft.Xna.Framework;

namespace GameInstanceServer.Game.World
{
    /// <summary>
    /// An individual cell of the WorldGrid, which
    /// contains references to the objects in this cell.
    /// </summary>
    public class WorldCell
    {
        // Parent grid
        private readonly WorldGrid Parent;

        // List of objects in cell
        private Dictionary<WorldComponent, Entity> ObjectsInCell;

        /// <summary>
        /// Creates an instance of a world cell.
        /// </summary>
        /// <param name="parent">Parent WorldGrid</param>
        public WorldCell(WorldGrid parent)
        {
            Parent = parent;
            ObjectsInCell = new Dictionary<WorldComponent, Entity>();
        }

        /// <summary>
        /// Returns a list of all objects in the cell.
        /// </summary>
        /// <returns>List of objects.</returns>
        public List<Entity> GetObjects()
        {
            return new List<Entity>(ObjectsInCell.Values);
        }

        /// <summary>
        /// Returns a list of all objects in surounding cells.
        /// </summary>
        /// <param name="position">Position to test from.</param>
        /// <returns>List of nearby objects.</returns>
        public List<Entity> GetNearbyObjects(Vector2 position)
        {
            return Parent.GetObjectsInCellsAroundPoint(position.ToPoint());
        }

        /// <summary>
        /// Adds an object to this cell.
        /// </summary>
        /// <param name="obj">Object to add.</param>
        public void Add(WorldComponent obj)
        {
            ObjectsInCell.Add(obj, obj.Entity);
        }

        /// <summary>
        /// Trys to remove an object from this cell.
        /// </summary>
        /// <param name="obj">Object to remove.</param>
        public void Remove(WorldComponent obj)
        {
            try
            {
                ObjectsInCell.Remove(obj);
            }
            catch
            {
            }
        }

        /// <summary>
        /// Empties the cell entirely.
        /// </summary>
        public void Clear()
        {
            ObjectsInCell.Clear();
        }
    }
}
