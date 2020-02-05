using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Runtime.InteropServices;
using System.IO.Pipes;
using System.IO;

namespace ConnectFour
{
    public partial class Form1 : Form
    {
        public Board gameBoard;
        public Player player1, player2;
        public Connect4 game;
        public bool isStarted = false;
        public State[,] b;
        public string pipeName = "Connect4Pipe";

        public Form1()
        {
            InitializeComponent();
            comboBox1.SelectedIndex = 0;
            comboBox2.SelectedIndex = 2;
        }

        private void button_Click(object sender, EventArgs e)
        {
            int input = Convert.ToInt32(((Button)sender).Text) - 1;
            using (NamedPipeClientStream pipeClient =
            new NamedPipeClientStream(pipeName))
            {
                try
                {
                    pipeClient.Connect(10);

                    using (StreamWriter sw = new StreamWriter(pipeClient))
                    {
                        sw.AutoFlush = true;
                        sw.WriteLine(input + "-<EOM>");
                    }
                }
                catch (TimeoutException)
                {
                    Console.WriteLine("Time Out");
                }
            }
        }

        private void startGame(object sender, EventArgs e)
        {
            switch (comboBox1.SelectedIndex)
            {
                case 0: player1 = new Human(pipeName); break;
                case 1: player1 = new AIRandom(); break;
                case 2: player1 = new AIMiniMax(); break;
            }
            switch (comboBox2.SelectedIndex)
            {
                case 0: player2 = new Human(pipeName); break;
                case 1: player2 = new AIRandom(); break;
                case 2: player2 = new AIMiniMax(); break;
            }
            gameBoard = new Board(6, 7);
            game = new Connect4(player1, player2, gameBoard);
            Thread t = new Thread(mainLoop);
            t.Start();
        }

        public void mainLoop()
        {
            isStarted = true;
            b = (State[,])game.gameBoard.BoardFields.Clone();
            pictureBox1.Invalidate();
            while (!game.NextTurn())
            {
                b = (State[,])game.gameBoard.BoardFields.Clone();
                pictureBox1.Invalidate();
                switch(game.gameBoard.CurPlayer)
                {
                    case State.first: 
                        label3.BeginInvoke(new Action(() => { label3.Text = "Player1's turn"; }));
                        break;
                    case State.second:
                        label3.BeginInvoke(new Action(() => { label3.Text = "Player2's turn"; }));
                        break;
                }
                
            }
            b = (State[,])game.gameBoard.BoardFields.Clone();
            pictureBox1.Invalidate();
            switch (game.Winner)
            {
                case State.first:
                    label3.BeginInvoke(new Action(() => { label3.Text = "Player1 won!"; }));
                    break;
                case State.second:
                    label3.BeginInvoke(new Action(() => { label3.Text = "Player2 Won!"; }));
                    break;
            }
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            Pen blackPen = new Pen(Color.Black);
            g.Clear(Color.White);
            g.DrawRectangle(blackPen, 0, 0, pictureBox1.Width - 1, pictureBox1.Height - 1);
            if (isStarted)
            {
                
                SolidBrush blueBrush = new SolidBrush(Color.Blue);
                SolidBrush redBrush = new SolidBrush(Color.Red);
                
                int cellHeight = (int)(pictureBox1.Height / game.gameBoard.Height);
                int cellWidth = (int)(pictureBox1.Width / game.gameBoard.Width);

                for (int row = game.gameBoard.Height - 1; row >= 0; row--)
                {
                    for (int col = 0; col < game.gameBoard.Width; col++)
                    {
                        switch (b[row, col])
                        {
                            case State.first:
                                g.FillEllipse(blueBrush, col * cellWidth, (game.gameBoard.Height - 1 - row) * cellHeight, cellWidth, cellHeight);
                                break;
                            case State.second:
                                g.FillEllipse(redBrush, col * cellWidth, (game.gameBoard.Height - 1 - row) * cellHeight, cellWidth, cellHeight);
                                break;
                        }
                    }
                }
                

                for (int col = 1; col < game.gameBoard.Width; col++)
                {
                    g.DrawLine(blackPen, col * cellWidth, 0, col * cellWidth, pictureBox1.Height);
                }
            }

        }

    }
}
