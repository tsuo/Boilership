using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleShip
{
    public class Ship
    {
        public string Name { get; set; }
        public int Length { get; set; }
        public int Orientation { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public Ship(string name, int length, int rot, int x, int y)
        {
            Name = name;
            Length = length;
            Orientation = rot;
            X = x;
            Y = y;
        }
    }
}
