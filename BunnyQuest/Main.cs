using BunnyQuest.ECS;
using BunnyQuest.ECS.Components;
using BunnyQuest.Entities;
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

        KeyboardState keyboardState, prev_keyboardState;
        MouseState mouseState, prev_mouseState;
        Rectangle cursor_rect;
        Vector2 cursor_pos;

        Camera.Camera2DControlled camera;
        int camera_entity;
        AlphaBunny player;
        ECS.System system;
        Map2D map;

        Texture2D tex_pixel;
        Texture2D tex_background;
        Texture2D tex_carrot;

        Texture2D tex_changedBunny_marker;
        bool flag_changedBunny_marker;
        float t_changedBunny_marker = 2;
        float sin_marker;

        public Main()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            Window.AllowUserResizing = true;
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            camera = new Camera.Camera2DControlled();
            camera.Zoom = 1.5f;

            map = new Map2D(Content, 16);
            system = new ECS.System(map);

            mouseState = Mouse.GetState();
            cursor_rect = new Rectangle(mouseState.X, mouseState.Y, 1, 1);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            tex_pixel = Content.Load<Texture2D>("etc/pixel");
            tex_changedBunny_marker = Content.Load<Texture2D>("etc/hand");
            tex_carrot = Content.Load<Texture2D>("spritesheets/carrot");
            tex_background = Content.Load<Texture2D>("etc/fk");


            system.AddEntity(new AlphaBunny(0, this.Content)
            {
                size = new Vector2(32, 32),
                pos = new Vector2(240, 240)
            });

            this.player = (AlphaBunny)system.GetEntityFromIndex(0);


            for(int i = 2; i < 10; ++i)
            {
                var bb = new BetaBunny(i, this.Content)
                {
                    size = new Vector2(32, 32),
                    pos = new Vector2(101 + (32 * i), 450)
                };
                system.AddEntity(bb);
            }


            //var enemy = new Entities.EvilBunny(11, this.Content)
            //{
            //    pos = new Vector2(96, 32),
            //    size = new Vector2(32, 32)
            //};
            //system.AddEntity(enemy);


            for (int i = 0; i < 4; ++i)
                system.AddEntity(new Entities.Tree(99 + i, Content) { pos = new Vector2(224 + (32 * i), 100) });

            for (int i = 0; i < 4; ++i)
                system.AddEntity(new Entities.Tree(99 + i, Content) { pos = new Vector2(384 + (32 * i), 100) });
        }


        protected override void UnloadContent() { }

        private void ChangeBunny()
        {
            if(player.UUID == 1)
                this.player = (AlphaBunny)system.GetEntity(0);
            else if(player.UUID == 0)
                this.player = (AlphaBunny)system.GetEntity(1);


            t_changedBunny_marker = 2;
            flag_changedBunny_marker = true;

            for (int i = 0; i < system.GetEntityCount(); i++)
            {
                if (i == player.UUID)
                    continue;

                var ent = system.GetEntity(i);

                if (ent is AlphaBunny p)
                {
                    player = p;
                    t_changedBunny_marker = 2;
                    flag_changedBunny_marker = true;
                }
            }
        }

        private void FollowPlayer(BetaBunny bb)
        {
            bb.Follow(player);
        }

        private Entity GetCameraTarget()
        {
            return system.GetEntityFromIndex(camera_entity);
        }

        private void SetCameraTarget()
        {
            camera_entity += 1;
            if (camera_entity == system.GetEntityCount())
                camera_entity = 0;

            t_changedBunny_marker = 0;
            flag_changedBunny_marker = true;
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Get state of keyboard (pressed buttons etc)
            keyboardState = Keyboard.GetState(); 

            // Get state of mouse and set to position and rectangle (used for screen clicking)
            mouseState = Mouse.GetState();
            cursor_pos.X = mouseState.X;
            cursor_pos.Y = mouseState.Y;
            cursor_rect.X = mouseState.X;
            cursor_rect.Y = mouseState.Y;

            if (keyboardState.IsKeyDown(Keys.Escape))
                Exit(); // Close the game

            if (player.expired)
            {
                // If player controlled alpha bunny is dead we find the next one.... hopefully
                for (int i = 0; i < system.GetEntityCount(); ++i)
                {
                    if (system.GetEntityFromIndex(i) is AlphaBunny p)
                    {
                        if (p.expired == false)
                        {
                            player = p;
                            break;
                        }
                    }
                }
            }

            UpdateCursorSelection();

            // All the beta bunnies unfollow each other
            if (keyboardState.IsKeyDown(Keys.Tab))
            {
                for(int i = 0; i < BetaBunny.followers.Count; ++i)
                {
                    BetaBunny.followers[i].Unfollow();
                }
            }

            // Left click with mouse
            if (mouseState.LeftButton == ButtonState.Released
                && prev_mouseState.LeftButton == ButtonState.Pressed)
            {
                CursorSelectionReleased();
            }


            if(t_changedBunny_marker < 2)
            {
                t_changedBunny_marker += (float)gameTime.ElapsedGameTime.TotalSeconds;
                sin_marker += (float)Math.Cos(t_changedBunny_marker * 8);
            }
            if(t_changedBunny_marker > 2)
            {
                flag_changedBunny_marker = false;
                t_changedBunny_marker = 0;
            }


            #region Keyboard movement stuff
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
            #endregion

            #region Rotation to player direction stuff

            // Rotate player
            RotateAnimToDirection(player.GetComponent<CmpAnim>(), player.direction);

            // TODO: Move this into CmpAI_Follower?
            // Rotate follower bunnies
            foreach (BetaBunny bb in system.EnumerateEntities<BetaBunny>(1, 100))
            {
                if (bb.ai.State == CmpAI_Follower.STATE_CmpAI_Follower.Following)
                {
                    bb.anim.renderColor = Color.LightGray;
                    RotateAnimToDirection(bb.anim, player.direction);
                }
                else
                    bb.anim.renderColor = Color.Gray;
            }
            #endregion

            player.pos += (player.speed * player.direction);

            camera.Position = GetCameraTarget().pos + GetCameraTarget().size / 2;
            // Update camera before updating the ECS
            camera.UpdateControls((float)gameTime.ElapsedGameTime.TotalSeconds);
            // Update the ECS
            system.Update(gameTime);


            // Set previous values to values of this frame
            prev_keyboardState = keyboardState;
            prev_mouseState = mouseState;

            base.Update(gameTime);
        }


        Rectangle cursorSelection_rect;
        bool cursorSelection_started;

        private void CursorSelectionReleased()
        {
            foreach (BetaBunny bb in system.EnumerateEntities<BetaBunny>(1, 100))
            {
                var bb_collider = bb.GetComponent<CmpCollider>();
                var bb_screenPos = GetEntityScreenPosition(bb);
                var rect = new Rectangle((int)bb_screenPos.X, (int)bb_screenPos.Y, bb_collider.rect.Width, bb_collider.rect.Height);

                //Rectangle c_rect = new Rectangle(
                //    cursorSelection_rect.X, 
                //    cursorSelection_rect.Y,
                //    cursor_rect.Width + cursorSelection_rect.Width,
                //    cursor_rect.Height + cursorSelection_rect.Height);
                //Rectangle c_rect = Rectangle.Union(cursor_rect, cursorSelection_rect);

                if (Rectangle.Intersect(cursorSelection_rect, rect) != Rectangle.Empty)
                {
                    if (bb.ai.State != CmpAI_Follower.STATE_CmpAI_Follower.Following)
                    {
                        bb.Follow(player);
                    }
                }
            }

            cursorSelection_rect = Rectangle.Empty;
        }

        private void UpdateCursorSelection()
        {
            if (prev_mouseState.LeftButton == ButtonState.Pressed && mouseState.LeftButton == ButtonState.Pressed)
            {
                if(!cursorSelection_started)
                {
                    cursorSelection_rect.X = cursor_rect.X;
                    cursorSelection_rect.Y = cursor_rect.Y;
                    cursorSelection_started = true;
                    return;
                }

                // TODO: Fix screen selection so that it works in all four directions
                //if (cursor_rect.X < cursorSelection_rect.X)
                //    cursorSelection_rect.Width = cursor_rect.X - cursorSelection_rect.X;
                //else
                //{
                //    //cursorSelection_rect.X = cursor_rect.X;
                //    cursorSelection_rect.Width = 64; //cursorSelection_rect.X - cursor_rect.X;
                //}
                //if (cursor_rect.Y < cursorSelection_rect.Y)
                //    cursorSelection_rect.Height = cursor_rect.Y - cursorSelection_rect.Y;
                //else
                //    cursorSelection_rect.Height = cursor_rect.Y;

                cursorSelection_rect.Width = cursor_rect.X - cursorSelection_rect.X;
                cursorSelection_rect.Height = cursor_rect.Y - cursorSelection_rect.Y;
            }
            else
            {
                if(cursorSelection_started)
                {
                    CursorSelectionReleased();
                    cursorSelection_started = false;
                }
            }
        }

        private Vector2 GetEntityScreenPosition(Entity entity)
        {
            return Vector2.Transform(entity.pos, camera.GetTransformation(GraphicsDevice));
        }


        private void RotateAnimToDirection(CmpAnim anim, Vector2 direction)
        {
            // There is probably a smarter way to do this...

            if (direction.X == 1 && direction.Y == -1)
            {
                anim.currentSpriteCollection = 0;
                anim.rotation = 0.45f;
            }
            else if (direction.X == -1 && direction.Y == -1)
            {
                anim.currentSpriteCollection = 0;
                anim.rotation = -0.45f;
            }
            else if (direction.X == 1 && direction.Y == 1)
            {
                anim.currentSpriteCollection = 1;
                anim.rotation = 0.90f;
            }
            else if (direction.X == -1 && direction.Y == 1)
            {
                anim.currentSpriteCollection = 3;
                anim.rotation = -0.90f;
            }
            else anim.rotation = 0f;
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.DarkGreen);

            spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, null);
            spriteBatch.Draw(tex_background, new Rectangle(0, 0, Window.ClientBounds.Width, Window.ClientBounds.Height), Color.White);
            spriteBatch.End();

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


            #region User Interface stuff
            spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, null);

            if(cursorSelection_rect != Rectangle.Empty)
            {
                spriteBatch.Draw(tex_pixel, cursorSelection_rect, Color.Blue * 0.2f);
            }

            for(int i = 0; i < player.GetComponent<CmpStats>().health_cap; ++i)
                spriteBatch.Draw(tex_carrot, new Vector2(25 + (24 * i), 25), Color.Black);

            for (int i = 0; i < player.GetComponent<CmpStats>().GetHealth(); ++i)
                spriteBatch.Draw(tex_carrot, new Vector2(25 + (24 * i), 25), Color.White);            
            spriteBatch.End();
            #endregion

            base.Draw(gameTime);
        }
    }
}
