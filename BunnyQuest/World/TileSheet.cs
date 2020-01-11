using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BunnyQuest.World
{
    class TileSheet
    {
        /// <summary>
        /// The tilesheet atlas, a.k.a spritesheet.
        /// </summary>
        public Texture2D tex;

        /// <summary>
        /// The size of a tile.
        /// </summary>
        int tileSize;

        /// <summary>
        /// The rectangles that are the bounds of the atlas to be rendered;
        /// </summary>
        public Rectangle[] sourceRects;

        public TileSheet(Texture2D tex, int tileSize)
        {
            this.tex = tex;
            this.tileSize = tileSize;

            this.sourceRects = new Rectangle[]
            {
                new Rectangle(0, 64, 32, 32), // Grass
                new Rectangle(32, 64, 32, 32), // Water
                new Rectangle(64, 64, 32, 32), // Brown Rocks
                new Rectangle(96, 64, 32, 32) // Bricks
            };
        }
    }
}
