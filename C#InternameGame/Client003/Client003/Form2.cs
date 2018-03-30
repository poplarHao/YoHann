using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Client003
{
    public partial class Form2 : Form
    {
        public static List<Label> LabelName = new List<Label>();
        public static List<TableLayoutPanel> Table = new List<TableLayoutPanel>();
        public static List<Bitmap> img = new List<Bitmap>();//所有牌对应的图片
        public static Dictionary<String, TableLayoutPanel> NameTable = new Dictionary<string, TableLayoutPanel> { };
        public static bool IsMyGround = false;
        public static string Msg = null;

        public Form2()
        {
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            foreach (Control c in this.Controls)//遍历所有label控件存入数组
            {
                if (c is Label)
                {
                    Label d = c as Label;
                    LabelName.Add(d);
                }
            }

            foreach (Control c in this.Controls)//遍历所有table控件存入数组
            {
                if (c is TableLayoutPanel)
                {
                    TableLayoutPanel d = c as TableLayoutPanel;
                    Table.Add(d);
                }
            }

            for(int i = 0; i < Table.Count; i++)
            {
                NameTable.Add(Player.AllName[i], Table[i]);
            }

            ShowMsg.ShowName(LabelName, Player.AllName);//显示所有玩家名字
            Resource1 s = new Resource1();
            foreach (PropertyInfo pr in s.GetType().GetProperties(BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Instance).Where(c => c.Name != "ResourceManager" && c.Name != "Culture"))
            {
                if (pr.GetValue(s, null) is Bitmap)
                {
                    Bitmap b = pr.GetValue(s, null) as Bitmap;
                    img.Add(b);
                }
            }
            labelChip.Text ="筹码：" + Player.Chip.ToString();
            Player.Init();
            ShowMsg.ShowBack(Table);
            Thread thGround = new Thread(MyGround);
            thGround.IsBackground = true;
            thGround.Start();
        }

        private void btnSeeCard_Click(object sender, EventArgs e)
        {
            btnSeeCard.Enabled = false;
            ShowMsg.ShowCards(Player.MyCards, NameTable[Player.MyName]);
        }

        private void btnAdb_Click(object sender, EventArgs e)
        {
            btnAdb.Enabled = false;
            NetWork.SendMsg("abandon");
            NameTable[Player.MyName].Controls.Clear();
            IsMyGround = false;
        }

        /// <summary>
        /// 代表自己的回合
        /// </summary>
        public void MyGround()
        {
            while (true)
            {
                if (MsgPanduan.IsNewMsg)
                {
                    listName.Items.Clear();
                    foreach (string str in Player.Players)
                    {
                        listName.Items.Add(str);
                    }
                    listMsg.Items.Add(Msg);
                    MsgPanduan.IsNewMsg = false;
                }
                if (IsMyGround)
                {
                    btnAdb.Enabled = true;
                    //btnAdd.Enabled = true;
                    btnFlow.Enabled = true;
                    btnCompare.Enabled = true;
                }
                else if (!IsMyGround)
                {
                    btnAdb.Enabled = false;
                    btnAdd.Enabled = false;
                    btnFlow.Enabled = false;
                    btnCompare.Enabled = false;
                }
            }
        }

        private void btnCompare_Click(object sender, EventArgs e)
        {
            if (listName.SelectedIndex == -1)
            {
                MessageBox.Show("请选择要比较的玩家", "提示", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            else
            {
                NetWork.SendMsg("compare with " + listName.Text);
                Player.Chip -= 50; labelChip.Text = "筹码：" + Player.Chip.ToString();
                IsMyGround = false;
            }
        }

        private void btnFlow_Click(object sender, EventArgs e)
        {
            NetWork.SendMsg("50");
            Player.Chip -= 50;          labelChip.Text ="筹码：" + Player.Chip.ToString();
            IsMyGround = false;
        }
    }
}
