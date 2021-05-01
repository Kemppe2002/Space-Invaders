using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Template
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        Texture2D spaceship;
        Rectangle rect1 = new Rectangle(500, 400, 20, 20);

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        //KOmentar
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            graphics.ToggleFullScreen();

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here 
            spaceship = Content.Load<Texture2D>("spaceship");
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            KeyboardState kstate = Keyboard.GetState();
            if (kstate.IsKeyDown(Keys.Right)) { rect1.X+=5; } //Adds movement to the ship when pressing buttons.
            if (kstate.IsKeyDown(Keys.Left)) { rect1.X-=5; }
            if (kstate.IsKeyDown(Keys.Down)) { rect1.Y += 5; }
            if (kstate.IsKeyDown(Keys.Up)) { rect1.Y -= 5; }

            if (rect1.X < 0) { rect1.X = 0; }
            if (rect1.X > Window.ClientBounds.Width - rect1.Width) { rect1.X = Window.ClientBounds.Width - rect1.Width; }
            if (rect1.Y < 0) { rect1.Y = 0; }
            if (rect1.Y > Window.ClientBounds.Height - rect1.Height) { rect1.Y = Window.ClientBounds.Height - rect1.Height; }
            

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here.
            spriteBatch.Begin();
            spriteBatch.Draw(spaceship, rect1, Color.White);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
