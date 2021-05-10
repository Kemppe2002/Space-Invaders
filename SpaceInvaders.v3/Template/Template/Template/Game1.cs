using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace Template
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        Texture2D spaceship;
        Rectangle spaceshippos = new Rectangle(310, 400, 30, 20);
        int shipspeed = 2;

        Texture2D enemy;
        Rectangle enemypos = new Rectangle(310, 100, 20, 15);
        int xspeed = 1;

        Texture2D laser;
        int laserspeed = 1;
        Rectangle laserpos = new Rectangle(50, 300, 5, 20);
        bool drawlaser = false;

        List<Rectangle> laserlist = new List<Rectangle>();
        
        Texture2D gameover;
        Rectangle gameoverpos = new Rectangle(200, 100, 400, 200);

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
            if (kstate.IsKeyDown(Keys.Right)) { spaceshippos.X += shipspeed; } 
            if (kstate.IsKeyDown(Keys.Left)) { spaceshippos.X -= shipspeed; }
            if (kstate.IsKeyDown(Keys.Down)) { spaceshippos.Y += shipspeed; }
            if (kstate.IsKeyDown(Keys.Up)) { spaceshippos.Y -= shipspeed; }

            //Spacship boundaries
            if (spaceshippos.X < 0) { spaceshippos.X = 0; }
            if (spaceshippos.X > Window.ClientBounds.Width - spaceshippos.Width) { spaceshippos.X = Window.ClientBounds.Width - spaceshippos.Width; }
            if (spaceshippos.Y < 0) { spaceshippos.Y = 0; }
            if (spaceshippos.Y > Window.ClientBounds.Height - spaceshippos.Height) { spaceshippos.Y = Window.ClientBounds.Height - spaceshippos.Height; }


            //Enemy moving and boundary logic
            enemypos.X += xspeed;
            if (enemypos.X<0||enemypos.X>Window.ClientBounds.Width-enemypos.Width) { xspeed *= -1; enemypos.Y+=15; }
            if (enemypos.Y >= Window.ClientBounds.Height - enemypos.Height) { enemypos.Y = Window.ClientBounds.Height - enemypos.Height; GameOver(); }

            //Collision logic
            if (enemypos.X < spaceshippos.X + spaceshippos.Width && enemypos.X+enemypos.Width > spaceshippos.X && 
                enemypos.Y < spaceshippos.Y + spaceshippos.Height && enemypos.Y+enemypos.Height > spaceshippos.Y) { GameOver(); }

            
            //Laser logic
            if (kstate.IsKeyDown(Keys.Space)) { laserlist.Add(laserpos); drawlaser = true; }
            laserpos.Y -= laserspeed;
            
            
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
            xspeed = 0; //stops enemy movement
            shipspeed = 0; //stops player movement
            stategameover = true;
        }
    }
}
