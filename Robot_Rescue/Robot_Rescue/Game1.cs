using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;

using Robot_Rescue.Sprites;
using Robot_Rescue.Camera;
using Robot_Rescue.Level;

namespace Robot_Rescue
{

    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        //PlayerRobot player;
        Map map;

        List<Texture2D> Menus;

        // Create an instance of Texture2D that will
        // contain the background texture.
        Texture2D background;
        
        // Create a Rectangle that will define
        // the limits for the main game screen.
        Rectangle fullBackgroundSize;


        Camera2d cam = new Camera2d();
        //GameObject robot;

    
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            map = new Map();
            Menus = new List<Texture2D>();

        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well./
        /// </summary>
        protected override void Initialize()
        {
            Vector2 playerPosition;
            GameState.Initialize( Content);
            

            //Change the resolution to 500
            graphics.PreferredBackBufferWidth = 600;
            graphics.PreferredBackBufferHeight = 400;
            graphics.ApplyChanges();
            
            spriteBatch = new SpriteBatch(GraphicsDevice);
            playerPosition = new Vector2(100, 500);

            cam.Pos = new Vector2(playerPosition.X, playerPosition.Y);

          //  player = new PlayerRobot(this, @"Characters\man", spriteBatch, playerPosition, Vector2.Zero);

        //    player = new PlayerRobot();

            base.Initialize();

           // playerPosition.X = (Window.ClientBounds.Width - playerTexture.Width) / 5;
           // playerPosition.Y = (Window.ClientBounds.Height - playerTexture.Height) / 2;
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

        //    player.LoadContent(this.Content);           
            //robot = new Robot(new Vector2(110), Content.Load<Texture2D>("Textures\\Robot(S)"), map, Content.Load<SoundEffect>(@"Sound/Jump"));

            spriteBatch = new SpriteBatch(graphics.GraphicsDevice);
            background = Content.Load<Texture2D>("Textures\\shiny_blue");
            fullBackgroundSize = new Rectangle(0, 0, background.Width, background.Height);

            Menus.Add(Content.Load<Texture2D>("StateImages\\startscreen"));
            Menus.Add(Content.Load<Texture2D>("StateImages\\startscreen"));

            LoadAndPlayStageMusic();

            map.LoadMap("map", Content);
            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        protected void LoadAndPlayStageMusic()
        {
            MediaPlayer.Play(GameState.SongList[0]);

            /*
            SoundEffectInstance soundEffect = Content.Load<SoundEffect>(@"Sound/Level1").CreateInstance();
            soundEffect.Play();
            */
           /* SoundEffect soundEffect;
            soundEffect = Content.Load<SoundEffect>(@"/Sound/Level1");
            soundEffect.Play();*/
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                this.Exit();

    //        player.Update(gameTime);
            //robot.Update();


            switch (GameState.State)
            {
                case State.Main:
                    if (Keyboard.GetState().IsKeyDown(Keys.Space))
                    {
                        MediaPlayer.IsRepeating = true;
                        MediaPlayer.Play(GameState.SongList[1]);
                        GameState.State = State.SinglePlayer;
                    }
                    break;
                case State.SinglePlayer:
                    map.Update();
                    cam.Pos = map.Robot.Position;
                    break;
                case State.End:
                    //highScores.Update();
                    break;
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Gray);


            switch (GameState.State)
            {
                case State.Main:
                    spriteBatch.Begin();
                    spriteBatch.Draw(Menus[0], new Vector2(0), Color.White);
                    spriteBatch.End();
                    break;
                case State.SinglePlayer:
            spriteBatch.Begin(SpriteSortMode.Deferred,
                        BlendState.AlphaBlend,
                        null,
                        null,
                        null,
                        null,
                        cam.get_transformation(this.GraphicsDevice));
                    map.Draw(spriteBatch);
                    spriteBatch.End();
                    break;
                case State.End:
                    spriteBatch.Begin();
                    spriteBatch.Draw(Menus[1], new Vector2(0), Color.White);
            spriteBatch.End();
                    break;
            }


            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}