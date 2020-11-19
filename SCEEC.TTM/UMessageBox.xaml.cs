using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SCEEC.TTM
{
    /// <summary>
    /// UMessageBox.xaml 的交互逻辑
    /// </summary>
    public partial class UMessageBox : Window
    {
        private UMessageBox()
        {
            InitializeComponent();
        }
        public new string Title
        {
            get { return this.lblTitle.Text; }
            set { this.lblTitle.Text = value; }
        }

        public string Message
        {
            get { return this.lblMsg.Text; }
            set { this.lblMsg.Text = value; }
        }

        public string ConfireMessage
        {
            get { return this.confireButton.Text; }
            set { this.confireButton.Text = value; }
        }

        public string FirstButtonText
        {
            get { return this.firstButton.Text; }
            set { this.firstButton.Text = value; }
        }

        public bool FirstVisible
        {
            get
            {
                if (this.border2.Visibility == Visibility.Visible)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            set
            {
                if (value)
                {
                    this.border2.Visibility = Visibility.Visible;
                }
                else
                {
                    this.border2.Visibility = Visibility.Hidden;

                }
            }
        }

        /// <summary>
        /// 静态方法 模拟MESSAGEBOX.Show方法
        /// </summary>
        /// <param name="title">标题</param>
        /// <param name="msg">消息</param>
        /// <param name="confireMessage">右边的按钮的文字</param>
        /// <param name="firstButtonVisible">左边按钮是否显示</param>
        /// <returns></returns>
        public static bool? Show(string title, string msg, string confireMessage = "跳过", bool firstButtonVisible = true,string firstButtonMessage="重做")
        {
            var msgBox = new UMessageBox();
            msgBox.Title = title;
            msgBox.Message = msg;
            msgBox.ConfireMessage = confireMessage;
            msgBox.FirstVisible = firstButtonVisible;
            msgBox.FirstButtonText = firstButtonMessage;
            return msgBox.ShowDialog();
        }

        private void Yes_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }


        private void No_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DialogResult = true;
            this.Close();
        }
    }
}
