using BunnyQuest.ECS;
using BunnyQuest.ECS.Components;
using BunnyQuest.Entities;
using BunnyQuest.World;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

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

        CursorSelection cursorSelection;

        Texture2D tex_pixel;
        Texture2D tex_background;
        Texture2D tex_carrot;

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

            cursorSelection = new CursorSelection(Content);

            tex_pixel = Content.Load<Texture2D>("etc/pixel");
            tex_carrot = Content.Load<Texture2D>("spritesheets/carrot");
            tex_background = Content.Load<Texture2D>("etc/jungle");


            system.AddEntity(new AlphaBunny(0, this.Content)
            {
                size = new Vector2(32, 32),
                pos = new Vector2(240, 240)
            });

            this.player = (AlphaBunny)system.GetEntityFromIndex(0);


            for (int i = 2; i < 10; ++i)
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


            for (int i = 0; i < 2; ++i)
                system.AddEntity(new Entities.Tree(99 + i, Content) { pos = new Vector2(224 + (32 * i), 100) });

            for (int i = 0; i < 2; ++i)
                system.AddEntity(new Entities.Tree(99 + i, Content) { pos = new Vector2(384 + (32 * i), 100) });
        }


        protected override void UnloadContent() { }


        // Not used
        private void ChangeBunny()
        {
            if (player.UUID == 1)
                this.player = (AlphaBunny)system.GetEntity(0);
            else if (player.UUID == 0)
                this.player = (AlphaBunny)system.GetEntity(1);


            //t_changedBunny_marker = 2;
            //flag_changedBunny_marker = true;

            for (int i = 0; i < system.GetEntityCount(); i++)
            {
                if (i == player.UUID)
                    continue;

                var ent = system.GetEntity(i);

                if (ent is AlphaBunny p)
                {
                    player = p;
                    //t_changedBunny_marker = 2;
                    //flag_changedBunny_marker = true;
                }
            }
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

            //t_changedBunny_marker = 0;
            //flag_changedBunny_marker = true;
        }


        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            UpdateInputs(gameTime);
            UpdateCursorSelection(gameTime);

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


            //if (t_changedBunny_marker < 2)
            //{
            //    t_changedBunny_marker += (float)gameTime.ElapsedGameTime.TotalSeconds;
            //    sin_marker += (float)Math.Cos(t_changedBunny_marker * 8);
            //}
            //if (t_changedBunny_marker > 2)
            //{
            //    flag_changedBunny_marker = false;
            //    t_changedBunny_marker = 0;
            //}


            #region Rotation to player direction stuff
            // Rotate player
            Utilities.RotateAnimToDirection(player.GetComponent<CmpAnim>(), player.direction);

            // TODO: Move this into CmpAI_Follower?
            // Rotate follower bunnies
            foreach (BetaBunny bb in system.EnumerateEntities<BetaBunny>(1, 100))
            {
                if (bb.ai.State == CmpAI_Follower.STATE_CmpAI_Follower.Following)
                {
                    bb.anim.renderColor = Color.LightGray;
                    Utilities.RotateAnimToDirection(bb.anim, player.direction);
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

            base.Update(gameTime);
        }

        /// <summary>
        /// Update logic that contains cursor and keyboard stuff, including player movement and actions.
        /// </summary>
        private void UpdateInputs(GameTime gameTime)
        {
            // Get state of keyboard (pressed buttons etc)
            keyboardState = Keyboard.GetState();

            if (keyboardState.IsKeyDown(Keys.Escape))
                Exit(); // Close the game

            // Get state of mouse and set to position and rectangle (used for screen clicking)
            mouseState = Mouse.GetState();
            cursor_pos.X = mouseState.X;
            cursor_pos.Y = mouseState.Y;
            cursor_rect.X = mouseState.X;
            cursor_rect.Y = mouseState.Y;

            // All the beta bunnies unfollow each other
            if (keyboardState.IsKeyDown(Keys.Tab))
            {
                player.RemoveAllFollowers();
            }

            if (keyboardState.IsKeyDown(Keys.LeftShift))
            {
                if(mouseState.LeftButton == ButtonState.Released
                    && prev_mouseState.LeftButton == ButtonState.Pressed)
                {
                    foreach(var bb in player.followers)
                    {
                        bb.ai.SetDestination(cursor_pos);
                    }
                }
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

            // Set previous values to values of this frame
            prev_keyboardState = keyboardState;
            prev_mouseState = mouseState;
        }

        private void UpdateCursorSelection(GameTime gameTime)
        {
            cursorSelection.Update((float)gameTime.ElapsedGameTime.TotalSeconds);

            if (prev_mouseState.RightButton == ButtonState.Pressed
                && mouseState.RightButton == ButtonState.Released)
            {
                // Enumerate and get those who collide with cursor
                foreach (var e in Utilities.EnumerateCursorCollisioners(system, cursor_rect, camera, GraphicsDevice))
                {
                    if (e is BetaBunny bb) // Cast those of type BetaBunny
                    {
                        // Those who got right clicked will unfollow.
                        if (bb.ai.State == CmpAI_Follower.STATE_CmpAI_Follower.Following)
                        {
                            // Used to be: player.RemoveFollower(bb);
                            bb.ai.OnStoppedFollowing();
                        }
                    }
                }
            }

            if (prev_mouseState.LeftButton == ButtonState.Pressed
                && mouseState.LeftButton == ButtonState.Pressed)
            {
                if (!cursorSelection.selection_started)
                {
                    cursorSelection.Start(cursor_pos);
                    return;
                }

                cursorSelection.UpdateCursorSelection(cursor_pos);
            }
            else
            {
                if (cursorSelection.selection_started)
                {
                    // Same as when enumerating the cursor_rect except this time we enumerate the selection rectangle
                    foreach (var e in Utilities.EnumerateCursorCollisioners(system, cursorSelection.rect, camera, GraphicsDevice))
                    {
                        if (e is BetaBunny bb) // Cast entity if it's a BetaBunny. Otherwise tree's and shit will follow us.
                        {
                            if (bb.ai.State != CmpAI_Follower.STATE_CmpAI_Follower.Following)
                                player.AddFollower(bb); //bb.Follow(player);
                        }
                    }

                    cursorSelection.End();
                }
            }
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

            #region Render Tiles
            spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, camera.GetTransformation(this.GraphicsDevice));
            for (int i = 0; i < map.mapWidth; ++i)
            {
                for (int j = 0; j < map.mapHeight; ++j)
                {
                    var t = map.tileGrid[i, j];
                    spriteBatch.Draw(map.tileSheet.tex, t.rect, map.tileSheet.sourceRects[t.tileid], Color.White);
                }
            }
            #endregion



            #region Render the ECS world
            system.Render(spriteBatch);
            spriteBatch.End();
            #endregion



            #region User Interface stuff
            spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, null);

            cursorSelection.Render(spriteBatch);

            for (int i = 0; i < player.GetComponent<CmpStats>().health_cap; ++i)
                spriteBatch.Draw(tex_carrot, new Vector2(25 + (24 * i), 25), Color.Black);

            for (int i = 0; i < player.GetComponent<CmpStats>().GetHealth(); ++i)
                spriteBatch.Draw(tex_carrot, new Vector2(25 + (24 * i), 25), Color.White);

            spriteBatch.End();
            #endregion

            base.Draw(gameTime);
        }
    }
}
