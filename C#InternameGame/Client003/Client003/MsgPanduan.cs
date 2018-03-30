using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client003
{
    class MsgPanduan
    {
        public static bool IsNewMsg = false;
        public static bool IsWinner = false;
        public static bool IsGameOver = false;


        public static void Panduan(string msg)
        {
            Form2.Msg = msg;
            if (!NetWork.IsGameStart)//游戏开始前
            {
                try
                {
                    int i = Convert.ToInt32(msg);
                    Player.MyCards.Add(msg);//游戏开始前，数字就存入玩家卡牌
                }
                catch
                {
                    Player.AllName.Add(msg);//游戏开始前，字符就是所有玩家ID
                }
            }

            //游戏开始后如果是自己的名字代表当前是自己的回合
            else if (msg == "玩家 " + Player.MyName + " 的回合")
            {

                IsNewMsg = true;
                Form2.IsMyGround = true;
            }
            else
            {
                IsNewMsg = true;
                foreach (string name in Player.AllName)//弃牌就移除这个玩家.
                {
                    if (msg == "玩家: " + name + " 弃牌")
                    {
                        Player.Players.Remove(name);
                        IsNewMsg = true;
                    }

                    else if (msg == "玩家 " + name + " 弃牌")
                    {
                        IsNewMsg = true;
                    }

                    else if (msg == "玩家 " + name + " 下注:50")
                    {
                        IsNewMsg = true;
                    }
                    else if(msg=="winner is " + Player.MyName + " win chip is ")
                    {
                        IsNewMsg = true;
                        IsWinner = true;
                    }
                    else if (msg == "游戏结束！")
                    {
                        System.Environment.Exit(0);
                    }
                    else { IsNewMsg = true; }
                }
                if (IsWinner)
                {
                    int i = Convert.ToInt32(msg);
                    Player.Chip += i;
                    IsWinner = false;
                    System.Environment.Exit(0);
                }
            }
        }
    }
}
