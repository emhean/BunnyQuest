using Microsoft.Xna.Framework;

namespace BunnyQuest.World
{
    struct Tile
    {
        public int tileid;
        public Rectangle rect;
        public bool collidable;

        public static Tile Create(int tileid, int x, int y)
        {
            if (tileid == 3)
                return new Tile(tileid, new Rectangle(x * 32, y * 32, 32, 32), true);
            else if (tileid == 4)
                return new Tile(tileid, new Rectangle(x * 32, y * 32, 32, 64), true);


            return new Tile(tileid, new Rectangle(x * 32, y * 32, 32, 32), false);
        }

        // Private constructor
        Tile(int tileid, Rectangle rect, bool collidable) 
        {
            this.tileid = tileid;
            this.rect = rect;
            this.collidable = collidable;
        }

        public override string ToString()
        {
            return tileid.ToString();
        }
    }
}
