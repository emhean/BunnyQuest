using BunnyQuest.ECS.Components;
using BunnyQuest.World;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace BunnyQuest
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Main : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        KeyboardState keyboardState;


        Camera.Camera2DControlled camera;
        Entities.Player player;
        ECS.System system;

        Map map;

        public Main()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            camera = new Camera.Camera2DControlled();
            camera.Zoom = 2;

            map = new Map(Content, 8);
            system = new ECS.System(map);



            base.Initialize();
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);


            player = new Entities.Player(0, this.Content)
            {
                size = new Vector2(32, 32)
            };
            system.AddEntity(player);

            var enemy = new Entities.EvilBunny(11, this.Content)
            {
                pos = new Vector2(96, 32),
                size = new Vector2(32, 32)
            };
            system.AddEntity(enemy);
        }


        protected override void UnloadContent() { }

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


            if(keyboardState.IsKeyDown(Keys.W)) // Up
            {
                player.pos.Y -= 1;
                player.GetComponent<CmpAnim>().currentSpriteCollection = 0;
            }
            else if (keyboardState.IsKeyDown(Keys.S)) // Down
            {
                player.pos.Y += 1;
                player.GetComponent<CmpAnim>().currentSpriteCollection = 2;
            }

            if (keyboardState.IsKeyDown(Keys.A)) // Left
            {
                player.pos.X -= 1;
                player.GetComponent<CmpAnim>().currentSpriteCollection = 3;
            }
            else if (keyboardState.IsKeyDown(Keys.D)) // Right
            {
                player.pos.X += 1;
                player.GetComponent<CmpAnim>().currentSpriteCollection = 1;
            }

            camera.Position = player.pos + player.size / 2;
            // Update camera before updating the ECS
            camera.UpdateControls((float)gameTime.ElapsedGameTime.TotalSeconds);
            // Update the ECS
            system.Update(gameTime);

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

            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
