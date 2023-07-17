using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleShip
{
    public class Utils
    {
        private static int IND_SHIP_NAME { get { return 0; } }
        private static int IND_SHIP_LENGTH { get { return 1; } }
        private static int IND_SHIP_ORIENTATION { get { return 2; } }
        private static int IND_SHIP_X { get { return 3; } }
        private static int IND_SHIP_Y { get { return 4; } }

        /// <summary>
        /// Config must be in format:
        /// 
        /// [section1]
        /// key1=val1
        /// key2=val2
        /// key3=val3
        /// etc...
        /// 
        /// [section2]
        /// key1=val1
        /// key2=val2
        /// ...
        /// 
        /// </summary>
        /// <param name="configFile"></param>
        /// <returns></returns>
        public static Player? LoadPlayerFromConfig(string configFile)
        {
            string? name = null;
            string? width = null;
            string? height = null;
            Board? b;

            string? curSection = null;
            using StreamReader? sr = new(configFile);
            string? line = null;
            List<Ship>? ships = new();

            while ((line = sr.ReadLine()) != null)
            {
                if (line.StartsWith(';'))
                    continue;

                if (line.StartsWith('['))
                {
                    curSection = line[1..^1].ToUpper();
                }
                else if (curSection != null && !string.IsNullOrEmpty(line))
                {
                    string[]? keyVal = null;
                    switch (curSection)
                    {
                        case "PLAYER":
                            keyVal = line.Split('=');
                            if (keyVal[0].ToUpper().Equals("NAME"))
                                name = keyVal[1];
                            break;
                        case "BOARD":
                            keyVal = line.Split('=');
                            if (keyVal[0].ToUpper().Equals("WIDTH"))
                                width = keyVal[1];
                            else if (keyVal[0].ToUpper().Equals("HEIGHT"))
                                height = keyVal[1];
                            break;
                        case "SHIPS":
                            keyVal = line.Split(',');
                            ships.Add(new(keyVal[IND_SHIP_NAME],
                                            int.Parse(keyVal[IND_SHIP_LENGTH]),
                                            int.Parse(keyVal[IND_SHIP_ORIENTATION]),
                                            int.Parse(keyVal[IND_SHIP_X]),
                                            int.Parse(keyVal[IND_SHIP_Y])
                                          ));
                            break;
                    }
                }
            }

            if (name == null || width == null || height == null || ships == null || ships.Count == 0)
                return null;

            b = new Board(int.Parse(width), int.Parse(height));

            for(int i = 0; i < ships.Count; i++)
            {
                Ship curShip = ships[i];
                for(int l = 0; l < curShip.Length; l++)
                {
                    int x = curShip.X;
                    int y = curShip.Y;

                    if (curShip.Orientation == 0) x = curShip.X + l;
                    else if (curShip.Orientation == 1) y = curShip.Y + l;

                    if (x < 0 || x >= b.Width || y < 0 || y >= b.Height)
                        throw new IndexOutOfRangeException();

                    b.Set(x, y, i + 1);
                }
            }

            return new(name, ships, b);
        }
    }
}
