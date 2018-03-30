using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GameServerFinal
{
    class MsgControl
    {

        public static bool IsGroundOver = false;
        public static void MsgJudge(string message,Socket sef)
        {
            int i = 0;
            if (message == "abandon")
            {
                //传入的是弃牌的相应处理
                GameFlow.i--;
                GameFlow.AllPlayerNum--;
                GameFlow.PlayerGround.Remove(Form1.PointName[sef.RemoteEndPoint.ToString()]);
                GameFlow.SendAllMsg("玩家: " + Form1.PointName[sef.RemoteEndPoint.ToString()]+" 弃牌");
                IsGroundOver = true;
            }
            else
            {
                try
                {
                    //接收到筹码信息所做的处理
                    i = Convert.ToInt32(message);
                    Player.AllChip += i;
                    Player.MinChip = i;
                    GameFlow.SendAllMsg("玩家 "+ Form1.PointName[sef.RemoteEndPoint.ToString()]+" 下注:"+message);
                    IsGroundOver = true;
                }
                catch
                {
                    //传入的是与其他玩家比较的处理
                    Player p1 = null;
                    foreach(Player p in GameFlow.listAllPlayer)
                    {
                        if(p.playerName== Form1.PointName[sef.RemoteEndPoint.ToString()])
                        {
                            p1 = p;
                        }
                    }
                    foreach(Player p2 in GameFlow.listAllPlayer)
                    {
                        if (message =="compare with "+ p2.playerName)
                        {
                            GameFlow.i--;
                            GameFlow.AllPlayerNum--;
                            if(WinLogicCode.AThanB(p1, p2))//p1赢
                            {
                                GameFlow.SendAllMsg("玩家: " + p2.playerName + " 弃牌");
                                Thread.Sleep(1000);
                                GameFlow.PlayerGround.Remove(p2.playerName);
                                IsGroundOver = true;
                            }
                            else//p2赢
                            {
                                GameFlow.SendAllMsg("玩家: " + p1.playerName + " 弃牌");
                                GameFlow.PlayerGround.Remove(p1.playerName);
                                IsGroundOver = true;
                            }
                        }
                    }
                }
            }
        }

    }
}
