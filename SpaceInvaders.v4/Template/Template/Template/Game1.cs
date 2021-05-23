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
        Rectangle spaceshippos;
        int shipspeed = 2;

        Texture2D enemy;
        Rectangle enemypos;
        int enemyspeed = 5;

        Texture2D laser;
        Rectangle laserpos; 
        int laserspeed;
        bool drawlaser = false;

        Texture2D gameover;
        Rectangle gameoverpos;

        bool stategameover = false;
        bool statewinner = false;

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

            spaceshippos = new Rectangle(310, 400, 30, 20);
            enemypos = new Rectangle(310, 100, 20, 15);
            laserpos = new Rectangle(0, -100, 5, 20);
            gameoverpos = new Rectangle(Window.ClientBounds.Width/2-200, Window.ClientBounds.Height/2-100, 400, 200);

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
            laser = Content.Load<Texture2D>("laser");
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
            if (kstate.IsKeyDown(Keys.Right) || kstate.IsKeyDown(Keys.D)) { spaceshippos.X += shipspeed; }
            if (kstate.IsKeyDown(Keys.Left) || kstate.IsKeyDown(Keys.A)) { spaceshippos.X -= shipspeed; }
            if (kstate.IsKeyDown(Keys.Down) || kstate.IsKeyDown(Keys.S)) { spaceshippos.Y += shipspeed; }
            if (kstate.IsKeyDown(Keys.Up) || kstate.IsKeyDown(Keys.W)) { spaceshippos.Y -= shipspeed; }

            //Spacship boundaries
            if (spaceshippos.X < 0) { spaceshippos.X = 0; }
            if (spaceshippos.X > Window.ClientBounds.Width - spaceshippos.Width) { spaceshippos.X = Window.ClientBounds.Width - spaceshippos.Width; }
            if (spaceshippos.Y < 0) { spaceshippos.Y = 0; }
            if (spaceshippos.Y > Window.ClientBounds.Height - spaceshippos.Height) { spaceshippos.Y = Window.ClientBounds.Height - spaceshippos.Height; }


            //Enemy moving and boundary logic
            enemypos.X += enemyspeed;
            if (enemypos.X < 0 || enemypos.X > Window.ClientBounds.Width - enemypos.Width) { enemyspeed *= -1; enemypos.Y += 2*enemypos.Height; }
            if (enemypos.Y >= Window.ClientBounds.Height - enemypos.Height) { enemypos.Y = Window.ClientBounds.Height - enemypos.Height; GameOver(); }

            //Collision logic
            if (enemypos.X < spaceshippos.X + spaceshippos.Width && enemypos.X + enemypos.Width > spaceshippos.X &&
                enemypos.Y < spaceshippos.Y + spaceshippos.Height && enemypos.Y + enemypos.Height > spaceshippos.Y) { GameOver(); }

            //Laser logic
            if (kstate.IsKeyDown(Keys.Space) && laserpos.Y <= -laserpos.Height && stategameover == false && statewinner == false) { laserpos = new Rectangle(spaceshippos.X+spaceshippos.Width/2-laser.Width/2,
                spaceshippos.Y-20, 5, 20); drawlaser = true; laserspeed = 10; }
            laserpos.Y -= laserspeed;

            //Laser hit logic
            if (enemypos.X < laserpos.X + laserpos.Width && enemypos.X + enemypos.Width > laserpos.X &&
                enemypos.Y < laserpos.Y + laserpos.Height && enemypos.Y + enemypos.Height > laserpos.Y) { YouWin(); }

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

            if (drawlaser == true) { spriteBatch.Draw(laser, laserpos, Color.White); }
            if (stategameover == true) { spriteBatch.Draw(gameover, gameoverpos, Color.White); }
            spriteBatch.End();

            base.Draw(gameTime);
        }

        void GameOver()
        {
            enemyspeed = 0; //stops enemy movement
            shipspeed = 0; //stops player movement
            laserspeed = 0; //stops laser movement
            stategameover = true;
        }

        void YouWin()
        {
            enemyspeed = 0;
            laserspeed = 0;
            statewinner = true;
        }
    }
}
