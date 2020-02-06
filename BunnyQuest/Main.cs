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
        /// <summary>
        /// A reference to the player entity inside the ECS engine.
        /// </summary>
        AlphaBunny player;

        Entity splasher;

        Engine engine;
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
            camera.Zoom = 1f;

            map = new Map2D(Content, 16);
            engine = new ECS.Engine(map);

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


            engine.AddEntity(new AlphaBunny(0, this.Content)
            {
                size = new Vector2(32, 32),
                pos = new Vector2(240, 240)
            });

            this.player = (AlphaBunny)engine.GetEntityFromIndex(0);


            this.splasher = new Entity(1);
            splasher.AddComponent(new CmpSplashes(splasher, Content.Load<Texture2D>("etc/flag"), -99));
            engine.AddEntity(splasher);



            for (int i = 2; i < 10; ++i)
            {
                var bb = new BetaBunny(i, this.Content)
                {
                    size = new Vector2(32, 32),
                    pos = new Vector2(101 + (32 * i), 450)
                };
                engine.AddEntity(bb);
            }


            //var enemy = new Entities.EvilBunny(11, this.Content)
            //{
            //    pos = new Vector2(96, 32),
            //    size = new Vector2(32, 32)
            //};
            //system.AddEntity(enemy);


            //for (int i = 0; i < 2; ++i)
            //    engine.AddEntity(new Entities.Tree(99 + i, Content) { pos = new Vector2(224 + (32 * i), 100) });

            //for (int i = 0; i < 2; ++i)
            //    engine.AddEntity(new Entities.Tree(99 + i, Content) { pos = new Vector2(384 + (32 * i), 100) });
        }


        protected override void UnloadContent() { }


        // Not used
        private void ChangeBunny()
        {
            if (player.UUID == 1)
                this.player = (AlphaBunny)engine.GetEntity(0);
            else if (player.UUID == 0)
                this.player = (AlphaBunny)engine.GetEntity(1);


            //t_changedBunny_marker = 2;
            //flag_changedBunny_marker = true;

            for (int i = 0; i < engine.GetEntityCount(); i++)
            {
                if (i == player.UUID)
                    continue;

                var ent = engine.GetEntity(i);

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
            return engine.GetEntityFromIndex(camera_entity);
        }

        private void SetCameraTarget()
        {
            camera_entity += 1;
            if (camera_entity == engine.GetEntityCount())
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
                for (int i = 0; i < engine.GetEntityCount(); ++i)
                {
                    if (engine.GetEntityFromIndex(i) is AlphaBunny p)
                    {
                        if (p.expired == false)
                        {
                            player = p;
                            break;
                        }
                    }
                }
            }


            #region Rotation to player direction stuff

            // Rotate player
            Utils.SetAnimBasedOfDirection(player.GetComponent<CmpAnim>(), player.direction);

            // Rotate follower bunnies
            for (int i = 2; i < 100; ++i)
            {
                if (engine.HasEntityOfIndex(i) == false || engine.HasEntityOfIndex(i + 1) == false)
                    break;

                BetaBunny bb = (BetaBunny)engine.GetEntityFromIndex(i);

                Vector2 dir = Vector2.Zero; // We set this in the if statements below. its the direction that sprite will be flipped to.
                if (bb.ai.destination_set)
                {
                    dir = bb.ai.destination;
                    //bb.anim.renderColor = Color.LightGray;
                }
                else if (bb.ai.State == CmpAI_Follower.STATE_CmpAI_Follower.Following)
                {
                    dir = bb.ai.entity_toFollow.GetCenterPosition();
                    //bb.anim.renderColor = Color.LightGray;
                }
                else
                {
                    //bb.anim.renderColor = Color.Gray;
                    continue;
                }

                Utils.SetAnimBasedOfDirection(bb.anim, Utils.GetDirection(bb.GetCenterPosition(), dir));
            }
            #endregion


            player.pos += (player.speed * player.direction);

            Vector2 pos = new Vector2(
                GetCameraTarget().pos.X + (GraphicsDevice.Viewport.Width) * 0.5f, 
                GetCameraTarget().pos.Y + (GraphicsDevice.Viewport.Height) * 0.5f);
            camera.Position = pos; //GetCameraTarget().pos + graphics.GraphicsDevice.; //+ GetCameraTarget().size / 2;

            // Update camera before updating the ECS
            camera.UpdateControls((float)gameTime.ElapsedGameTime.TotalSeconds);

            // Update the ECS
            engine.Update(gameTime);

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
                    var cursor_worldPos = camera.GetWorldPosition(cursor_pos);

                    splasher.GetComponent<CmpSplashes>().ClearSplashes();
                    splasher.GetComponent<CmpSplashes>().CreateSplash(cursor_worldPos - Vector2.UnitY * 32);

                    void RemoveSplash(object o, EntityArgs args)
                    {
                        if (o is CmpAI_Follower ai)
                        {
                            //foreach(var bb in player.GetFollowersOfFollower( (BetaBunny)args.Entity))
                            //{
                            //    bb.GetComponent<CmpAnim>().currentSpriteCollection = 2;
                            //}

                            player.RemoveFollower(ai.entity, args);

                            splasher.GetComponent<CmpSplashes>().ClearSplashes();
                            ai.DestinationReached -= RemoveSplash;
                        }
                    };

                    // Send all bunnies to clicked position
                    if (player.followers.Count != 0)
                    {
                        var list = player.GetFollowersOfFollower(player.followers[0]);

                        player.followers[0].ai.SetDestination(cursor_worldPos);
                        player.followers[0].ai.DestinationReached += RemoveSplash;
                        player.RemoveFollower(null, new EntityArgs(player.followers[0], null));
                        
                        for(int i = 0; i < list.Count; ++i)
                        {
                            player.AddFollower(list[i]);
                        }
                    }
                }
            }



            #region Keyboard movement stuff
            if (keyboardState.IsKeyDown(Keys.W)) // Up
            {
                player.direction.X = 0;
                player.direction.Y = -1;
            }
            else if (keyboardState.IsKeyDown(Keys.S)) // Down
            {
                player.direction.X = 0;
                player.direction.Y = 1;
            }
            else if (keyboardState.IsKeyDown(Keys.A)) // Left
            {
                player.direction.X = -1;
                player.direction.Y = 0;
            }
            else if (keyboardState.IsKeyDown(Keys.D)) // Right
            {
                player.direction.X = 1;
                player.direction.Y = 0;
            }
            else
            {
                player.direction.X = 0;
                player.direction.Y = 0;
            }


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
                foreach (var e in Utils.EnumerateCursorCollisioners(engine, cursor_rect, camera, GraphicsDevice))
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
                    foreach (var e in Utils.EnumerateCursorCollisioners(engine, cursorSelection.rect, camera, GraphicsDevice))
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
            engine.Render(spriteBatch);
            spriteBatch.End();
            #endregion



            #region User Interface stuff
            spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, null);

            cursorSelection.Render(spriteBatch);

            // Render logic for the player healthbar (the carrots at top left)
            var stats = player.GetComponent<CmpStats>();
            // Render the black carrots that is relative to actual max health but subtraced by current health
            for (int i = stats.health_cap; i > stats.health; --i)
                spriteBatch.Draw(tex_carrot, new Vector2((24 * i), 25), Color.Black);
            // Render the carrots that is relative to current health
            for (int i = 0; i < stats.GetHealth(); ++i)
                spriteBatch.Draw(tex_carrot, new Vector2(25 + (24 * i), 25), Color.White);

            spriteBatch.End();
            #endregion

            base.Draw(gameTime);
        }
    }
}
