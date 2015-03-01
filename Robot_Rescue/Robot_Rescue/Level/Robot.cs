using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

using Microsoft.Xna.Framework.Content;

namespace Robot_Rescue.Level
{
    class Robot : GameObject
    {

        #region Fields
        State rCurrentState = State.Walking;

        KeyboardState rPreviousKeyboardState;

        Vector2 mStartingPosition = Vector2.Zero;
   
        private float jumpMax = 30;
        float jumpCurrent = 0;
        State PlayerState;
        SoundEffect soundJump;
        #endregion

     
       // List<SoundEffectInstance> RobotSounds; 

        public Robot()
            : base()
        { }
        public Robot(Vector2 position, Texture2D sprite, Map map) : base(position, sprite, map) { }
        public Robot(Vector2 position, Texture2D sprite, Map map, SoundEffect sound1)
            : base(position, sprite, map) 
        {
            PlayerState = State.Walking;
            this.soundJump = sound1;
        }    

        enum State
        {
            Walking,
            Jumping
        }   

      

        #region XNA Override
        public override void Update()
        {
            Position += new Vector2(0, 5);

            KeyboardState rCurrentKeyboardState = Keyboard.GetState();
           
         
         //   UpdateMovement(aCurrentKeyboardState);
          //  UpdateJump(aCurrentKeyboardState);

            if (rCurrentKeyboardState.IsKeyDown(Keys.Right))
                Position += new Vector2(6, 0);
            if (rCurrentKeyboardState.IsKeyDown(Keys.Left))
                Position += new Vector2(-6, 0);

            if (PlayerState == State.Walking && isRobotOnGround)
            {
                if ((rCurrentKeyboardState.IsKeyDown(Keys.Space) == true) && (rPreviousKeyboardState.IsKeyDown(Keys.Space) == false))
                {
                    PlayerState = State.Jumping;
                    jumpCurrent = 0;
                }
            }
                //Position += new Vector2(-6, 0);

            Jump();
            rCurrentKeyboardState = rPreviousKeyboardState;
 
            HandleCollisions();
        }

        private void UpdateJump(KeyboardState aCurrentKeyboardState)
        {
            if (rCurrentState == State.Walking)
            {
                if (aCurrentKeyboardState.IsKeyDown(Keys.Space) == true && rPreviousKeyboardState.IsKeyDown(Keys.Space) == false)
                {
                  //  Position += new Vector2(0, -10);
                    Jump();
                }
            }

            if (rCurrentState == State.Jumping)
            {
                 if (mStartingPosition.Y - Position.Y > 5)
                {
                   // Direction.Y = MOVE_DOWN;
                    Position += new Vector2(0, 10);
                }

                if (isRobotOnGround)
                {
                 //    Position = new Vector2(Position.X, Vector2.Zero.Y);
                    
                    rCurrentState = State.Walking;                   
                }
            }
        }

        public void Update(Tile[,] tiles, int tilesWidth)
        {
            Position += new Vector2(0, 5);

            KeyboardState keyBoard = Keyboard.GetState();

            if (keyBoard.IsKeyDown(Keys.Right))
                Position += new Vector2(2, 0);
            if (keyBoard.IsKeyDown(Keys.Left))
                Position += new Vector2(-2, 0);
            if (keyBoard.IsKeyDown(Keys.Space))
                Position += new Vector2(0, -10);

            //base.Update(tiles, tilesWidth);
        }


        #endregion

        #region Helper Methods
        private void Jump()
        {
            if (PlayerState == State.Jumping && jumpCurrent < jumpMax)
            {
                if(soundJump != null)
                soundJump.Play(0.1f, 0.0f, 0.0f);
                Position += new Vector2(0, -10);
                jumpCurrent += 1;
                // Speed = new Vector2(ROBOT_SPEED, ROBOT_SPEED);
            }
            else
                PlayerState = State.Walking;
        }
        #endregion 
    }
}