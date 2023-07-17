using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleShip
{
    public class Board
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public int[,] BoardData { get; set; }
        public Board(int width, int height)
        {
            Width = width;
            Height = height;
            BoardData = new int[width, height];
        }

        public bool Set(int x, int y, int val)
        {
            if (x < 0 || x >= Width || y < 0 || y >= Height)
                return false;
            BoardData[y, x] = val;
            return true;
        }

        public int Get(int x, int y)
        {
            return BoardData[y, x];
        }

        public override string ToString()
        {
            string toRet = "";

            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                    toRet += $"{BoardData[y, x]} ";
                toRet += "\n";
            }

            return toRet;
        }
    }
}
