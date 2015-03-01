using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

namespace Robot_Rescue.Level
{
    enum LoadState
    {
        None,
        MapDimensions,
        TileDimensions,
        Textures,
        MapTextures,
        TileCollisions,
        MapCollisions,
        PalyerStart,
        Humans,
    }
    class Map
    {
        Tile[,] tiles;
        int tileWidth;
        int tileHeight;

        SoundEffect pickUpSound;
        Robot player;
        List<Human> humans;

        public Tile[,] Tiles
        { get { return tiles; } }
        public int TileWidth
        { get { return tileWidth; } }
        public int TileHeight
        { get { return tileHeight; } }
        public Robot Robot
        {
            get { return player; }
        }

        public Map()
        {
            tiles = new Tile[1, 1];
            int tileWidth = 0;
            int tileHeight = 0;

            humans = new List<Human>();
        }

        public void Update()
        {
            player.Update();
            for (int i = humans.Count - 1; i > -1; i--)
            {
                humans[i].Update();
                if (humans[i].HandlePickUP(player))
                {
                    pickUpSound.Play();
                    humans.RemoveAt(i);
                }
            }
            if (humans.Count < 1)
            {
                MediaPlayer.Play(GameState.SongList[2]);
                GameState.State = State.End;
            }
        }

        public void LoadMap(string mapName, ContentManager content)
        {
            pickUpSound = content.Load<SoundEffect>("Sound\\rescue");

            //Makes a Read File and Reads Frist Line
            StreamReader sr = new StreamReader(mapName + ".txt");
            string line = sr.ReadLine();

            //Used to Control LoadMap
            LoadState loadState = LoadState.None;
            bool changedLoadState;

            //Used to Split Strings
            char[] delims;
            string[] stringArray;

            //Used for the row of the 2DArrays
            int ySprite = 0;
            int yCollision = 0;

            //Lists Used
            List<Texture2D> spriteList = new List<Texture2D>();

            Texture2D humansSprit = content.Load<Texture2D>("Textures\\Human");

            //Goes though file one line at a time
            while (!sr.EndOfStream)
            {
                //PickLoadState
                if (line == "[MapDimensions]")
                {
                    loadState = LoadState.MapDimensions;
                    changedLoadState = true;
                }
                else if (line == "[TileDimensions]")
                {
                    loadState = LoadState.TileDimensions;
                    changedLoadState = true;
                }
                else if (line == "[Textures]")
                {
                    loadState = LoadState.Textures;
                    changedLoadState = true;
                }
                else if (line == "[MapTextures]")
                {
                    loadState = LoadState.MapTextures;
                    changedLoadState = true;
                }
                else if (line == "[TileCollisions]")
                {
                    loadState = LoadState.TileCollisions;
                    changedLoadState = true;
                }
                else if (line == "[MapCollisions]")
                {
                    loadState = LoadState.MapCollisions;
                    changedLoadState = true;
                }
                else if (line == "[PalyerStart]")
                {
                    loadState = LoadState.PalyerStart;
                    changedLoadState = true;
                }
                else if (line == "[Humans]")
                {
                    loadState = LoadState.Humans;
                    changedLoadState = true;
                }
                else
                {
                    changedLoadState = false;
                }

                //Parses Line
                if (!changedLoadState && line != string.Empty)
                    switch (loadState)
                    {
                        case LoadState.MapDimensions:
                            delims = new char[] { ',' };
                            stringArray = line.Split(delims);
                            tiles = new Tile[int.Parse(stringArray[0]), int.Parse(stringArray[1])];
                            break;
                        case LoadState.TileDimensions:
                            delims = new char[] { ',' };
                            stringArray = line.Split(delims);
                            tileWidth = int.Parse(stringArray[0]);
                            tileHeight = int.Parse(stringArray[1]);
                            break;
                        case LoadState.Textures:
                            spriteList.Add(content.Load<Texture2D>("Textures\\"+line));
                            break;
                        case LoadState.MapTextures:
                            delims = new char[] { ' ' };
                            stringArray = line.Split(delims);
                            CleanSpaces(ref stringArray);
                            for (int i = 0; i < stringArray.GetLength(0); i++)
                            {
                                int num = int.Parse(stringArray[i]);
                                if (num < 0)
                                {
                                    tiles[i, ySprite] = new Tile(new Vector2(i * tileWidth, ySprite * tileHeight), tileWidth, tileHeight);
                                }
                                else
                                {
                                    Texture2D sprite = spriteList[num];
                                    tiles[i, ySprite] = new Tile(new Vector2(i * tileWidth, ySprite * tileHeight), sprite);
                                }
                            }
                            ySprite++;
                            break;
                        case LoadState.MapCollisions:
                            delims = new char[] { ' ' };
                            stringArray = line.Split(delims);
                            CleanSpaces(ref stringArray);
                            for (int i = 0; i < stringArray.GetLength(0); i++)
                            {
                                int num = int.Parse(stringArray[i]);
                                tiles[i, yCollision].TileCollision = FineTileCollisions(num);
                            }
                            yCollision++;
                            break;
                        case LoadState.PalyerStart:
                            delims = new char[] { ',' };
                            stringArray = line.Split(delims);
                            player = new Robot(new Vector2(int.Parse(stringArray[0]), int.Parse(stringArray[1])), content.Load<Texture2D>("Textures\\Robot(S)"), this);
                            break;
                        case LoadState.Humans:
                            delims = new char[] { ',' };
                            stringArray = line.Split(delims);
                            humans.Add(new Human(new Vector2(int.Parse(stringArray[0]), int.Parse(stringArray[1])), humansSprit, this));
                            break;
                    }

                //Read Line
                line = sr.ReadLine();
            }

            //Closes The File
            sr.Close();
        }
        public void CleanSpaces(ref string[] stringArray)
        {
            List<string> tempList = new List<string>();

            foreach (string s in stringArray)
            {
                if (s != string.Empty)
                    tempList.Add(s);
            }

            stringArray = tempList.ToArray();
        }
        public TileCollision FineTileCollisions(int i)
        {
            switch (i)
            {
                case 0:
                    return TileCollision.Passable;
                case 1:
                    return TileCollision.Impassable;
                case 2:
                    return TileCollision.Platform;
                default:
                    return TileCollision.Passable;
            }
        }

