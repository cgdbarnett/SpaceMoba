using System.Collections.Generic;

using GameInstanceServer.Game.Objects;

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
        private List<IGameObject> ObjectsInCell;

        /// <summary>
        /// Creates an instance of a world cell.
        /// </summary>
        /// <param name="parent">Parent WorldGrid</param>
        public WorldCell(WorldGrid parent)
        {
            Parent = parent;
            ObjectsInCell = new List<IGameObject>();
        }

        /// <summary>
        /// Returns a list of all objects in the cell.
        /// </summary>
        /// <returns>List of objects.</returns>
        public List<IGameObject> GetObjects()
        {
            return new List<IGameObject>(ObjectsInCell);
        }

        /// <summary>
        /// Adds an object to this cell.
        /// </summary>
        /// <param name="obj">Object to add.</param>
        public void Add(IGameObject obj)
        {
            ObjectsInCell.Add(obj);
        }

        /// <summary>
        /// Trys to remove an object from this cell.
        /// </summary>
        /// <param name="obj">Object to remove.</param>
        public void Remove(IGameObject obj)
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
