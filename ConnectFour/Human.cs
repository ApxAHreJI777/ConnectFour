using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.IO.Pipes;

namespace ConnectFour
{
    class Human : Player
    {
        string pipeName;

        public Human(string pipeName)
        {
            this.pipeName = pipeName;
        }

        public override int Play(Board CurGame)
        {
            bool OK = false;
            int col = -1;
            
            while (!OK)
            {
                using (NamedPipeServerStream pipe =
                new NamedPipeServerStream(pipeName))
                {
                    pipe.WaitForConnection();
                    try
                    {
                        using (StreamReader sr = new StreamReader(pipe))
                        {
                            string line;
                            if (!(line = sr.ReadLine()).Contains("<EOM>")) continue;
                            string[] input = line.Split('-');
                            lock (this)
                            {
                                col = Convert.ToInt32(input[0]);
                                OK = CurGame.IsMoveValid(col);
                            }
                        }
                    }
                    catch (IOException e)
                    {
                        Console.WriteLine("ERROR: {0}", e.Message);
                    }
                }
            }
            return col;
            
        }
    }
}
