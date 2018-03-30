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

namespace Client003
{
    class NetWork
    {
        public static Thread thClient = null;
        public static Socket skClient = null;
        public static bool IsGameStart = false;


        /// <summary>
        /// 连接服务器并且新建通信线程
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        /// <returns></returns>
        public static bool Connect(string ip,int port)
        {
            skClient = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPAddress ipServer = IPAddress.Parse(ip);
            IPEndPoint pointClient = new IPEndPoint(ipServer, port);//绑定一个网络节点
            try
            {
                //连接后，创建新线程保持连接
                skClient.Connect(pointClient);
                thClient = new Thread(Receive);
                thClient.IsBackground = true;
                thClient.Start();
                return true;
            }
            catch
            {
                MessageBox.Show("连接失败，请确认服务器开启");
                return false;
            }
        }

        /// <summary>
        /// 接收消息
        /// </summary>
        private static void Receive()
        {
            while (true)
            {
                try
                {
                    byte[] arrRecvmsg = new byte[1024 * 1024];

                    int length = skClient.Receive(arrRecvmsg);

                    string strRevMsg = Encoding.UTF8.GetString(arrRecvmsg, 0, length);

                    if (strRevMsg == "GameStart")
                    {
                        NetWork.IsGameStart = true;
                    }
                    else
                    {
                        MsgPanduan.Panduan(strRevMsg);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("服务器已经关闭");
                    break;
                }
            }
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="sendMsg"></param>
        public static void SendMsg(string sendMsg)
        {
            byte[] arrSendMsg = Encoding.UTF8.GetBytes(sendMsg);
            skClient.Send(arrSendMsg);
        }


    }
}