        public void HandleCollisions(GameObject gameobject)
        {
            // Get the player's bounding rectangle and find neighboring tiles.
            Rectangle bounds = gameobject.BoundingRectangle;
            int leftTile = (int)Math.Floor((float)bounds.Left / tileWidth);
            int rightTile = (int)Math.Ceiling(((float)bounds.Right / tileWidth)) - 1;
            int topTile = (int)Math.Floor((float)bounds.Top / tileHeight);
            int bottomTile = (int)Math.Ceiling(((float)bounds.Bottom / tileHeight)) - 1;

            // Reset flag to search for ground collision.
            bool isOnGround = false;

            // For each potentially colliding tile,
            for (int y = topTile; y <= bottomTile; ++y)
            {
                for (int x = leftTile; x <= rightTile; ++x)
                {
                    tiles[x, y].Collision(isOnGround, bounds, gameobject);
                }
            }


        }
        public TileCollision GetTileCollision(int x, int y)
        {
            // Prevent escaping past the level ends.
            if (x < 0 || x >= tiles.GetLength(0))
                return TileCollision.Impassable;
            // Allow jumping past the level top and falling through the bottom.
            if (y < 0 || y >= tiles.GetLength(1))
                return TileCollision.Impassable;

            return tiles[x, y].TileCollision;
        }
        public Rectangle GetTileBoundingRectangle(int x, int y)
        {
            return new Rectangle(x * tileWidth, y * tileHeight, tileWidth, tileHeight);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Tile t in tiles)
                if (t != null)
                    t.Draw(spriteBatch);
            foreach (Human h in humans)
                h.Draw(spriteBatch);
            player.Draw(spriteBatch);
        }
    }
}