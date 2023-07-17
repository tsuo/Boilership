using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleShip
{
    public class BattleShip
    {
        private Player? Player1 { get; set; }
        private Player? Player2 { get; set; }
        public BattleShip()
        {
            int turn = 0;

            // load config for player 1
            Player1 = Utils.LoadPlayerFromConfig("config.cfg");

            // load config for player 2
            Player2 = Utils.LoadPlayerFromConfig("config2.cfg");


            Player[]? rotation = new Player[] { Player1, Player2 };

            if (Player1 != null && Player2 != null) {
                while (!Player1.Won && !Player2.Won)
                {
                    Console.Clear();
                    Console.WriteLine(Player1.Name);
                    Console.WriteLine(Player1.Board);

                    Console.WriteLine(Player2.Name);
                    Console.WriteLine(Player2.Board);

                    turn %= 2;
                    Console.WriteLine($"Player {rotation[turn].Name}:");
                    rotation[turn].SendSignal(rotation[(turn + 1) % 2]);
                    turn++;
                    Console.ReadLine();
                }
                Console.WriteLine("Game End!");
            }
            else {
                Console.WriteLine("Failed to load Players");
            }
        }
    }

    public class Player
    {
        public string Name { get; set; }
        public bool Won { get; set; }
        public List<Ship> Ships { get; set; }
        public Board Board { get; set; }
        public int NumDestroyed
        {
            get 
            {
                int count = 0;
                foreach (var ship in Ships)
                {
                    if (ship.Length == 0)
                    {
                        count++;
                    }
                }
                return count;
            }
        }

        public Player(string name, List<Ship> ships, Board board)
        {
            Won = false;
            Name = name;
            Ships = ships;
            Board = board;
        }

        public void SendSignal(Player toPlayer)
        {
            string? signal;
            if (NumDestroyed == Ships.Count)
            {
                signal = "LOSE";
            }
            else
            {
                signal = $"ATTACK,{Console.ReadLine()}";
            }

            toPlayer.ProcessSignal(this, signal);
        }

        public void ProcessSignal(Player fromPlayer, string? signal)
        {
            if(signal != null)
            {
                string[] input = signal.Split(',');
                switch (input[0])
                {
                    case "LOSE":    //LOSE
                        Won = true;
                        Console.WriteLine($"{Name} Won!");
                        break;
                    case "HIT":     //HIT,x,y
                        // record hit locally if needed
                        Console.WriteLine($"{fromPlayer.Name} was HIT at ({input[1]},{input[2]})");
                        break;
                    case "MISS":    //MISS,x,y
                        // record miss locally if needed
                        Console.WriteLine($"{Name} MISSED at ({input[1]},{input[2]})");
                        break;
                    case "ATTACK":  //ATTACK,x,y
                        int inputX = int.Parse(input[1]);
                        int inputY = int.Parse(input[2]);
                        int boardVal = Board.Get(inputX, inputY);

                        Console.WriteLine($"{fromPlayer.Name} declared an ATTACK at ({inputX},{inputY})");

                        if (boardVal <= 0)
                        {
                            fromPlayer.ProcessSignal(this, $"MISS,{inputX},{inputY}");
                        }
                        else
                        {
                            Ships[boardVal - 1].Length--;
                            Board.Set(inputX, inputY, -1);
                            fromPlayer.ProcessSignal(this, $"HIT,{inputX},{inputY}");
                        }
                        break;
                }
            }
        }
    }
}
