namespace BunnyQuest.World
{
    /// <summary>
    /// A grid of tiles. Inherits from the generic Grid2D<T> class.
    /// </summary>
    class TileGrid : Grid2D<Tile>
    {
        public TileGrid(int size) : base(size)
        {
        }
    }
}
