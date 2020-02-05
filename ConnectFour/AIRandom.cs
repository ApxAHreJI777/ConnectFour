using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConnectFour
{
    class AIRandom : AI
    {
        private static Random rnd = new Random();

        public override int Play(Board CurGame)
        {
            bool OK = false;
            int col = -1;
            while (!OK)
            {
                col = rnd.Next(CurGame.Width);
                OK = CurGame.IsMoveValid(col);
            }
            return col;
        }

        //public override void Evaluate()
        //{
        //    ;
        //}
    }
}
