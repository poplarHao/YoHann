using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace GameServerFinal
{
    public partial class Form1 : Form
    {

        public static int readyUser = 0;
        public string remotePoint = null;
        public Socket skWatch = null;
        public static List<string> playersName = new List<string>();//保存所有用户名
        public static List<Socket> connect = new List<Socket>();
        public List<string> playersRemote = new List<string>();
        //节点对应通信线程的一个集合，用于点对点发送消息
        public static Dictionary<string, Socket> connectClient = new Dictionary<string, Socket> { };
        public static Dictionary<string, string> PointName = new Dictionary<string, string> { };



        public Form1()
        {
            InitializeComponent();
            //使得可以访问其他线程创建的控件
            Control.CheckForIllegalCrossThreadCalls = false;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            txtLocalIP.Text = GetLocalIP();
        }

        /// <summary>
        /// 获取本机IP并返回
        /// </summary>
        /// <returns></returns>
        public string GetLocalIP()//获取本机IP地址
        {
            string name = Dns.GetHostName();
            IPAddress[] ipadrlist = Dns.GetHostAddresses(name);
            foreach (IPAddress ipa in ipadrlist)
            {
                if (ipa.AddressFamily == AddressFamily.InterNetwork)
                    return ipa.ToString();
            }
            return null;
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            btnOpen.Text = "服务器已启动";
            btnOpen.Enabled = false;

            skWatch = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint pointWatch = new IPEndPoint(IPAddress.Parse(txtLocalIP.Text), int.Parse(txtLocalPort.Text.Trim()));
            skWatch.Bind(pointWatch);
            skWatch.Listen(5);

            Thread thWatch = new Thread(watchConnect);
            thWatch.Start();
        }

        /// <summary>
        /// 监听连接，为每一个客户端新建通信线程
        /// </summary>
        private void watchConnect()
        {
            Socket skConnection = null;
            while (readyUser!=4)
            {
                try
                {
                    
                    //新建套接字保持点对点通信
                    skConnection = skWatch.Accept();
                    //获取客户端节点信息,在列表框中显示，加入集合
                    remotePoint = skConnection.RemoteEndPoint.ToString();
                    listConnectUser.Items.Add(remotePoint);
                    connectClient.Add(remotePoint, skConnection);
                    connect.Add(skConnection);
                    playersRemote.Add(remotePoint);

                    ParameterizedThreadStart pts = new ParameterizedThreadStart(ReceiveMsg);
                    Thread thread = new Thread(pts);
                    thread.IsBackground = true;
                    thread.Start(skConnection);
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    break;
                }
            }
        }

        private void ReceiveMsg(object boj)
        {
            Socket skReceive = boj as Socket;
            int i = 0;
            while (true)
            {
                byte[] arrRecMsg = new byte[1024 * 1024];
                try
                {
                    if (i == 1)
                    {
                        //字节数组转换为字符串
                        int length = skReceive.Receive(arrRecMsg);
                        string strRecMsg = Encoding.UTF8.GetString(arrRecMsg, 0, length);
                        //这里放玩家传入的字节进行相应操作
                        if (strRecMsg == "ready")
                        {
                            readyUser++;
                            i = 1;
                        }
                        MsgControl.MsgJudge(strRecMsg, skReceive);
                    }
                    else
                    {
                        //第一次客户端传入玩家ID，当点击准备即传入ready。
                        int length = skReceive.Receive(arrRecMsg);
                        string strRecMsg = Encoding.UTF8.GetString(arrRecMsg, 0, length);
                        if (strRecMsg == "ready")
                        {
                            readyUser++;
                            i = 1;
                        }
                        else
                        {
                            //保存玩家信息
                            PointName.Add(skReceive.RemoteEndPoint.ToString(), strRecMsg);
                            playersName.Add(strRecMsg);
                        }
                        if (readyUser == 4)//当准备玩家数等于4，开辟新线程开始游戏
                        {
                            GameFlow.First(playersName, playersRemote);
                            Thread thGame = new Thread(GameFlow.GameStart);
                            thGame.Start();
                        }
                    }
                }
                catch(Exception ex)//若客户端断开连接，则移除该客户端所有信息。
                {
                    listConnectUser.Items.Remove(skReceive.RemoteEndPoint.ToString());
                    connectClient.Remove(skReceive.RemoteEndPoint.ToString());
                    playersName.Remove(PointName[skReceive.RemoteEndPoint.ToString()]);
                    connect.Remove(skReceive);
                    skReceive.Close();
                    readyUser--;
                    break;
                }
            }
        }

    }
}
