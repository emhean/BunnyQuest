using BunnyQuest.ECS.Components;
using BunnyQuest.World;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace BunnyQuest
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Main : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        KeyboardState previous_keyboardState;
        KeyboardState keyboardState;


        Camera.Camera2DControlled camera;
        int camera_entity;
        Entities.Player player;
        ECS.System system;
        Map map;

        Texture2D tex_carrot;

        public Main()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            camera = new Camera.Camera2DControlled();
            camera.Zoom = 1.5f;

            map = new Map(Content, 16);
            system = new ECS.System(map);



            base.Initialize();
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            tex_changedBunny_marker = Content.Load<Texture2D>("etc/hand");
            tex_carrot = Content.Load<Texture2D>("spritesheets/carrot");

            system.AddEntity(new Entities.Player(0, this.Content)
            {
                size = new Vector2(32, 32),
                pos = new Vector2(240, 240)
            });
            system.AddEntity(new Entities.Player(1, this.Content)
            {
                size = new Vector2(32, 32),
                pos = new Vector2(277, 240)
            });

            this.player = (Entities.Player)system.GetEntity(0);

            var enemy = new Entities.EvilBunny(11, this.Content)
            {
                pos = new Vector2(96, 32),
                size = new Vector2(32, 32)
            };
            system.AddEntity(enemy);



            system.AddEntity(new Entities.Tree(99, Content)
            {
                pos = new Vector2(350, 100)
            });
        }


        protected override void UnloadContent() { }

        Texture2D tex_changedBunny_marker;
        bool flag_changedBunny_marker;
        float t_changedBunny_marker = 2;
        float sin_marker;

        private void ChangeBunny()
        {
            if(player.UUID == 1)
                this.player = (Entities.Player)system.GetEntity(0);
            else if(player.UUID == 0)
                this.player = (Entities.Player)system.GetEntity(1);


            t_changedBunny_marker = 2;
            flag_changedBunny_marker = true;

            for (int i = 0; i < system.GetEntityCount(); i++)
            {
                if (i == player.UUID)
                    continue;

                var ent = system.GetEntity(i);

                if (ent is Entities.Player p)
                {
                    player = p;
                    t_changedBunny_marker = 2;
                    flag_changedBunny_marker = true;
                }
            }
        }

        private ECS.Entity GetCameraTarget() => system.GetEntity(camera_entity);
        private void SetCameraTarget()
        {
            camera_entity += 1;
            if (camera_entity == system.GetEntityCount())
                camera_entity = 0;

            t_changedBunny_marker = 2;
            flag_changedBunny_marker = true;
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            keyboardState = Keyboard.GetState(); // Get state of keyboard (pressed button etc)
            if (keyboardState.IsKeyDown(Keys.Escape))
                Exit();

            if (keyboardState.IsKeyUp(Keys.Tab) && previous_keyboardState.IsKeyDown(Keys.Tab))
            {
                SetCameraTarget();
                //ChangeBunny();
            }

            if(t_changedBunny_marker > 0)
            {
                t_changedBunny_marker -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                sin_marker = (float)Math.Sin(t_changedBunny_marker + sin_marker) * 4;
            }
            if(t_changedBunny_marker < 0)
            {
                flag_changedBunny_marker = false;
                t_changedBunny_marker = 0;
            }



            if (keyboardState.IsKeyDown(Keys.W)) // Up
            {
                player.direction.Y = -1;
                player.GetComponent<CmpAnim>().currentSpriteCollection = 0;
            }
            else if (keyboardState.IsKeyDown(Keys.S)) // Down
            {
                player.direction.Y = 1;
                player.GetComponent<CmpAnim>().currentSpriteCollection = 2;
            }
            else player.direction.Y = 0;

            if (keyboardState.IsKeyDown(Keys.A)) // Left
            {
                player.direction.X = -1;
                player.GetComponent<CmpAnim>().currentSpriteCollection = 3;
            }
            else if (keyboardState.IsKeyDown(Keys.D)) // Right
            {
                player.direction.X = 1;
                player.GetComponent<CmpAnim>().currentSpriteCollection = 1;
            }
            else player.direction.X = 0;


            if (player.direction == Vector2.Zero)
            {
                player.GetComponent<CmpAnim>().IsUpdated = false;
            }
            else player.GetComponent<CmpAnim>().IsUpdated = true;



            if (player.direction.X == 1 && player.direction.Y == -1)
            {
                player.GetComponent<CmpAnim>().currentSpriteCollection = 0;
                player.GetComponent<CmpAnim>().rotation = 0.45f;
            }
            else if (player.direction.X == -1 && player.direction.Y == -1)
            {
                player.GetComponent<CmpAnim>().currentSpriteCollection = 0;
                player.GetComponent<CmpAnim>().rotation = -0.45f;
            }
            else if (player.direction.X == 1 && player.direction.Y == 1)
            {
                player.GetComponent<CmpAnim>().currentSpriteCollection = 1;
                player.GetComponent<CmpAnim>().rotation = 0.90f;
            }
            else if (player.direction.X == -1 && player.direction.Y == 1)
            {
                player.GetComponent<CmpAnim>().currentSpriteCollection = 3;
                player.GetComponent<CmpAnim>().rotation = -0.90f;
            }
            else player.GetComponent<CmpAnim>().rotation = 0f;


            player.pos += (player.speed * player.direction);

            camera.Position = GetCameraTarget().pos + GetCameraTarget().size / 2;
            // Update camera before updating the ECS
            camera.UpdateControls((float)gameTime.ElapsedGameTime.TotalSeconds);
            // Update the ECS
            system.Update(gameTime);


            previous_keyboardState = keyboardState;
            base.Update(gameTime);
        }


        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, camera.GetTransformation(this.GraphicsDevice));

            for (int i = 0; i < map.mapWidth; ++i)
            {
                for (int j = 0; j < map.mapHeight; ++j)
                {
                    var t = map.tileGrid[i, j];
                    spriteBatch.Draw(map.tileSheet.tex, t.rect, map.tileSheet.sourceRects[t.tileid], Color.White);
                }
            }

            // Render the ECS world
            system.Render(spriteBatch);

            if(flag_changedBunny_marker)
            {
                Vector2 pos = new Vector2(
                    GetCameraTarget().pos.X + tex_changedBunny_marker.Width / 4,
                    GetCameraTarget().pos.Y - GetCameraTarget().size.Y + sin_marker);
                spriteBatch.Draw(tex_changedBunny_marker, pos, Color.White);
            }
            spriteBatch.End();

            spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, null);
            for (int i = 0; i < player.GetComponent<CmpStats>().GetHealth(); ++i)
            {
                spriteBatch.Draw(tex_carrot, new Vector2((50 * i), 50), Color.White);
            }

            spriteBatch.End();


            base.Draw(gameTime);
        }
    }
}
