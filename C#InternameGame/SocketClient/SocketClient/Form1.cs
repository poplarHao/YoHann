using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SocketClient
{
    public partial class Form1 : Form
    {
        Thread thClient = null;
        Socket skClient = null;
        


        public Form1()
        {
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            btnConnect.Enabled = false;

            skClient=new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPAddress ipServer = IPAddress.Parse(txtIP.Text);
            IPEndPoint pointClient = new IPEndPoint(ipServer, Convert.ToInt32(txtPort.Text));//绑定一个网络节点

            try
            {
                skClient.Connect(pointClient);//连接这个网络节点
            }
            catch
            {
                MessageBox.Show("连接失败，请确认服务器开启");
                btnConnect.Enabled = true;
                return;
            }

            thClient = new Thread(Receive);
            thClient.IsBackground = true;
            thClient.Start();

        }

        private void Receive()
        {
            while (true)
            {
                try
                {
                    byte[] arrRecvmsg = new byte[1024 * 1024];

                    int length = skClient.Receive(arrRecvmsg);

                    string strRevMsg = Encoding.UTF8.GetString(arrRecvmsg, 0, length);
                    
                    txtMsg.Items.Add("服务器:"  + strRevMsg);
                    
                }
                catch(Exception ex)
                {
                    txtMsg.Items.Add("服务器已经关闭");
                    break;
                }
            }
        }

        public void ClientSendMsg(string sendMsg)
        {
            byte[] arrSendMsg = Encoding.UTF8.GetBytes(sendMsg);
            skClient.Send(arrSendMsg);
            txtMsg.Items.Add("我:" + sendMsg);
        }


        private void btnSend_Click(object sender, EventArgs e)
        {
            ClientSendMsg(txtSendMsg.Text.Trim());
            txtSendMsg.Clear();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            txtIP.Text = "192.168.43.68";
            txtPort.Text = "7788";
        }
    }
}
