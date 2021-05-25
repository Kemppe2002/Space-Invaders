using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System;
using System.Diagnostics;

namespace Template
{
    public class Game1 : Game
    {
        Texture2D spaceship;
        Rectangle spaceshippos;
        int shipspeed = 2;

        Texture2D enemy;
        List<Rectangle> enemylist = new List<Rectangle>();
        int enemyspeed = 2;
        int enemywidth = 20;
        int enemyheight = 15;

        Texture2D laser;
        Rectangle laserpos; 
        int laserspeed = 5;
        bool drawlaser = false;

        Texture2D gameover;
        Rectangle gameoverpos;
        bool stategameover = false;

        Texture2D speedbuff; //buffs
        Rectangle speedbuffpos;
        Texture2D laserbuff;
        Rectangle laserbuffpos;
        Texture2D enemynerf;
        Rectangle enemynerfpos;

        double points;
        Stopwatch timer = new Stopwatch();

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            graphics.ToggleFullScreen();

            int gap = 0;
            enemylist.Add(new Rectangle(0, 0, enemywidth, enemyheight));
            for(int i = 0; i<16; i++)
            {
                enemylist.Add(new Rectangle(gap, 0, enemywidth, enemyheight));
                enemylist.Add(new Rectangle(gap, enemyheight + 5, enemywidth, enemyheight));
                enemylist.Add(new Rectangle(gap, 2 * enemyheight + 10, enemywidth, enemyheight));
                enemylist.Add(new Rectangle(gap, 3 * enemyheight + 15, enemywidth, enemyheight));
                enemylist.Add(new Rectangle(gap, 4 * enemyheight + 20, enemywidth, enemyheight));
                enemylist.Add(new Rectangle(gap, 5 * enemyheight + 25, enemywidth, enemyheight));
                enemylist.Add(new Rectangle(gap, 6 * enemyheight + 30, enemywidth, enemyheight));
                enemylist.Add(new Rectangle(gap, 7 * enemyheight + 35, enemywidth, enemyheight));
                gap += 25; //creates gap between enemies
            }

            spaceshippos = new Rectangle(Window.ClientBounds.Width/2-15, Window.ClientBounds.Height-100, 30, 20); //initial positions
            laserpos = new Rectangle(0, -100, 3, 20);
            gameoverpos = new Rectangle(Window.ClientBounds.Width/2-200, Window.ClientBounds.Height/2-150, 400, 300);

            Random r = new Random();                                    //randomizer
            int randomx1 = r.Next(Window.ClientBounds.Width - 30); 
            int randomy1 = r.Next(Window.ClientBounds.Height - 20);
            int randomx2 = r.Next(Window.ClientBounds.Width - 30);
            int randomy2 = r.Next(Window.ClientBounds.Height - 20);
            int randomx3 = r.Next(Window.ClientBounds.Width - 30);
            int randomy3 = r.Next(Window.ClientBounds.Height - 20);

            speedbuffpos = new Rectangle(randomx1, randomy1, 20, 20); //buff positions
            laserbuffpos = new Rectangle(randomx2, randomy2, 20, 20);
            enemynerfpos = new Rectangle(randomx3, randomy3, 20, 20);

            timer.Start();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            spaceship = Content.Load<Texture2D>("spaceship");
            enemy = Content.Load<Texture2D>("enemy");
            gameover = Content.Load<Texture2D>("gameover");
            laser = Content.Load<Texture2D>("laser");
            speedbuff = Content.Load<Texture2D>("speedbuff");
            laserbuff = Content.Load<Texture2D>("laserbuff");
            enemynerf = Content.Load<Texture2D>("enemynerf");
        }

       protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            KeyboardState kstate = Keyboard.GetState();

            if (kstate.IsKeyDown(Keys.Right) || kstate.IsKeyDown(Keys.D)) { spaceshippos.X += shipspeed; } //spaceship moving logic
            if (kstate.IsKeyDown(Keys.Left) || kstate.IsKeyDown(Keys.A)) { spaceshippos.X -= shipspeed; }
            if (kstate.IsKeyDown(Keys.Down) || kstate.IsKeyDown(Keys.S)) { spaceshippos.Y += shipspeed; }
            if (kstate.IsKeyDown(Keys.Up) || kstate.IsKeyDown(Keys.W)) { spaceshippos.Y -= shipspeed; }

