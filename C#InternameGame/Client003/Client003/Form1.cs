using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Client003
{
    public partial class Form1 : Form
    {
        public static bool IsConnect = false;
        public static bool Stratr = false;
        public Form1()
        {
            InitializeComponent();

        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            IsConnect = NetWork.Connect(txtServerIP.Text, Convert.ToInt32(txtServerPort.Text));
            //连接成功就发送用户名
            if (IsConnect)
            {
                NetWork.SendMsg(txtName.Text.Trim());
                Player.MyName = txtName.Text.Trim();
                MessageBox.Show("连接成功,请准备");
            }
            else
            {
                MessageBox.Show("错误发生，请稍后再试!");
            }
        }

        private void btnReady_Click(object sender, EventArgs e)
        {
            NetWork.SendMsg("ready");
            while (!NetWork.IsGameStart)
            {
                Thread.Sleep(3000);
            }
            Form2 f2 = new Form2();
            f2.Show();
            this.Hide();
        }
        
        private void Form1_Load(object sender, EventArgs e)
        {
            
        }
    }
}
