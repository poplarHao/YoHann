using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;


namespace GameServerFinal
{
    class GameFlow
    {
        public static int AllPlayerNum = 4;
        public static bool IsGameOver = false;
        public static string WinPlayerName = null;
        public static List<object> listAllCards = new List<object>();//所有牌
        public static List<object> listAllPlayer = new List<object>();//所有玩家的一个数组
        public static List<string> playerName = null;
        public static List<string> remote = null;
        public static List<string> PlayerGround = null;
        public static int i=0;


        public static void First(List<string> s, List<string> y)
        {
            playerName = s;
            remote = y;
        }
        public static void GameStart()
        {
            Player player1 = new Player(playerName[0], remote[0]);
            Player player2 = new Player(playerName[1], remote[1]);
            Player player3 = new Player(playerName[2], remote[2]);
            Player player4 = new Player(playerName[3], remote[3]);

            listAllCards = CardOperation.CreateCards();//生成所有牌

            PlayerGround = playerName;

            SendAllPlayerName(playerName);//给所有客户端发送所有ID

            //给所有玩家发牌
            foreach (Player p in listAllPlayer)
            {
                p.listPlayerCards = CardOperation.GiveAndSortPlayerCards(listAllCards);
                p.playerCardSize = Convert.ToInt32(WinLogicCode.getPlayerCardSize(p.listPlayerCards));

                //将牌权值发送至客户端
                for (int i = 0; i < 3; i++)
                {
                    Card poker = new Card();
                    poker = (Card)p.listPlayerCards[i];
                    SendSingleMsg(poker.Value.ToString(), p.remotePoint);
                    Thread.Sleep(1000);
                }
            }

            SendAllMsg("GameStart");
            Thread.Sleep(1000);
            MsgControl.IsGroundOver = true;

            GroundSystem();

        }

        /// <summary>
        /// 向一个客户端发送消息
        /// </summary>
        /// <param name="sendMsg"></param>
        /// <param name="remote"></param>
        public static void SendSingleMsg(string sendMsg, string remote)
        {
            byte[] arrSendMsg = Encoding.UTF8.GetBytes(sendMsg);
            Form1.connectClient[remote].Send(arrSendMsg);
        }
        /// <summary>
        /// 向所有客户端发送消息
        /// </summary>
        /// <param name="sendMsg"></param>
        public static void SendAllMsg(string sendMsg)
        {
            byte[] arrSendMsg = Encoding.UTF8.GetBytes(sendMsg);
            foreach (Socket sk in Form1.connect)
            {
                sk.Send(arrSendMsg);
            }
        }

        /// <summary>
        /// 发送所有玩家昵称
        /// </summary>
        /// <param name="name"></param>
        public static void SendAllPlayerName(List<string> name)
        {
            foreach(string str in name)
            {
                SendAllMsg(str);
                Thread.Sleep(1000);
            }
        }

        /// <summary>
        /// 发送这个ID表示是这个用户的回合
        /// </summary>
        public static void GroundSystem()
        {
            while (true)
            {
                if (MsgControl.IsGroundOver)
                {
                    if (PlayerGround.Count == 1)
                    {
                        SendAllMsg("winner is " + PlayerGround[0] + " win chip is ");
                        Thread.Sleep(1000);
                        SendAllMsg(Player.AllChip.ToString());
                        Thread.Sleep(1000);
                        SendAllMsg("游戏结束！");
                        Thread.Sleep(1000000);
                    }
                    else
                    {
                        Thread.Sleep(1000);
                        if (i >= PlayerGround.Count)
                        {
                            i -= PlayerGround.Count;
                        }
                        string str = PlayerGround[i];
                        SendAllMsg("玩家 " + str + " 的回合");
                        i++;
                        MsgControl.IsGroundOver = false;
                    }
                }
            }
        }
    }
}