            if (spaceshippos.X < 0) { spaceshippos.X = 0; } //spacship boundaries
            if (spaceshippos.X > Window.ClientBounds.Width - spaceshippos.Width) 
            { spaceshippos.X = Window.ClientBounds.Width - spaceshippos.Width; } 
            if (spaceshippos.Y < 0) { spaceshippos.Y = 0; }
            if (spaceshippos.Y > Window.ClientBounds.Height - spaceshippos.Height) { spaceshippos.Y = Window.ClientBounds.Height - spaceshippos.Height; }

            for (int i = 0; i < enemylist.Count; i++) //enemy logic
            {
                enemylist[i] = new Rectangle(enemylist[i].X + enemyspeed, enemylist[i].Y, enemywidth, enemyheight); //moving logic
                if (enemylist[i].X <= 0 || enemylist[i].X >= Window.ClientBounds.Width - enemywidth) //boundary logic
                {
                    for (int j = 0; j < enemylist.Count; j++) 
                    {
                        enemylist[j] = new Rectangle(enemylist[j].X, enemylist[j].Y+enemyheight, enemywidth, enemyheight);
                    }
                    enemyspeed *= -1;
                }
                if (enemylist[i].Y > Window.ClientBounds.Height - enemyheight) { GameOver(); } //boundary logic
                
                if (enemylist[i].X < spaceshippos.X + spaceshippos.Width && enemylist[i].X + enemywidth > spaceshippos.X && //player collision
                enemylist[i].Y < spaceshippos.Y + spaceshippos.Height && enemylist[i].Y + enemyheight > spaceshippos.Y)
                {
                    GameOver();
                }
                
                if (enemylist[i].X < laserpos.X + laserpos.Width && enemylist[i].X + enemywidth > laserpos.X && //laser hit
                enemylist[i].Y < laserpos.Y + laserpos.Height && enemylist[i].Y + enemyheight > laserpos.Y)
                {
                    enemylist.RemoveAt(i);
                    laserpos.Y = -100;
                    points = points + 100;
                }
            }
            if (enemylist.Count == 0) //game completed
            {
                YouWin();
            }

            if (kstate.IsKeyDown(Keys.Space) && laserpos.Y <= -laserpos.Height && stategameover == false) //shooting logic
            {
                laserpos = new Rectangle(spaceshippos.X+spaceshippos.Width/2-laser.Width/2,
                spaceshippos.Y-20, 3, 20); drawlaser = true; 
            }
            laserpos.Y -= laserspeed;

            if (speedbuffpos.X < spaceshippos.X + spaceshippos.Width && speedbuffpos.X + enemywidth > spaceshippos.X && //buff logic
                speedbuffpos.Y < spaceshippos.Y + spaceshippos.Height && speedbuffpos.Y + enemyheight > spaceshippos.Y && enemylist.Count<101)
            { shipspeed = 4; speedbuff.Dispose(); }
            if (laserbuffpos.X < spaceshippos.X + spaceshippos.Width && laserbuffpos.X + enemywidth > spaceshippos.X &&
                laserbuffpos.Y < spaceshippos.Y + spaceshippos.Height && laserbuffpos.Y + enemyheight > spaceshippos.Y && enemylist.Count<61)
            { laserspeed = 10; laserbuff.Dispose(); }
            if (enemynerfpos.X < spaceshippos.X + spaceshippos.Width && enemynerfpos.X + enemywidth > spaceshippos.X &&
                enemynerfpos.Y < spaceshippos.Y + spaceshippos.Height && enemynerfpos.Y + enemyheight > spaceshippos.Y && enemylist.Count<21)
            {
                if (enemyspeed > 0) { enemyspeed = 1; }
                else if (enemyspeed <0) { enemyspeed = -1; }
                enemynerf.Dispose();
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin();

            spriteBatch.Draw(spaceship, spaceshippos, Color.White);

            foreach (Rectangle j in enemylist) { spriteBatch.Draw(enemy, j, Color.White); }

            if (drawlaser == true) { spriteBatch.Draw(laser, laserpos, Color.White); }

            if (enemylist.Count < 101) { spriteBatch.Draw(speedbuff, speedbuffpos, Color.White); } //draw buffs
            if (enemylist.Count < 61) { spriteBatch.Draw(laserbuff, laserbuffpos, Color.White); }
            if (enemylist.Count < 21) { spriteBatch.Draw(enemynerf, enemynerfpos, Color.White); }

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
            timer.Stop();
            double timebonus = 12000-100*timer.Elapsed.TotalSeconds;
            double score = points + timebonus; //point system
            enemyspeed = 0;
            shipspeed = 15;
            laserspeed =50;
        }
    }
}
