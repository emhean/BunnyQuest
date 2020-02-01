using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace BunnyQuest.World
{
    /// <summary>
    /// A two-dimensional map of tiles.
    /// </summary>
    class Map2D
    {
        /// <summary>
        /// A grid of Tiles. TileGrid inherits from the generic Grid class.
        /// </summary>
        public TileGrid tileGrid;
        public TileSheet tileSheet;

        /// <summary>
        /// Map width in tiles.
        /// </summary>
        public int mapWidth;

        /// <summary>
        /// Map height in tiles.
        /// </summary>
        public int mapHeight => mapWidth; // cus itsa s'queer

        /// <summary>
        /// The background music.
        /// </summary>
        public SoundEffectInstance bgm;

        public Map2D(ContentManager content, int size, bool randomize = false)
        {
            this.mapWidth = size; // To keep the map size for logic
            this.tileGrid = new TileGrid(size);
            this.tileSheet = new TileSheet(content.Load<Texture2D>("tilesets/ts_nature"), 32);

            this.bgm = content.Load<SoundEffect>("bgm/bunnies").CreateInstance();
            this.bgm.Play();

            if (randomize)
            {
                Random rnd = new Random(); // Init random
                for (int x = 0; x < mapWidth; x++)
                {
                    for (int y = 0; y < mapHeight; y++)
                    {
                        tileGrid[y, x] = Tile.Create(rnd.Next(0, tileSheet.sourceRects.Length), x, y);
                    }
                }
            }
            else // Do not randomize map.
            {
                int[][] tiles = new int[][]
                {
                    new int[] { 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                    new int[] { 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                    new int[] { 1, 1, 0, 0, 0, 0, 0, 3, 3, 3, 0, 0, 0, 0, 0, 0 },
                    new int[] { 1, 1, 0, 0, 3, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                    new int[] { 1, 1, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                    new int[] { 1, 1, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                    new int[] { 1, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                    new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                    new int[] { 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                    new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                    new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                    new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                    new int[] { 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                    new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                    new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                    new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }
                };

                for (int x = 0; x < mapWidth; x++)
                {
                    for (int y = 0; y < mapHeight; y++)
                    {
                        tileGrid[y, x] = Tile.Create(tiles[x][y], x, y);
                    }
                }
            }
        }
    }
}
