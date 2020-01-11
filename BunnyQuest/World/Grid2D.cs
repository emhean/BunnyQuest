namespace BunnyQuest.World
{
    /// <summary>
    /// A generic 2D grid class.
    /// </summary>
    /// <typeparam name="T">The type of the 2-dimensional grid. Must be struct.</typeparam>
    class Grid2D<T> where T : struct
    {
        protected T[,] grid;

        public readonly int Size;

        public Grid2D(int size)
        {
            Size = size; // Size is readonly.

            // Initialize grid (2D array)
            grid = new T[size, size];
        }

        /// <summary>
        /// Grid indexer of type T.
        /// </summary>
        /// <param name="y">The row of the grid, the first element.</param>
        /// <param name="x">The column of the grid, the second element.</param>
        public T this[int y, int x]
        {
            get
            {
                return grid[y, x];
            }
            set
            {
                grid[y, x] = value;
            }
        }

        public T GetElement(int x, int y)
        {
            return grid[y, x];
        }
    }
}
