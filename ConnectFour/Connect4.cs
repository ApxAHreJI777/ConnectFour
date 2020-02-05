using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConnectFour
{
    public class Connect4
    {
        public Board gameBoard;
        public Player player1, player2;
        public State Winner { get; private set; }

        public Connect4(Player p1, Player p2, Board b)
        {
            this.player1 = p1;
            this.player2 = p2;
            this.gameBoard = b;
            Winner = State.empty;
        }

        public bool NextTurn()
        {
            int move = -1;
            if (gameBoard.CurPlayer == State.first)
                move = this.player1.Play(gameBoard); 
            else
                move = this.player2.Play(gameBoard);
            if (move != -1)
                gameBoard.MakeMove(move);
            this.Winner = gameBoard.TestVictory();
            return this.Winner != State.empty;
        }
    }
}
