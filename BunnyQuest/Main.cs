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
        Vector2 cursor_pos, cursor_worldPos;

        Camera.Camera2DControlled camera;
        int camera_entity;
        /// <summary>
        /// A reference to the player entity inside the ECS engine.
        /// </summary>
        AlphaBunny alphaBunny;

        Car car;

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


            // AlphaBunny
            //engine.AddEntity(new AlphaBunny(0, this.Content)
            //{
            //    size = new Vector2(32, 32),
            //    pos = new Vector2(240, 240)
            //});
            //this.alphaBunny = (AlphaBunny)engine.GetEntityFromIndex(0);



            engine.AddEntity(new Car(0, this.Content)
            {
                size = new Vector2(24, 43),
                pos = new Vector2(240, 240)
            });
            this.car = (Car)engine.GetEntityFromIndex(0);



            this.splasher = new Entity(1);
            splasher.AddComponent(new CmpSplashes(splasher, Content.Load<Texture2D>("etc/flag"), -99));
            engine.AddEntity(splasher);


            // Add some BetaBunnies
            //for (uint i = 2; i < 10; ++i)
            //{
            //    var bb = new BetaBunny(i, this.Content)
            //    {
            //        size = new Vector2(32, 32),
            //        pos = new Vector2(101 + (32 * i), 450)
            //    };
            //    engine.AddEntity(bb);
            //}


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


            // AlphaBunny Update logic
            if(alphaBunny != null)
            {
                if (alphaBunny.Expired)
                {
                    // If player controlled alpha bunny is dead we find the next one.... hopefully
                    for (int i = 0; i < engine.GetEntityCount(); ++i)
                    {
                        if (engine.GetEntityFromIndex(i) is AlphaBunny p)
                        {
                            if (p.Expired == false)
                            {
                                alphaBunny = p;
                                break;
                            }
                        }
                    }
                }


                #region Rotation to player direction stuff

                // Rotate player
                Utils.SetAnimBasedOfDirection(alphaBunny.GetComponent<CmpAnim>(), alphaBunny.direction);

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


                alphaBunny.pos += (alphaBunny.speed * alphaBunny.direction);
            }





            // Camera target
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
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

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
            cursor_worldPos = camera.GetWorldPosition(cursor_pos);

            // AlphaBunny Control Logic
            if(alphaBunny != null)
            {
                // All the beta bunnies unfollow each other
                if (keyboardState.IsKeyDown(Keys.Tab))
                {
                    alphaBunny.RemoveAllFollowers();
                }

                if (keyboardState.IsKeyDown(Keys.LeftControl)) //&& prev_keyboardState.IsKeyUp(Keys.LeftControl))
                {
                    foreach (var bb in alphaBunny.followers)
                    {
                        bb.pos += Utils.GetDirection(bb.pos, cursor_worldPos) * 2;
                    }
                }
                //else if (keyboardState.IsKeyUp(Keys.LeftControl) && prev_keyboardState.IsKeyDown(Keys.LeftControl))
                //{
                //}




                if (keyboardState.IsKeyDown(Keys.LeftShift))
                {
                    if (alphaBunny.followers.Count != 0
                    && mouseState.LeftButton == ButtonState.Released
                    && prev_mouseState.LeftButton == ButtonState.Pressed)
                    {
                        Tile tile = GetCursorTile();

                        // This vector centers the flag to the tiles center position.
                        Vector2 flag_center = new Vector2(tile.rect.X + tile.rect.Width / 2, tile.rect.Y - tile.rect.Height / 2);
                        splasher.GetComponent<CmpSplashes>().ClearSplashes();
                        splasher.GetComponent<CmpSplashes>().CreateSplash(flag_center);

                        void RemoveSplash(object o, EntityArgs args)
                        {
                            if (o is CmpAI_Follower ai)
                            {
                                //foreach(var bb in player.GetFollowersOfFollower( (BetaBunny)args.Entity))
                                //{
                                //    bb.GetComponent<CmpAnim>().currentSpriteCollection = 2;
                                //}

                                alphaBunny.RemoveFollower(ai.parent, args);

                                splasher.GetComponent<CmpSplashes>().ClearSplashes();
                                ai.DestinationReached -= RemoveSplash;
                            }
                        };

                        // Send all bunnies to clicked position
                        if (alphaBunny.followers.Count != 0)
                        {
                            var list = alphaBunny.GetFollowersOfFollower(alphaBunny.followers[0]);

                            // Destination vector that is the center of the tile for followers to move to
                            Vector2 dest = new Vector2(tile.rect.X + tile.rect.Width / 2, tile.rect.Y + tile.rect.Height / 2);

                            if (mouseState.RightButton == ButtonState.Pressed)
                            {
                                alphaBunny.followers[0].ai.SetDestination(dest);

                                var rnd = new Random();
                                foreach (var f in alphaBunny.GetFollowersOfFollower(alphaBunny.followers[0]))
                                {
                                    f.ai.SetDestination(dest + Vector2.One * rnd.Next(-32, 33));
                                }
                                alphaBunny.followers[0].ai.DestinationReached += RemoveSplash;
                            }
                            else
                            {
                                alphaBunny.followers[0].ai.SetDestination(dest);
                                alphaBunny.followers[0].ai.DestinationReached += RemoveSplash;
                                alphaBunny.RemoveFollower(null, new EntityArgs(alphaBunny.followers[0], null));

                                for (int i = 0; i < list.Count; ++i)
                                {
                                    alphaBunny.AddFollower(list[i]);
                                }
                            }

                        }
                    }
                }



                #region Keyboard and movement for AlphaBunny
                    if (keyboardState.IsKeyDown(Keys.W)) // Up
                    {
                        alphaBunny.direction.X = 0;
                        alphaBunny.direction.Y = -1;
                    }
                    else if (keyboardState.IsKeyDown(Keys.S)) // Down
                    {
                        alphaBunny.direction.X = 0;
                        alphaBunny.direction.Y = 1;
                    }
                    else if (keyboardState.IsKeyDown(Keys.A)) // Left
                    {
                        alphaBunny.direction.X = -1;
                        alphaBunny.direction.Y = 0;
                    }
                    else if (keyboardState.IsKeyDown(Keys.D)) // Right
                    {
                        alphaBunny.direction.X = 1;
                        alphaBunny.direction.Y = 0;
                    }
                    else
                    {
                        alphaBunny.direction.X = 0;
                        alphaBunny.direction.Y = 0;
                    }


                    if (alphaBunny.direction == Vector2.Zero)
                    {
                        alphaBunny.GetComponent<CmpAnim>().IsUpdated = false;
                    }
                    else alphaBunny.GetComponent<CmpAnim>().IsUpdated = true;
                }
            #endregion




            // Update logic for Car is here in UpdateInputs because its all we care about for now
            if(car != null)
            {
                var anim = car.GetComponent<CmpAnim>();
                anim.origin = new Vector2(12, 12);

                Console.WriteLine();
                Console.WriteLine("car.direction:" + car.direction);
                Console.WriteLine("anim.rotation: " + anim.rotation);
                Console.WriteLine("car.rotation_angle:" + car.rotation_angle);

                // RotationAngle is set using a value in degrees (the float f).
                //car.rotation_angle = MathHelper.ToRadians(anim.rotation);

                car.rotation_angle = MathHelper.ToRadians( 
                    MathHelper.ToDegrees(anim.rotation) - 90
                    );


                car.direction = new Vector2((float)Math.Cos(car.rotation_angle),
                                (float)Math.Sin(car.rotation_angle));

                car.direction.Normalize();

                // Move forward
                car.pos += car.speed * car.direction;

                // Gas
                if (keyboardState.IsKeyDown(Keys.Space))
                {
                    if(car.speed <= 3)
                    {
                        car.speed += dt;
                    }
                }
                else
                {
                    if(car.speed > 0)
                    {
                        car.speed -= dt * 2;

                        if(car.speed < 0)
                        {
                            car.speed = (int)0;
                        }
                    }
                }

                if (keyboardState.IsKeyDown(Keys.A)) // Left
                {
                    anim.rotation -= dt * car.speed;
                    //car.rotation_angle -= dt * 90;
                }
                else if (keyboardState.IsKeyDown(Keys.D)) // Right
                {
                    anim.rotation += dt * car.speed;
                    //car.rotation_angle += dt * 90;
                }
                else
                {
                }
            }


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
                                alphaBunny.AddFollower(bb); //bb.Follow(player);
                        }
                    }

                    cursorSelection.End();
                }
            }
        }


        /// <summary>
        /// Gets the Tile that the cursor is on.
        /// </summary>
        private Tile GetCursorTile()
        {
            map.tileGrid.TranslateToGridIndex((int)cursor_worldPos.X, (int)cursor_worldPos.Y, out int x, out int y);
            Tile tile = map.tileGrid[y, x];
            return tile;
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

            // Render the tiles.
            for (int i = 0; i < map.mapWidth; ++i)
            {
                for (int j = 0; j < map.mapHeight; ++j)
                    spriteBatch.Draw(map.tileSheet.tex, map.tileGrid[i, j].rect, map.tileSheet.sourceRects[map.tileGrid[i, j].tileid], Color.White);
            }

            // Renders the tile selection area
            spriteBatch.Draw(tex_pixel, GetCursorTile().rect, Color.AliceBlue * 0.37f);
            #endregion



            #region Render the ECS world
            engine.Render(spriteBatch);
            spriteBatch.End();
            #endregion



            #region User Interface stuff
            spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, null);

            cursorSelection.Render(spriteBatch);


            // AlphaBunny Render logic
            if(alphaBunny != null)
            {
                // Render logic for the player healthbar (the carrots at top left)
                var stats = alphaBunny.GetComponent<CmpStats>();
                // Render the black carrots that is relative to actual max health but subtraced by current health
                for (int i = stats.health_cap; i > stats.health; --i)
                    spriteBatch.Draw(tex_carrot, new Vector2((24 * i), 25), Color.Black);
                // Render the carrots that is relative to current health
                for (int i = 0; i < stats.GetHealth(); ++i)
                    spriteBatch.Draw(tex_carrot, new Vector2(25 + (24 * i), 25), Color.White);
            }


            spriteBatch.End();
            #endregion

            base.Draw(gameTime);
        }
    }
}
