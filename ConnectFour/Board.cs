using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConnectFour
{
    public enum State { empty = 1, first = 2, second = 3, draw = 4};
    public class Board : ICloneable
    {
        public int Height { get; private set; }
        public int Width { get; private set; }
        public State[,] BoardFields;
        public int[] piecInCol { get; private set; }
        public State CurPlayer { get; private set; }

        public Board(int h, int w)
        {
            this.Height = h;
            this.Width = w;
            this.CurPlayer = State.first;
            this.BoardFields = new State[this.Height, this.Width];
            piecInCol = new int[Width];
            for (int col = 0; col < this.Width; col++)
            {
                piecInCol[col] = 0;
                for (int row = 0; row < this.Height; row++)
                    BoardFields[row, col] = State.empty;
            }
        }

        public void ChangePlayer()
        {
            this.CurPlayer = (this.CurPlayer == State.first) ? State.second : State.first;
            
        }

        public void MakeMove(int coloumn)
        {
            int row = this.piecInCol[coloumn];
            this.BoardFields[row, coloumn] = this.CurPlayer;
            this.piecInCol[coloumn]++;
            this.ChangePlayer();
        }

        public void UnMove(int coloumn)
        { 
            this.piecInCol[coloumn]--;
            int row = this.piecInCol[coloumn];
            this.BoardFields[row, coloumn] = State.empty;
            this.ChangePlayer();
        }

        public State GetFieldState(int row, int col)
        {
            return this.BoardFields[row, col];
        }

        public void SetFieldState(int row, int col, State st)
        {
            this.BoardFields[row, col] = st;
        }

        public State TestVictory()
        {
            // draw
            bool draw = true;
            for (int col = 0; col < this.Width; col++)
                if (IsMoveValid(col))
                {
                    draw = false;
                    break;
                }
            if (draw)
                return State.draw;

            // horizontal
            for (int row = 0; row < this.Height; row++ )
                for (int col = 0; col < this.Width - 3; col++)
                {
                    if ((BoardFields[row, col] != State.empty)
                        && (BoardFields[row, col] == BoardFields[row, col + 1])
                        && (BoardFields[row, col] == BoardFields[row, col + 2])
                        && (BoardFields[row, col] == BoardFields[row, col + 3]))
                        return BoardFields[row, col];
                }
            // vertical
            for (int row = 0; row < this.Height - 3; row++)
                for (int col = 0; col < this.Width; col++)
                {
                    if ((BoardFields[row, col] != State.empty)
                        && (BoardFields[row, col] == BoardFields[row + 1, col])
                        && (BoardFields[row, col] == BoardFields[row + 2, col])
                        && (BoardFields[row, col] == BoardFields[row + 3, col]))
                        return BoardFields[row, col];
                }
            // diagonal 1
            for (int row = 0; row < this.Height - 3; row++)
                for (int col = 0; col < this.Width - 3; col++)
                {
                    if ((BoardFields[row, col] != State.empty)
                        && (BoardFields[row, col] == BoardFields[row + 1, col + 1])
                        && (BoardFields[row, col] == BoardFields[row + 2, col + 2])
                        && (BoardFields[row, col] == BoardFields[row + 3, col + 3]))
                        return BoardFields[row, col];
                }
            // diagonal 2
            for (int row = this.Height - 1; row >= 3; row--)
                for (int col = 0; col < this.Width - 3; col++)
                {
                    if ((BoardFields[row, col] != State.empty)
                        && (BoardFields[row, col] == BoardFields[row - 1, col + 1])
                        && (BoardFields[row, col] == BoardFields[row - 2, col + 2])
                        && (BoardFields[row, col] == BoardFields[row - 3, col + 3]))
                        return BoardFields[row, col];
                }
            return State.empty;
        }

        public bool IsMoveValid(int col)
        {
            if (col >= Width) return false;
            return piecInCol[col] < Height;
        }

        public List<int> GetValidMoves()
        {
            List<int> ValidMoves = new List<int> { };
            for (int col = 0; col < this.Width; col++)
                if (IsMoveValid(col))
                    ValidMoves.Add(col);
            return ValidMoves;
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
