using BunnyQuest.ECS;
using BunnyQuest.ECS.Components;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System;

namespace BunnyQuest.Entities
{
    class Car : Entity
    {
        //public Vector2 speed = new Vector2(2f, 2f);

        public float speed_max = 3;
        public float speed;

        public Vector2 direction = Vector2.Zero;

        public float rotation_angle;

        public Car(uint UUID, ContentManager content) : base(UUID)
        {
            var collider = new CmpCollider(this, content.Load<Texture2D>("etc/pixel"))
            {
                GetsPushed = true,
                GetsPushedBySameType = true,
                Type = "car",
                TypeFilter = new string[] { "betabunny" }
            };

            // This so that the collider rotates according to front wheels of the car
            collider.offset.Y = 8;
            collider.offset.Height = -16;

            this.AddComponent(collider);

            var sprites = new Rectangle[][]
            {
                new Rectangle[] { new Rectangle(0, 0, 24, 43) },
            };

            var anim = new CmpAnim(this, content.Load<Texture2D>("spritesheets/car"),
                sprites // The sprite(s) we created above
                )
            {
                renderColor = Color.White
            };
            AddComponent(anim);
        }
    }
}
