using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServerFinal
{
    class Card
    {
        public enum color { hreat = 0, diamond = 1, club = 2, spade = 3 }
        public enum points
        {
            two = 1, three = 2, four = 3, five = 4, six = 5, seven = 6,
            eight = 7, nine = 8, teb = 9, J = 10, Q = 11, K = 12, A = 13,
        }
        public int Value { get; set; }//这是每一张牌唯一对应的权值，权值用来排序和生成图片
        public string Color { get; set; }
        public int Point { get; set; }
        public Card(int color, int point)
        {
            Color = Enum.GetName(typeof(color), color);
            Point = point;
            Value = color + (point * 4 - 3);
        }
        public Card(int value) { Value = value; }
        public Card() { }
    }
}
