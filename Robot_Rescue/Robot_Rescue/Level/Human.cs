using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace Robot_Rescue.Level
{
    class Human : GameObject
    {
            public Human(Vector2 position, Texture2D sprite, Map map) : base(position, sprite, map) { }

            public override void Update()
            {
                Position += new Vector2(0, 5);

                HandleCollisions();
            }
    }

}
