using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client003
{
    class Player
    {
        public static string MyName;
        public static List<string> Players = new List<string>();
        public static List<string> AllName = new List<string>();
        public static List<string> MyCards = new List<string>();
        public static int MinChip = 50;
        public static int Chip = 1000;

        public static void Init()
        {
            foreach(string str in AllName)
            {
                if (str != MyName)
                {
                    Players.Add(str);
                }
            }
        }

    }
}
