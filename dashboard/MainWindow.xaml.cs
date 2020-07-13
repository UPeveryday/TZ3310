using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

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
            this.DataContext = this;

            InitCreateChart();



        }

        public Func<double, string> XFormatter { get; set; }
        public Func<double, string> YFormatter { get; set; }
        public SeriesCollection LcCurrentVolate { get; set; }
        public ChartValues<ObservablePoint> TanEleVolatevalue { get; set; } = new ChartValues<ObservablePoint>();
        public void InitCreateChart()
        {
            LineSeries t1 = new LineSeries
            {
                StrokeThickness = 2,
                Stroke = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(28, 142, 196)),
                Fill = System.Windows.Media.Brushes.Transparent,
                LineSmoothness = 10,//0为折现样式
                PointGeometrySize = 0,
                PointForeground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(34, 46, 49)),
                Values = TanEleVolatevalue
            };
            LcCurrentVolate = new SeriesCollection { };
            LcCurrentVolate.Add(t1);
            XFormatter = val => (val).ToString("N3") + "A";
            YFormatter = val => (val).ToString("N3") + " V";
        }

        private async void button_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < 60; i++)
            {

                if (i < 12)
                {
                    TanEleVolatevalue.Add(new ObservablePoint(i, 1000));
                }
                if (i >= 12 && i < 24)
                    TanEleVolatevalue.Add(new ObservablePoint(i, 100));
                if (i >= 24 && i < 36)
                    TanEleVolatevalue.Add(new ObservablePoint(i, 1000));
                if (i >= 36 && i < 48)
                    TanEleVolatevalue.Add(new ObservablePoint(i, 100));
                if (i >= 48)
                    TanEleVolatevalue.Add(new ObservablePoint(i, 1000));

            }
        }

        private void control5_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            //var he = control5.ActualHeight;
            //var we = control5.ActualWidth;
        }
    }
}
