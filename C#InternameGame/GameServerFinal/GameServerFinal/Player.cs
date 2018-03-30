using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServerFinal
{
    class Player
    {
        public static int AllChip { get; set; }
        public static int MinChip = 50;
        public string remotePoint { get; set; }
        public string playerName { get; set; }

        public int playerCardSize { get; set; }

        public List<object> listPlayerCards = new List<object>();

        public Player(string name, string remote)
        {
            playerName = name;
            remotePoint = remote;
            GameFlow.listAllPlayer.Add(this);
        }
        public Player() { }
    }
}
