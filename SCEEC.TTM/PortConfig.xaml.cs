using SCEEC.MI.TZ3310;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
    /// PortConfig.xaml 的交互逻辑
    /// </summary>
    public partial class PortConfig : Window
    {
        public PortConfig()
        {
            InitializeComponent();
            Loadbase();
        }


        private void Loadbase()
        {
            WorkingSets.local.Tz3310 = new ClassTz3310();
            portname.ItemsSource = WorkingSets.local.Tz3310.GetPortNames();
            if (WorkingSets.local.Tz3310.GetPortNames().Length > 0)
                portname.SelectedIndex = 0;
            baudRate.ItemsSource = new List<int> { 600, 1200, 1800, 2400, 4800, 7200, 9600, 115200, 14400, 19200, 28800 };
            baudRate.SelectedIndex = 7;

            databits.ItemsSource = new List<int> { 5, 6, 7, 8 };
            databits.SelectedIndex = 3;

            stopbits.ItemsSource = new List<int> { 1, 2, 3, 4 };
            stopbits.SelectedIndex = 0;

        }

        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void MinimumButton_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void MaximumButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.WindowState == WindowState.Maximized)
            {
                this.WindowState = WindowState.Normal;
                maximumButtonImage.Source = new BitmapImage(new Uri("Resources/maximum.png", UriKind.Relative));
            }
            else
            {
                this.WindowState = WindowState.Maximized;
                maximumButtonImage.Source = new BitmapImage(new Uri("Resources/maximum2.png", UriKind.Relative));
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();

        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            string p = (string)portname.SelectedValue;
            int b = (int)baudRate.SelectedItem;
            int d = (int)databits.SelectedItem;
            int s = (int)stopbits.SelectedItem;
            Task.Run(() =>
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    OKButton.IsEnabled = false;
                    OKButton.Opacity = 0.2;
                    CancelButton.Opacity = 0.2;
                    CancelButton.IsEnabled = false;
                    OKButton.Content = "正在联机中...";
                });
                if (true == WorkingSets.local.Tz3310.OpenPort(p, b, d, s))
                {
                    if (WorkingSets.local.Tz3310.CommunicationQuery(1))
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            this.Close();
                        });
                    else
                        MessageBox.Show("仪器通讯失败");
                }
                else
                    MessageBox.Show("打开串口失败");
                Application.Current.Dispatcher.Invoke(() =>
                {
                    OKButton.IsEnabled = true;
                    CancelButton.IsEnabled = true;
                    OKButton.Opacity = 1;
                    CancelButton.Opacity = 1;
                    OKButton.Content = "重新连接";

                });
            });
        }
    }
}
