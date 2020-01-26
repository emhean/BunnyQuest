using Microsoft.Xna.Framework;

namespace BunnyQuest.World
{
    struct Tile
    {
        public int tileid;
        public Rectangle rect;

        public Tile(int tileid, Rectangle rect)
        {
            this.tileid = tileid;
            this.rect = rect;
        }

        public override string ToString()
        {
            return tileid.ToString();
        }
    }
}
