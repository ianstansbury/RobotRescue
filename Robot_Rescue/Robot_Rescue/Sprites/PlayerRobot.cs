using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Robot_Rescue.Sprites
{
    class PlayerRobot : Sprite
    {
        const string ROBOT_ASSESTNAME = "Characters\\man2";
        const int START_POSITION_X = 125;
        const int START_POSITION_Y = 245;
        const int ROBOT_SPEED = 160;
        const int MOVE_UP = -1;
        const int MOVE_DOWN = 1;
        const int MOVE_LEFT = -1;
        const int MOVE_RIGHT = 1;

        enum State
        {
            Walking,
            Jumping
        }

        #region Fields
        State rCurrentState = State.Walking;
      //  private Vector2 RobotPosition;
        Vector2 rDirection = Vector2.Zero;
        Vector2 rSpeed = Vector2.Zero;

        KeyboardState mPreviousKeyboardState;

        Vector2 mStartingPosition = Vector2.Zero;
        #endregion 

        #region Constructor
    /*    public PlayerRobot(Game game, String textureName, SpriteBatch batch, Vector2 initialPosition, Vector2 initialVeocity) :
            base(game, textureName, batch, initialPosition, initialVeocity)
        {
            RobotPosition=initialPosition;
        } */  
        #endregion 

        public void LoadContent(ContentManager theContentManager)
        {
            Position = new Vector2(START_POSITION_X, START_POSITION_Y);
            base.LoadContent(theContentManager, ROBOT_ASSESTNAME);
        }

        public void Update(GameTime theGameTime)
        {
            KeyboardState aCurrentKeyboardState = Keyboard.GetState();

            UpdateMovement(aCurrentKeyboardState);
            UpdateJump(aCurrentKeyboardState);

            mPreviousKeyboardState = aCurrentKeyboardState;

            base.Update(theGameTime, rSpeed, rDirection);
        }

        private void UpdateMovement(KeyboardState aCurrentKeyboardState)
        {
            if (rCurrentState == State.Walking)
            {
                rSpeed = Vector2.Zero;
                rDirection = Vector2.Zero;

                if (aCurrentKeyboardState.IsKeyDown(Keys.Left) == true)
                {
                    rSpeed.X = ROBOT_SPEED;
                    rDirection.X = MOVE_LEFT;
                }
                else if (aCurrentKeyboardState.IsKeyDown(Keys.Right) == true)
                {
                    rSpeed.X = ROBOT_SPEED;
                    rDirection.X = MOVE_RIGHT;
                }

                if (aCurrentKeyboardState.IsKeyDown(Keys.Up) == true)
                {
                    rSpeed.Y = ROBOT_SPEED;
                    rDirection.Y = MOVE_UP;
                }
                else if (aCurrentKeyboardState.IsKeyDown(Keys.Down) == true)
                {
                    rSpeed.Y = ROBOT_SPEED;
                    rDirection.Y = MOVE_DOWN;
                }
            }
        }

        private void UpdateJump(KeyboardState aCurrentKeyboardState)
        {
            if (rCurrentState == State.Walking)
            {
                if (aCurrentKeyboardState.IsKeyDown(Keys.Space) == true && mPreviousKeyboardState.IsKeyDown(Keys.Space) == false)
                {
                    Jump();
                }
            }

            if (rCurrentState == State.Jumping)
            {
                if (mStartingPosition.Y - Position.Y > 150)
                {
                    rDirection.Y = MOVE_DOWN;
                }

                if (position.Y > mStartingPosition.Y)
                {
                    position.Y = mStartingPosition.Y;
                    rCurrentState = State.Walking;
                    rDirection = Vector2.Zero;
                }
            }
        }

        private void Jump()
        {
            if (rCurrentState != State.Jumping)
            {
                rCurrentState = State.Jumping;
                mStartingPosition = Position;
                rDirection.Y = MOVE_UP;
                rSpeed = new Vector2(ROBOT_SPEED, ROBOT_SPEED);
            }
        }
    }
}
