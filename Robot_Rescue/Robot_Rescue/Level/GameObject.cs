using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Robot_Rescue.Level
{
    abstract class GameObject
    {
        Vector2 position;
        Vector2 spriteCenter;
        Texture2D sprite;
        Map map;
        bool isOnGround;
        float previousBottom;
        

        protected bool isRobotOnGround
        {
            get { return isOnGround; }
            set { isOnGround = value; }
        }

        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }
        public Vector2 SpriteCenter
        {
            get { return spriteCenter; }
        }
        public Texture2D Sprite
        {
            get { return sprite; }
        }

        public Rectangle BoundingRectangle
        {
            get { return new Rectangle((int)(position.X - spriteCenter.X), (int)(position.Y - spriteCenter.Y), sprite.Width, sprite.Height); }
        }

        public GameObject()
        {
            position = new Vector2(0);
            spriteCenter = new Vector2(0);
            sprite = null;
        }
        public GameObject(Vector2 position, Texture2D sprite, Map map)
        {
            this.position = position;
            spriteCenter = new Vector2(sprite.Width / 2, sprite.Height / 2);
            this.sprite = sprite;
            this.map = map;
        }

        public abstract void Update();

        public void HandleCollisions()
        {
            // Get the player's bounding rectangle and find neighboring tiles.
            Rectangle bounds = BoundingRectangle;
            int leftTile = (int)Math.Floor((float)bounds.Left / map.TileWidth);
            int rightTile = (int)Math.Ceiling(((float)bounds.Right / map.TileWidth)) - 1;
            int topTile = (int)Math.Floor((float)bounds.Top / map.TileHeight);
            int bottomTile = (int)Math.Ceiling(((float)bounds.Bottom / map.TileHeight)) - 1;

            // Reset flag to search for ground collision.
            isOnGround = false;

            // For each potentially colliding tile,
            for (int y = topTile; y <= bottomTile; ++y)
            {
                for (int x = leftTile; x <= rightTile; ++x)
                {
                    // If this tile is collidable,
                    TileCollision collision = map.GetTileCollision(x, y);
                    if (collision != TileCollision.Passable)
                    {
                        // Determine collision depth (with direction) and magnitude.
                        Rectangle tileBounds = map.Tiles[x, y].BoundingRectangle;
                        Vector2 depth = RectangleExtensions.GetIntersectionDepth(bounds, tileBounds);
                        if (depth != Vector2.Zero)
                        {
                            float absDepthX = Math.Abs(depth.X);
                            float absDepthY = Math.Abs(depth.Y);

                            // Resolve the collision along the shallow axis.
                            if (absDepthY < absDepthX || collision == TileCollision.Platform)
                            {
                                // If we crossed the top of a tile, we are on the ground.
                                if (previousBottom <= tileBounds.Top)
                                    isOnGround = true;

                                // Ignore platforms, unless we are on the ground.
                                if (collision == TileCollision.Impassable || isOnGround)
                                {
                                    // Resolve the collision along the Y axis.
                                    Position = new Vector2(Position.X, Position.Y + depth.Y);

                                    // Perform further collisions with the new bounds.
                                    bounds = BoundingRectangle;
                                }
                            }
                            else if (collision == TileCollision.Impassable) // Ignore platforms.
                            {
                                // Resolve the collision along the X axis.
                                Position = new Vector2(Position.X + depth.X, Position.Y);

                                // Perform further collisions with the new bounds.
                                bounds = BoundingRectangle;
                            }
                        }
                    }
                }
            }

            // Save the new bounds bottom.
            previousBottom = bounds.Bottom;
        }

        public bool HandlePickUP(GameObject gameObject)
        {
            if (BoundingRectangle.Intersects(gameObject.BoundingRectangle))
                return true;
            else return false;
        }
        public bool CheckCollision(GameObject g)
        {
            if (BoundingRectangle.Intersects(g.BoundingRectangle))
            {
                return true;
            }
            else
                return false;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(sprite, BoundingRectangle, Color.White);
        }
    }
}