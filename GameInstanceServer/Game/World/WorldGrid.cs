using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;

using GameInstanceServer.Game.Objects;

namespace GameInstanceServer.Game.World
{
    /// <summary>
    /// A space partioning grid used for the game world. The world is broken
    /// into cells, each cell can contain infinitely many objects. Objects
    /// are sorted into new cells as they move around.
    /// </summary>
    public class WorldGrid
    {
        private readonly int Width, Height, CellWidth, CellHeight;

        private WorldCell[][] Cells;

        private int CellCountX
        {
            get
            {
                return (int)Math.Ceiling((float)Width / CellWidth) + 2;
            }
        }

        private int CellCountY
        {
            get
            {
                return (int)Math.Ceiling((float)Height / CellHeight) + 2;
            }
        }

        /// <summary>
        /// Creates a new instance of a WorldGrid.
        /// </summary>
        /// <param name="width">Width of world.</param>
        /// <param name="height">Height of world.</param>
        /// <param name="cellwidth">Width of a cell.</param>
        /// <param name="cellheight">Height of a cell.</param>
        public WorldGrid(int width, int height, int cellwidth, int cellheight)
        {
            Width = width;
            Height = height;
            CellWidth = cellwidth;
            CellHeight = cellheight;

            // Initialise grid array
            Cells = new WorldCell[CellCountX][];
            for(int i = 0; i < CellCountX; i++)
            {
                Cells[i] = new WorldCell[CellCountY];
                for(int j = 0; j < CellCountY; j++)
                {
                    Cells[i][j] = new WorldCell(this);
                }
            }
        }

        /// <summary>
        /// Returns all the objects in nearby cells to a given point.
        /// </summary>
        /// <param name="position">Reference point.</param>
        /// <returns></returns>
        public List<IGameObject> GetObjectsInCellsAroundPoint(Point position)
        {
            // Find cell this point is in
            Tuple<int, int> cell = GetCellFromPoint(position);

            // Loop through all cells, and combine their outputed lists
            // into a single list to return.
            List<IGameObject> objectsInCells = new List<IGameObject>();
            for(int yy = -1; yy < 1; yy++)
            {
                for(int xx = -1; xx < 2; xx++)
                {
                    objectsInCells.AddRange(
                        Cells[cell.Item1 + xx][cell.Item2 + yy].GetObjects()
                        );
                }
            }

            return objectsInCells;
        }

        /// <summary>
        /// Adds an object to the WorldGrid.
        /// </summary>
        /// <param name="obj">Object to add.</param>
        public void Add(IGameObject obj)
        {
            try
            {
                Tuple<int, int> cellIndex = GetCellFromPoint(obj.GetPosition());
                WorldCell cell = Cells[cellIndex.Item1][cellIndex.Item2];

                cell.Add(obj);
                obj.SetCell(cell);
            }
            catch
            {
            }
        }

        /// <summary>
        /// Removes an object from the WorldGrid.
        /// </summary>
        /// <param name="obj">Object to remove.</param>
        public void Remove(IGameObject obj)
        {
            try
            {
                obj.GetCell().Remove(obj);
            }
            catch
            {
            }
        }

        /// <summary>
        /// Updates a given object within the world grid. Moving it to a new
        /// cell if required.
        /// </summary>
        /// <param name="obj">Object to update.</param>
        public void Update(IGameObject obj)
        {
            try
            {
                Tuple<int, int> cellIndex = GetCellFromPoint(obj.GetPosition());
                WorldCell cell = Cells[cellIndex.Item1][cellIndex.Item2];

                if (obj.GetCell() != cell)
                {
                    obj.GetCell().Remove(obj);
                    cell.Add(obj);
                    obj.SetCell(cell);
                }
            }
            catch
            {
            }
        }

        /// <summary>
        /// Clears all objects from the world grid.
        /// </summary>
        public void Clear()
        {
            for (int i = 0; i < CellCountY; i++)
            {
                for (int j = 0; j < CellCountX; j++)
                {
                    try
                    {
                        Cells[j][i].Clear();
                    }
                    catch
                    {
                    }
                }
            }
        }

        /// <summary>
        /// Returns the cell a given point is in.
        /// </summary>
        /// <param name="position">Reference point.</param>
        /// <returns>Tuple of cell index.</returns>
        private Tuple<int, int> GetCellFromPoint(Point position)
        {
            // Find cell this point is in
            if (position.X < 0 || position.Y < 0 ||
                position.X > Width || position.Y > Height)
            {
                throw (new ArgumentOutOfRangeException());
            }
            int cellX = position.X / CellWidth + 1;
            int cellY = position.Y / CellHeight + 1;

            return new Tuple<int, int>(cellX, cellY);
        }
    }
}
