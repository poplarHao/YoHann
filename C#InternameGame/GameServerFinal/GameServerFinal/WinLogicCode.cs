using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServerFinal
{
    class WinLogicCode
    {
        /// <summary>
        /// 传入玩家卡牌数组，返回一个牌型大小
        /// </summary>
        /// <param name="inPlayerCards"></param>
        /// <returns></returns>
        public static string getPlayerCardSize(List<object> inPlayerCards)
        {
            string playerCardSize;
            List<string> cardColor = new List<string>();//卡牌花色，用以判断牌型大小
            List<int> cardPoint = new List<int>();//卡牌点数，用以判断牌型大小
            Card playerCard = new Card();
            for (int i = 0; i < 3; i++)
            {
                playerCard = (Card)inPlayerCards[i];
                cardColor.Add(playerCard.Color);
                cardPoint.Add(playerCard.Point);
            }
            for (int i = 0; i < 2; i++)//将最小的放在数组最后，这样playercardsize最后两位（个位和十位）影响最小。
            {
                for (int j = i + 1; j < 3; j++)
                {
                    int Temp = 0;
                    if (cardPoint[i] < cardPoint[j])
                    {
                        Temp = cardPoint[i];
                        cardPoint[i] = cardPoint[j];
                        cardPoint[j] = Temp;
                    }
                }
            }
            playerCardSize = "0";//普通牌型第一位0
            if (cardPoint[0] == cardPoint[1] && cardPoint[1] == cardPoint[2])//如果是炸弹，就是11
            {
                playerCardSize = "11";
            }
            else if (cardPoint[0] == cardPoint[1] || cardPoint[1] == cardPoint[2])//判断对子，加入了两个对子先比较对子牌点数这个特性
            {
                if (cardPoint[1] == cardPoint[2])//如果是两位较小的点数一样，则将较小的点数与较大点数交换，如33A与K44就是先比较3和4而不是比较A与K
                {
                    playerCardSize = "1";
                    int Temp = 0;
                    Temp = cardPoint[2];
                    cardPoint[2] = cardPoint[0];
                    cardPoint[0] = Temp;
                }
                else
                {
                    playerCardSize = "1";
                }
            }
            if (cardColor[0] == cardColor[1] && cardColor[1] == cardColor[2])//金花第一位3
            {
                playerCardSize = "3";
            }
            if (playerCardSize == "3" && cardPoint[0] == cardPoint[1] + 1 && cardPoint[1] == cardPoint[2] + 1)//顺金第一位4
            {
                playerCardSize = "4";
            }
            if (cardPoint[0] == cardPoint[1] + 1 && cardPoint[0] == cardPoint[2] + 2)//顺子第一位2
            {
                playerCardSize = "2";
            }
            for (int i = 0; i < 3; i++)
            {
                playerCardSize = playerCardSize + cardPoint[i].ToString().PadLeft(2, '0');//字符串拼接，不是两位数在前面填充个0，方便转换为整数比较
            }
            return playerCardSize;
        }


        /// <summary>
        /// 开牌与被开牌相比较
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <returns></returns>
        public static bool AThanB(Player p1, Player p2)
        {
            if (p1.playerCardSize > p2.playerCardSize) return true;
            else return false;
        }
    }
}
