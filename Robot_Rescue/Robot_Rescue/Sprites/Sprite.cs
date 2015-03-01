using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Robot_Rescue.Sprites
{
    public class Sprite 
    {
        #region fields
        protected SpriteBatch spriteBatch;
        protected string textureName;
        protected Vector2 position;
        protected Vector2 velocity;
        protected Texture2D texture;
        
        //The texture object used when drawing the sprite
        private Texture2D mSpriteTexture;

        //The asset name for the Sprite's Texture
        public string AssetName;

        //The amount to increase/decrease the size of the original sprite. 
        private float mScale = 1.0f;

        //The Rectangular area from the original image that 
        //defines the Sprite on a sprite sheet.
        Rectangle mSource;
        public Rectangle Source
        {
            get { return mSource; }
            set
            {
                mSource = value;
                Size = new Rectangle(0, 0, (int)(mSource.Width * Scale), (int)(mSource.Height * Scale));
            }
        }
        #endregion 

        #region Properties
        //The Size of the Sprite (with scale applied)
        public Rectangle Size;

        public Vector2 Position
        {
            get { return this.position; }
            set { this.position = value; }
        }
        public int TextureHeight
        {
            get { return this.texture.Height; }
        }

        public int TextureWidth
        {
            get { return this.texture.Width; }
        }

        public string TextureName
        {
            get { return this.textureName; }
            set { this.textureName = value; }
        }

        public Texture2D Texture
        {
            get { return this.texture; }
            set { this.texture = value; }
        }

     
        //The amount to increase/decrease the size of the original sprite. When
        //modified throught he property, the Size of the sprite is recalculated
        //with the new scale applied.
        public float Scale
        {
            get { return mScale; }
            set
            {
                mScale = value;
                //Recalculate the Size of the Sprite with the new scale
                Size = new Rectangle(0, 0, (int)(texture.Width * Scale), (int)(texture.Height * Scale));
            }
        }
        #endregion 

        #region Constructors
 /*       /// <summary>
        /// Construct a new sprite with the given texture, position, and velocity.
        /// The sprite will be drawn using the supplied SpriteBatch; this class assumes that
        /// batch.Begin(...) and batch.End() are called elsewhere./
        /// </summary>
        /// <param name="game">the game context for this sprite</param>
        /// <param name="textureName">this sprite's texture</param>
        /// <param name="batch">the SpriteBatch used to draw this sprite</param>
        /// <param name="initialPosition">the sprite's initial position</param>
        /// <param name="initialVelocity">the sprite'S initial velocity</param>
        public Sprite(Game game, String textureName, SpriteBatch batch, Vector2 initialPosition, Vector2 initialVelocity)
            : base(game)
        {
            this.textureName = textureName;
            this.spriteBatch = batch;
            this.position = initialPosition;
            this.velocity = initialVelocity;
           //this.position = new Vector2(0, 0);
        }*/

        #endregion 
        //Load the texture for the sprite using the Content Pipeline
        public void LoadContent(ContentManager theContentManager, string theAssetName)
        {
            mSpriteTexture = theContentManager.Load<Texture2D>(theAssetName);
            AssetName = theAssetName;
            Source = new Rectangle(0, 0, mSpriteTexture.Width, mSpriteTexture.Height);
            Size = new Rectangle(0, 0, (int)(mSpriteTexture.Width * Scale), (int)(mSpriteTexture.Height * Scale));
        }

        //Update the Sprite and change it's position based on the passed in speed, direction and elapsed time.
        public void Update(GameTime theGameTime, Vector2 theSpeed, Vector2 theDirection)
        {
            Position += theDirection * theSpeed * (float)theGameTime.ElapsedGameTime.TotalSeconds;
        }

        //Draw the sprite to the screen
        public void Draw(SpriteBatch theSpriteBatch)
        {
            theSpriteBatch.Draw(mSpriteTexture, Position, Source,
              Color.White, 0.0f, Vector2.Zero, Scale, SpriteEffects.None, 0);
        }
    }
}
