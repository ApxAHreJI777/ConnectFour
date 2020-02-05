using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConnectFour
{
    public abstract class Player
    {
        public abstract int Play(Board CurGame);
        public int InputVal = -1;
    }
}
