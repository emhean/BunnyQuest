namespace BunnyQuest.World
{
    /// <summary>
    /// A generic 2D grid class.
    /// </summary>
    /// <typeparam name="T">The type of the 2-dimensional grid. Must be struct.</typeparam>
    class Grid2D<T> where T : struct
    {
        protected T[,] grid;

        public Grid2D(int size)
        {
            grid = new T[size, size];
        }
    }
}
