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
        Rectangle spaceshippos = new Rectangle(310, 400, 30, 20);

        Texture2D enemy;
        Rectangle enemypos = new Rectangle(310, 450, 20, 15);
        int xspeed = 2;

        Texture2D gameover;
        Rectangle gameoverpos = new Rectangle(250, 100, 400, 200);

        bool stategameover = false;

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
            //graphics.ToggleFullScreen();
            
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
            enemy = Content.Load<Texture2D>("enemy");
            gameover = Content.Load<Texture2D>("gameover");
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
            // Spaceship moving logic
            if (kstate.IsKeyDown(Keys.Right)) { spaceshippos.X+=5; } 
            if (kstate.IsKeyDown(Keys.Left)) { spaceshippos.X-=5; }
            if (kstate.IsKeyDown(Keys.Down)) { spaceshippos.Y += 5; }
            if (kstate.IsKeyDown(Keys.Up)) { spaceshippos.Y -= 5; }

            //Spacship boundaries
            if (spaceshippos.X < 0) { spaceshippos.X = 0; }
            if (spaceshippos.X > Window.ClientBounds.Width - spaceshippos.Width) { spaceshippos.X = Window.ClientBounds.Width - spaceshippos.Width; }
            if (spaceshippos.Y < 0) { spaceshippos.Y = 0; }
            if (spaceshippos.Y > Window.ClientBounds.Height - spaceshippos.Height) { spaceshippos.Y = Window.ClientBounds.Height - spaceshippos.Height; }
                        //Enemy moving and boundary logic
            enemypos.X += xspeed;
            if (enemypos.X<0||enemypos.X>Window.ClientBounds.Width-enemypos.Width) { xspeed *= -1; enemypos.Y+=15; }
            if (enemypos.Y >= Window.ClientBounds.Height - enemypos.Height) { enemypos.Y = Window.ClientBounds.Height - enemypos.Height; GameOver(); }

            
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <pawa dsram name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
               GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here.
            spriteBatch.Begin();
            spriteBatch.Draw(spaceship, spaceshippos, Color.White);
            spriteBatch.Draw(enemy, enemypos, Color.White);
            if (stategameover == true) { spriteBatch.Draw(gameover, gameoverpos, Color.White); }
            spriteBatch.End();

            base.Draw(gameTime);
        }

        void GameOver()
        {
            xspeed = 0;
            stategameover = true;
        }
    }
}
