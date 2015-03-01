using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Robot_Rescue.Level
{
    public enum TileCollision
    {
        Passable,
        Impassable,
        Platform,
    }
    class Tile
    {
        Vector2 position;
        Texture2D sprite;
        TileCollision tileCollision;
        int tileWidth;
        int tileHeight;

        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }
        public Texture2D Sprite
        {
            get { return sprite; }
        }
        public TileCollision TileCollision
        {
            get { return tileCollision; }
            set { tileCollision = value; }
        }

        public Rectangle BoundingRectangle
        {
            get { return new Rectangle((int)position.X, (int)position.Y, tileWidth, tileHeight); }
        }
        public Vector2 BlockCenter
        {
            get { return position - new Vector2(tileWidth, tileHeight); }
        }

        public Tile()
        {
            position = new Vector2();
            sprite = null;
            tileCollision = TileCollision.Passable;
            tileWidth = 0;
            tileHeight = 0;
        }
        public Tile(Vector2 position, int tileWidth, int tileHeight)
        {
            this.position = position;
            sprite = null;
            tileCollision = new TileCollision();
            tileWidth = 0;
            tileHeight = 0;
        }
        public Tile(Vector2 position, Texture2D sprite)
        {
            this.position = position;
            this.sprite = sprite;
            tileCollision = new TileCollision();
            tileWidth = sprite.Width;
            tileHeight = sprite.Height;
        }
        public Tile(Vector2 position, Texture2D sprite, TileCollision tileCollision)
        {
            this.position = position;
            this.sprite = sprite;
            this.tileCollision = tileCollision;
            tileWidth = sprite.Width;
            tileHeight = sprite.Height;
        }

        public void Collision(bool isGround, Rectangle bounds, GameObject gameObject)
        {
            // If this tile is collidable,
            TileCollision collision = tileCollision;
            if (collision != TileCollision.Passable)
            {
                // Determine collision depth (with direction) and magnitude.
                Rectangle tileBounds = BoundingRectangle;
                Vector2 depth = RectangleExtensions.GetIntersectionDepth(bounds, tileBounds);
                if (depth != Vector2.Zero)
                {
                    float absDepthX = Math.Abs(depth.X);
                    float absDepthY = Math.Abs(depth.Y);

                    // Resolve the collision along the shallow axis.
                    if (absDepthY < absDepthX || collision == TileCollision.Platform)
                    {
                        //// If we crossed the top of a tile, we are on the ground.
                        //if (previousBottom <= tileBounds.Top)
                        //    isOnGround = true;

                        //// Ignore platforms, unless we are on the ground.
                        //if (collision == TileCollision.Impassable || isOnGround)
                        //{
                        //    // Resolve the collision along the Y axis.
                        //    Position = new Vector2(Position.X, Position.Y + depth.Y);

                        //    // Perform further collisions with the new bounds.
                        //    bounds = BoundingRectangle;
                        //}
                    }
                    else if (collision == TileCollision.Impassable) // Ignore platforms.
                    {
                        // Resolve the collision along the X axis.
                        gameObject.Position = new Vector2(gameObject.Position.X + depth.X, gameObject.Position.Y);

                        // Perform further collisions with the new bounds.
                        bounds = BoundingRectangle;
                    }
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (sprite != null)
                spriteBatch.Draw(sprite, BoundingRectangle, Color.White);
        }
    }
}
