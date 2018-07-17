using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using MySql.Data;
using MySql.Data.MySqlClient;
using System.Threading;


namespace CollisionGame
{
    // This is a sample edit
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        long DataBaseId;
        void DoSlowWebRequest()
        {
            Thread.Sleep(10000);
        }
        MySqlConnection mySqlConnection;
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Texture2D m_crapFace;
        Texture2D player_face;
        Texture2D fire_ball;
        GameLogic logic;

        // task2
        Texture2D playerUp;
        Texture2D playerDown;
        Texture2D playerLeft;
        Texture2D playerRight;

        public Game1()
        {
            logic = new GameLogic();
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            string connstring = string.Format("Server=104.237.149.53; database=testdb; UID=sam; password=sam; SslMode=none");
            mySqlConnection = new MySqlConnection(connstring);
            mySqlConnection.Open();

            string input = Microsoft.VisualBasic.Interaction.InputBox("Enter your name: ", "Name", "Default", -1, -1);
            MySqlCommand cmd = new MySqlCommand("insert into User (Name) values (?name)", mySqlConnection);
            cmd.Parameters.Add("?name", MySqlDbType.VarChar).Value = input;
            cmd.ExecuteNonQuery();

            DataBaseId = cmd.LastInsertedId; 
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

            base.Initialize();
        }

        private void DoSomethingWithSpriteBatch(SpriteBatch sb)
        {

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
            m_crapFace = this.Content.Load<Texture2D>("Crapface");
            player_face = this.Content.Load<Texture2D>("PlayerFace");
            fire_ball = this.Content.Load<Texture2D>("FireBall");

            // task2

            playerUp = this.Content.Load<Texture2D>("up");
            playerDown = this.Content.Load<Texture2D>("down");
            playerLeft = this.Content.Load<Texture2D>("left");
            playerRight = this.Content.Load<Texture2D>("right");
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

            logic.player.left = Keyboard.GetState().IsKeyDown(Keys.Left);
            logic.player.up = Keyboard.GetState().IsKeyDown(Keys.Up);
            logic.player.down = Keyboard.GetState().IsKeyDown(Keys.Down);
            logic.player.right = Keyboard.GetState().IsKeyDown(Keys.Right);


            // task 1

            logic.player.space = Keyboard.GetState().IsKeyDown(Keys.Space);



            // TODO: Add your update logic here

            base.Update(gameTime);
            float dt = (float)gameTime.ElapsedGameTime.TotalMilliseconds / 1000;
            float t = (float)gameTime.TotalGameTime.TotalMilliseconds / 1000;
            logic.tick(t, dt);

           if(logic.isDead)
            {
                MySqlCommand cmd = new MySqlCommand("insert Into Results (Score, UserID) values (?score, ?userid)", mySqlConnection);
                int score = 1234;
                cmd.Parameters.Add("?score", MySqlDbType.Int32).Value = score;
                cmd.Parameters.Add("?userid", MySqlDbType.Int32).Value = DataBaseId;
                cmd.ExecuteNonQuery();

                System.Windows.Forms.MessageBox.Show("You died");
                System.Diagnostics.Process.GetCurrentProcess().Kill();
            }
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            if(logic.isDead)
            {
                GraphicsDevice.Clear(Color.Red);
            }
            // TODO: Add your drawing code here

            base.Draw(gameTime);
           
            spriteBatch.Begin();          
            for(int x = 0; x<logic.all.Count;x++)
            {
                GameObject go = logic.all[x];
                Rectangle rc = new Rectangle((int)go.x - 10, (int)go.y - 10, 20, 20);

                SpriteToDraw whichSprite = go.GetSprite();
                switch(whichSprite)
                {
                    case SpriteToDraw.CrapFace:
                        spriteBatch.Draw(m_crapFace, rc, Color.White);
                        break;
                    case SpriteToDraw.Player:
                        spriteBatch.Draw(player_face, rc, Color.White);
                        //task2
                        spriteBatch.Draw(playerUp, rc, Color.White);
                        spriteBatch.Draw(playerDown, rc, Color.White);
                        spriteBatch.Draw(playerRight, rc, Color.White);
                        spriteBatch.Draw(playerLeft, rc, Color.White);
                        break;
                    case SpriteToDraw.Fireball:
                        spriteBatch.Draw(fire_ball, rc, Color.White);
                        break;
                }
            }
            spriteBatch.End();
        }
    }
}
