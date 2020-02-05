using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConnectFour
{
    class AIMiniMaxN : AI
    {
        public int count = 0;
        private int MAX_DEEP = 6;
        private State playerNumber;
        public override int Play(Board CurGame)
        {
            playerNumber = CurGame.CurPlayer;
            List<int> movesList = CurGame.GetValidMoves();
            int bestVal = int.MinValue;
            int ColumnToPlay = -1;
            foreach (int T in movesList)
            {
                int eval = Evaluate(CurGame, T, 0);
                if (eval > bestVal)
                {
                    bestVal = eval;
                    ColumnToPlay = T;
                }
            }
            return ColumnToPlay;
        }

        public int Evaluate(Board CurGame, int col, int depth)
        {
            State Result = CurGame.TestVictory();
            if (Result != State.empty)
            {
                if (Result == State.draw)
                    return 0;
                return (Result == CurGame.CurPlayer) ? 10000 : -15000;
            }
            // cut deep
            if (depth == MAX_DEEP)
            {
                return DeapthCut(CurGame, col);
            }
            CurGame.MakeMove(col);
            List<int> movesList = CurGame.GetValidMoves();
            int alpha = int.MinValue;
            foreach (int T in movesList)
            {
                alpha = Math.Max(alpha, -1 * Evaluate(CurGame, T, depth + 1));
            }
            CurGame.UnMove(col);
            return alpha;
        }

        public int DeapthCut(Board CurGame, int move)
        {
            int n1, n2;
            int value1 = 0, value2 = 0;

            CurGame.MakeMove(move);
            int[] pInCol = CurGame.piecInCol;

            for (int row = 0; row < CurGame.Height; row++)
                for (int col = 0; col < CurGame.Width; col++)
                {
                    //if (row == pInCol[col]) break;
                    // horizontal
                    count++;
                    if (col < CurGame.Width - 3)
                    {
                        n1 = 0; n2 = 0;
                        for (int i = 0; i < 4; i++)
                        {
                            if (CurGame.BoardFields[row, col + i] == State.first) n1++;
                            if (CurGame.BoardFields[row, col + i] == State.second) n2++;
                        }
                        SetValues(n1, n2, ref value1, ref value2);
                        // diagonal l-u
                        if (row < CurGame.Height - 3)
                        {
                            n1 = 0; n2 = 0;
                            for (int i = 0; i < 4; i++)
                            {
                                if (CurGame.BoardFields[row + i, col + i] == State.first) n1++;
                                if (CurGame.BoardFields[row + i, col + i] == State.second) n2++;
                            }
                            SetValues(n1, n2, ref value1, ref value2);
                        }
                    }
                    // vertical
                    if (row < CurGame.Height - 3)
                    {
                        n1 = 0; n2 = 0;
                        for (int i = 0; i < 4; i++)
                        {
                            if (CurGame.BoardFields[row + i, col] == State.first) n1++;
                            if (CurGame.BoardFields[row + i, col] == State.second) n2++;
                        }
                        SetValues(n1, n2, ref value1, ref value2);
                        // diagonal r-d
                        if (col >= CurGame.Width + 4)
                        {
                            n1 = 0; n2 = 0;
                            for (int i = 0; i < 4; i++)
                            {
                                if (CurGame.BoardFields[row + i, col - i] == State.first) n1++;
                                if (CurGame.BoardFields[row + i, col - i] == State.second) n2++;
                            }
                            SetValues(n1, n2, ref value1, ref value2);
                        }
                    }
                }

            CurGame.UnMove(move);
            return (playerNumber == State.first) ? value2 - value1 : value1 - value2;
        }
        
        public void SetValues(int n1, int n2, ref int v1, ref int v2)
        {
            if(n2 == 0) 
                switch(n1)
                {
                    case 1: v1 += 1; break;
                    case 2: v1 += 5; break;
                    case 3: v1 += 100; break;
                }
            if(n1 == 0) 
                switch(n2)
                {
                    case 1: v2 += 2; break;
                    case 2: v2 += 10; break;
                    case 3: v2 += 400; break;
                }
        }
    }
}
