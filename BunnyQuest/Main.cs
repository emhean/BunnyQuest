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
        ECS.Entity player;
        ECS.System system;


        public Main()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            camera = new Camera.Camera2DControlled();
            camera.Zoom = 2;

            system = new ECS.System();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);


            player = new Entities.Bunny(0, this.Content);
            player.size = new Vector2(32, 32);
            system.AddEntity(player);


            var enemy = new Entities.Bunny(11, this.Content);
            enemy.pos = new Vector2(100, 100);
            enemy.size = new Vector2(32, 32);

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
            }
            else if (keyboardState.IsKeyDown(Keys.S)) // Down
            {
                player.pos.Y += 1;
            }

            if (keyboardState.IsKeyDown(Keys.A)) // Left
            {
                player.pos.X -= 1;
            }
            else if (keyboardState.IsKeyDown(Keys.D)) // Right
            {
                player.pos.X += 1;
            }


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
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, camera.GetTransformation(this.GraphicsDevice));

            // Render the ECS world
            system.Render(spriteBatch);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
