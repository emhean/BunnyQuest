﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

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

        public Map(ContentManager content, int size, bool randomize = false)
        {
            this.mapWidth = size; // To keep the map size for logic
            this.tileGrid = new TileGrid(size);
            this.tileSheet = new TileSheet(content.Load<Texture2D>("tilesets/ts_nature"), 32);

            if (randomize)
            {
                Random rnd = new Random(); // Init random
                for (int x = 0; x < mapWidth; x++)
                {
                    for (int y = 0; y < mapHeight; y++)
                    {
                        Rectangle tileRect = new Rectangle(x * 32, y * 32, 32, 32);
                        tileGrid[y, x] = new Tile(rnd.Next(0, tileSheet.sourceRects.Length), tileRect);
                    }
                }
            }
            else // Do not randomize map.
            {
                int[][] tiles = new int[][]
                {
                    new int[] { 1, 1, 1, 1, 0, 0, 0, 0 },
                    new int[] { 1, 0, 0, 0, 0, 0, 0, 0, },
                    new int[] { 1, 0, 0, 0, 0, 0, 0, 0, },
                    new int[] { 1, 0, 0, 0, 0, 0, 0, 0, },
                    new int[] { 0, 0, 0, 0, 0, 0, 0, 0, },
                    new int[] { 0, 0, 0, 0, 0, 0, 0, 0, },
                    new int[] { 0, 0, 0, 0, 0, 0, 0, 0, },
                    new int[] { 0, 0, 0, 0, 0, 0, 0, 0, }
                };


                MatrixFunktioner matrixFunktioner = new MatrixFunktioner();
                tiles = matrixFunktioner.Matrix_transformer(tiles);

                for (int x = 0; x < mapWidth; x++)
                {
                    for (int y = 0; y < mapHeight; y++)
                    {
                        Rectangle tileRect = new Rectangle(x * 32, y * 32, 32, 32);
                        tileGrid[y, x] = new Tile(tiles[x][y], tileRect);
                    }
                }
            }
        }
    }
}
