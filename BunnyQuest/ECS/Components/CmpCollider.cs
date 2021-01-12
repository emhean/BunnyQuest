using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BunnyQuest.ECS.Components
{
    class CmpCollider : Component
    {
        public Rectangle rect;
        public Rectangle offset;

        //public bool size_relativeToParent = true;

        /// <summary>
        /// A filter of types that is irrelevant to this collider.
        /// </summary>
        public string[] TypeFilter;
        /// <summary>
        /// Returns true if type exists in this colliders type filter.
        /// </summary>
        public bool HasTypeInFilter(string type)
        {
            for (int i = 0; i < TypeFilter.Length; ++i)
                if (TypeFilter[i] == type)
                    return true;
            return false;
        }

        public string Type = "null";
        public bool GetsPushedBySameType = false;
        public bool GetsPushed = false;

        Texture2D tex_px; // Debug texture
        public bool Debug { get; set; } = true;

        public CmpCollider(Entity owner) : base(owner)
        {
        }

        public CmpCollider(Entity owner, Texture2D debug_texture) : base(owner)
        {
            this.tex_px = debug_texture;
        }

        public void UpdateColliderPosition()
        {
            rect.X = (int)this.parent.pos.X;
            rect.Y = (int)this.parent.pos.Y;

            rect.Width = (int)this.parent.size.X;
            rect.Height = (int)this.parent.size.Y;


            rect.X += offset.X;
            rect.Y += offset.Y;
            rect.Width += offset.Width;
            rect.Height += offset.Height;
        }

        public override void Update(float delta)
        {
            UpdateColliderPosition();

            base.Update(delta);
        }

        public override void Render(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(tex_px, rect, Color.Red * 0.33f);
        }

        /// <summary>
        /// Prevents the entity to move outside the boundaries of the map.
        /// </summary>
        public void PlaceWithinBounds(int xStart, int xEnd, int yStart, int yEnd)
        {
            if (rect.X < xStart)
                SetX(0);
            else if (rect.X + rect.Width > xEnd)
                SetX(xEnd - rect.Width);

            if (rect.Y < yStart)
                SetY(0);
            else if (rect.Y + rect.Height > yEnd)
                SetY(yEnd - rect.Height);
        }

        /// <summary>
        /// Sets the position of the entity and the collider.
        /// </summary>
        public void SetPosition(int x, int y)
        {
            parent.pos.X = x;
            parent.pos.Y = y;

            UpdateColliderPosition();
        }

        /// <summary>
        /// Sets the position of the entity and the collider.
        /// </summary>
        public void SetX(int x)
        {
            parent.pos.X = x;
            UpdateColliderPosition();
        }

        /// <summary>
        /// Sets the position of the entity and the collider.
        /// </summary>
        public void SetY(int y)
        {
            parent.pos.Y = y;
            UpdateColliderPosition();
        }

        /// <summary>
        /// Sets the position of the entity and the collider.
        /// </summary>
        public void SetPosition(float x, float y)
        {
            parent.pos.X = x;
            parent.pos.Y = y;

            UpdateColliderPosition();
        }
    }
}
