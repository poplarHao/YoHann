using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServerFinal
{
    class CardOperation
    {
        /// <summary>
        /// 创造一个包含所有卡牌的泛型数组并返回这个数组
        /// </summary>
        /// <param name="Players"></param>
        /// <returns></returns>
        public static List<object> CreateCards()
        {
            Card poker = new Card();
            List<object> AllCards = new List<object>();
            List<object> AllPlayerCards = new List<object>();
            Random r = new Random();//生成随机数
            for (int i = 0; i < 4; i++)//生成所有牌
            {
                for (int j = 1; j < 14; j++)
                {
                    AllCards.Add(new Card(i, j));
                }
            }
            for (int i = 0; i < 52; i++)//随机从数组中取出牌
            {
                int re = r.Next(0, AllCards.Count);
                AllPlayerCards.Add(AllCards[re]);
                AllCards.RemoveAt(re);
            }
            return AllPlayerCards;
        }

        /// <summary>
        /// 给玩家发牌，传入所有牌，返回一个包含玩家牌的已排序的数组
        /// </summary>
        /// <param name="allPlayerCards"></param>
        /// <param name="Players"></param>
        /// <returns></returns>
        public static List<object> GiveAndSortPlayerCards(List<object> allPlayerCards)
        {
            Card poker1 = new Card();
            Card poker2 = new Card();
            List<object> player = new List<object>();
            Random r = new Random();
            for (int i = 0; i < 3; i++)
            {
                int re = r.Next(0, allPlayerCards.Count);
                player.Add(allPlayerCards[re]);
                allPlayerCards.RemoveAt(re);
            }
            for (int i = 0; i < 2; i++)
            {
                for (int j = i + 1; j < 3; j++)
                {
                    poker1 = (Card)player[i];
                    poker2 = (Card)player[j];
                    int Temp = 0;
                    if (poker1.Value > poker2.Value)
                    {
                        Temp = poker1.Value;
                        poker1.Value = poker2.Value;
                        poker2.Value = Temp;
                    }
                }
            }
            return player;
        }
    }
}
