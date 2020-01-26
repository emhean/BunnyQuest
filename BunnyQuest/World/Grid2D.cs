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

        public int[][] Rotate(int[][] grid)
        {
            int height = grid.Length;
            int width = grid[0].Length;
            int[][] new_matrix = new int[height][];

            for (int w = 0; w < new_matrix.Length; w++)
                //  for (int h = 0; h < height + 1; h++)
                new_matrix[w] = new int[height];


            if (height == width)
            {
                for (int x = 0; x < height; x++)
                {
                    for (int y = 0; y < width; y++)
                    {

                        new_matrix[height - 1 - y][x] = grid[y][x];
                    }
                }
                return new_matrix;
            }
            else throw new System.Exception("Your 2D array is not square! So we crash.");

        }
    }
}
