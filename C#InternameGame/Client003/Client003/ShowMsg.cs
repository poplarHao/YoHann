using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace Client003
{
    class ShowMsg
    {
        /// <summary>
        /// 显示玩家名
        /// </summary>
        /// <param name="label"></param>
        /// <param name="name"></param>
        public static void ShowName(List<Label> label,List<string> name)
        {
            for(int i = 0; i < name.Count; i++)
            {
                label[i].Text = name[i];//显示玩家名字
            }
        }

        /// <summary>
        /// 显示背景图片
        /// </summary>
        /// <param name="table"></param>
        public static void ShowBack(List<TableLayoutPanel> table)
        {
            foreach (TableLayoutPanel tb in table)
            {
                for (int i = 0; i < 3; i++)
                {
                    Bitmap b = Resource1.back;
                    PictureBox backimg = new PictureBox
                    {
                        Dock = DockStyle.Fill,
                        SizeMode = PictureBoxSizeMode.StretchImage,
                        Image = b
                    };
                    tb.Controls.Add(backimg);
                }
            }
        }

        public static void ShowMsgInList()
        {

        }

        /// <summary>
        /// 显示玩家手中的卡牌
        /// </summary>
        /// <param name="listCard"></param>
        /// <param name="t"></param>
        public static void ShowCards(List<string> listCard, TableLayoutPanel t)
        {
            t.Controls.Clear();
            foreach (string str in listCard)
            {
                int value = Convert.ToInt32(str);
                Bitmap b = Form2.img[--value];
                PictureBox pic = new PictureBox
                {
                    Dock = DockStyle.Fill,
                    SizeMode = PictureBoxSizeMode.StretchImage,
                    Image = b
                };
                t.Controls.Add(pic);
            }
        }
    }
}
