using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BunnyQuest.World
{
    class Map
    {
        /// <summary>
        /// A grid of Tiles. TileGrid inherits from the generic Grid class.
        /// </summary>
        public TileGrid tileGrid;
        public TileSheet tileSheet;

        public int mapWidth;
        public int mapHeight => mapWidth; // cus itsa s'queer
        public SoundEffectInstance bgm;

        public Map(ContentManager content)
        {
            this.mapWidth = 8;
            this.tileGrid = new TileGrid(8);

            this.tileSheet = new TileSheet(content.Load<Texture2D>("tilesets/ts_nature"), 32);

            int[][] tiles = new int[][]
            {
                new int[] { 0, 0, 0, 0, 0, 0, 0, 0 },
                new int[] {0, 0, 0, 0, 0, 0, 0, 0, },
                new int[] {0, 0, 0, 1, 1, 0, 0, 0, },
                new int[] {0, 0, 1, 1, 1, 1, 0, 0, },
                new int[] {0, 0, 1, 1, 1, 0, 0, 0, },
                new int[] {0, 0, 0, 1, 1, 0, 0, 0, },
                new int[] {0, 0, 0, 0, 0, 0, 0, 0, },
                new int[] {0, 0, 0, 0, 0, 0, 0, 0, }
            };

            Random rnd = new Random();

            for(int x = 0; x < mapWidth; x++)
            {
                for(int y = 0; y < mapHeight; y++)
                {
                    Rectangle tileRect = new Rectangle(x * 32, y * 32, 32, 32);

                    //int id = rnd.Next(0, tileSheet.sourceRects.Length);

                    int id = tiles[x][y];

                    tileGrid[y, x] = new Tile(id, tileRect);
                }
            }
        }
    }
}
