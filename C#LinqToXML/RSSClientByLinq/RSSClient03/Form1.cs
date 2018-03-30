using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;

namespace RSSClient03
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btn_localnews_Click(object sender, EventArgs e)
        {
            loadnews();
        }

        /// <summary>
        /// 加载本地新闻栏目
        /// </summary>
        public void loadnews()
        {
            listBox_mynews.Items.Clear();
            XDocument xdoc =
                XDocument.Load(@"dingyue.xml");//新建一个XDoucument对象将XML读入XML树。
            var query = from rssFeed in xdoc.Descendants("channel")
                        select new
                        {
                            Name = rssFeed.Element("name").Value,
                            Link = rssFeed.Element("link").Value,
                        };//选出新闻节点并保存到匿名类型query中
            foreach (var item in query)
            {
                listBox_mynews.Items.Add(item.Name);
                listBox_mynews.Items.Add(item.Link);
            }//遍历输出新闻及链接
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            btn_del.Enabled = false;
        }

        private void listBox_mynews_DoubleClick(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 读取XML网页上的新闻
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listBox_mynews_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            richTextBox.Clear();
            int index = this.listBox_mynews.IndexFromPoint(e.Location);
            string url = listBox_mynews.Items[index].ToString();//获取点击的URL
            if (url.Length > 15)
            {
                XDocument xdoc = XDocument.Load(url);//新建一个XDoucument对象将XML读入XML树。
                var query = from rssFeed in xdoc.Descendants("item")
                            select new
                            {
                                Title = rssFeed.Element("title").Value,
                                PubDate=  DateTime.Parse(rssFeed.Element("pubDate").Value),
                                Description = rssFeed.Element("description").Value,
                                Link = rssFeed.Element("link").Value,
                            };//选出新闻节点并保存到匿名类型query中
                foreach (var item in query)
                {
                    richTextBox.Text +="标题： "+ item.Title.Trim()+"          发布时间："+ item.PubDate.ToString() + "\n" +"内容： "+ item.Description.Trim() + "\n" +"链接： "+ item.Link.Trim() + "\n" + "\n";
                }//遍历输出
            }
            else
            {
                MessageBox.Show("请双击标题对应网址！");
            }
        }

        /// <summary>
        /// 删除节点
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_del_Click(object sender, EventArgs e)
        {
            XDocument xdoc = XDocument.Load("dingyue.xml");//新建一个XDoucument对象将XML读入XML树。
            XElement xRoot = xdoc.Root;

            var qu = from n in xRoot.Descendants("channel")
                     where n.Attribute("name").Value.ToString() == listBox_mynews.SelectedItem.ToString()
                     select n;
            qu.Remove();//选出选中的那个新闻标题并移除
            xdoc.Save("dingyue.xml");
            loadnews();//保存修改并重新加载
        }

        private void richTextBox_LinkClicked(object sender, LinkClickedEventArgs e)
        {
            webBrowser1.Navigate(e.LinkText.ToString());
        }

        private void listBox_mynews_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox_mynews.SelectedItem.ToString().Length < 15)
            {
                btn_del.Enabled = true;
            }
            else
            {
                btn_del.Enabled = false;
            }
        }

        /// <summary>
        /// 添加新节点
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_add_Click(object sender, EventArgs e)
        {
            //定义并从xml文件中加载节点（根节点）
            XElement rootNode = XElement.Load("dingyue.xml");
            //定义一个新节点并赋值
            XElement newNode = new XElement("channel", new XAttribute("name", textBox2.Text),
                                                        new XElement("name", textBox2.Text),
                                                        new XElement("link", textBox1.Text));
            //将此新节点添加到根节点下
            rootNode.Add(newNode);
            //保存对xml的更改操作
            rootNode.Save("dingyue.xml");
            textBox1.Text = textBox2.Text = null;
            loadnews();
        }
    }
}
