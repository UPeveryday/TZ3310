using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using System;
using System.Windows;
namespace dashboard
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            //    log4net.Config.XmlConfigurator.Configure();

            byte[] data = { 0x65, 0x55, 0x22 };
            GetBytestring(data);
        }

        string GetBytestring(byte[] data)
        {
            string ret = string.Empty;
            for (int i = 0; i < data.Length; i++)
            {
                ret += "0X" + Convert.ToString(data[i],16) + "  ";
            }
            return ret;
        }


        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            App.log.Info("sssss");
        }
    }
}
