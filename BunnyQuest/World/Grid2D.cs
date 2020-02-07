using System;

namespace BunnyQuest.World
{
    /// <summary>
    /// A generic 2D grid class.
    /// </summary>
    /// <typeparam name="T">The type of the 2-dimensional grid. Must be struct.</typeparam>
    class Grid2D<T> where T : struct
    {
        /// <summary>
        /// The 2D array. The Y axis is indexed first.
        /// </summary>
        public T[][] grid;

        /// <summary>
        /// The count of elements in both dimensions.
        /// </summary>
        public readonly int Size;

        public Grid2D(int size)
        {
            Size = size; // Size is readonly.

            // Initialize grid (2D array)
            grid = new T[size][];
            for (int y = 0; y < size; ++y) // Maybe not needed?
                grid[y] = new T[size];
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
                return grid[y][x];
            }
            set
            {
                grid[y][x] = value;
            }
        }

        public T GetElement(int x, int y)
        {
            return grid[y][x];
        }

        /// <summary>
        /// DOES NOT WORK AT THE MOMENT!!!!
        /// </summary>
        public T[][] RotateClockwise()
        {
            T[][] new_grid = new T[Size][];

            for (int i = 0; i < new_grid.Length; i++)
                new_grid[i] = new T[Size];

            for (int x = 0; x < Size; x++)
            {
                for (int y = 0; y < Size; y++)
                {
                    new_grid[Size - 1 - y][x] = grid[y][x];
                }
            }

            return new_grid;
        }

        public static int[][] RotateClockwise(int[][] grid)
        {
            int height = grid.Length;
            int width = grid[0].Length;

            if (height != width)
                throw new System.Exception("Your 2D array is not square! So we crash.");

            int[][] new_matrix = new int[height][];

            for (int w = 0; w < new_matrix.Length; w++)
                new_matrix[w] = new int[height];

            for (int x = 0; x < height; x++)
            {
                for (int y = 0; y < width; y++)
                {
                    new_matrix[height - 1 - y][x] = grid[y][x];
                }
            }

            return new_matrix;
        }

        public void Print()
        {
            for (int x = 0; x < Size; x++)
            {
                for (int y = 0; y < Size; y++)
                {
                    Console.Write(grid[y][x].ToString());
                }
                Console.WriteLine();
            }
        }
    }
}
