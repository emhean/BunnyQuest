namespace BunnyQuest.World
{
    /// <summary>
    /// A grid of tiles. Inherits from the generic Grid2D<T> class.
    /// </summary>
    class TileGrid : Grid2D<Tile>
    {
        public int tileWidth, tileHeight;

        public TileGrid(int tileWidth, int tileHeight, int size) : base(size)
        {
            this.tileWidth = tileWidth;
            this.tileHeight = tileHeight;
        }

        public void TranslateToGridIndex(int x, int y, out int grid_x, out int grid_y)
        {
            grid_x = x / tileWidth;
            grid_y = y / tileHeight;

            if (grid_x < 0)
                grid_x = 0;
            else if (grid_x >= Size)
                grid_x = Size - 1;

            if (grid_y < 0)
                grid_y = 0;
            else if (grid_y >= Size)
                grid_y = Size - 1;
        }

        public void TranslateToGridPosition(int x, int y, out int grid_x, out int grid_y)
        {
            grid_x = x / tileWidth;
            grid_y = y / tileHeight;

            if (grid_x < 0)
                grid_x = 0;
            else if (grid_x >= Size)
                grid_x = Size * tileWidth;
            else
                grid_x *= tileWidth;

            if (grid_y < 0)
                grid_y = 0;
            else if (grid_y >= Size)
                grid_y = Size * tileHeight;
            else
                grid_y *= tileHeight;
        }
    }
}
