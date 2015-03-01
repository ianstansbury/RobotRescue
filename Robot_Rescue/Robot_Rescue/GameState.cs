using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Content;

namespace Robot_Rescue
{

    //--------------------
    //Use GameState to contal the contal loop of the game
    //by useing a switch statement like this in both the 
    //update and the draw methods of Game1.
    //--------------------

    //switch (gameState.State)
    //{
    //    case State.Main:
    //        mainMenu.Update();
    //        break;
    //    case State.Instructions:
    //        instructions.Update();
    //        break;
    //    case State.Credits:
    //        credits.Update();
    //        break;
    //    case State.Exit:
    //        this.Exit();
    //        break;
    //    case State.HighScores:
    //        highScores.Update();
    //        break;
    //    case State.EnteringHighScore:
    //        highScores.Update();
    //        break;
    //}

    enum State
    {
        Main,
        SinglePlayer,
        MultiPlayer,
        HighScores,
        EnteringHighScore,
        Instructions,
        Credits,
        Exit, 
        End,
    }

    static class GameState
    {
        static State state;
        static List <Song> songList;

       

        public static State State
        {
            get { return state; }
            set { state = value; }
        }
        public static List<Song> SongList
        {
            get { return songList; }
        }

        public static void Initialize(ContentManager Content)
        { 
            state = State.Main; 
            songList = new List<Song>();
            songList.Add(Content.Load<Song>(@"Sound/Diag1Rob"));
            songList.Add(Content.Load<Song>(@"Sound/Level1"));
            songList.Add(Content.Load<Song>(@"Sound/Diag2Rob"));
        }
    }
}
