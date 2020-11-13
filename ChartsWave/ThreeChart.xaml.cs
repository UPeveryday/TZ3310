using LiveCharts.Defaults;
using LiveCharts.Geared;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ChartsWave
{
    /// <summary>
    /// ThreeChart.xaml 的交互逻辑
    /// </summary>
    public partial class ThreeChart : UserControl
    {
        public ThreeChart()
        {
            InitializeComponent();
        }

        public short[] shortWave
        {
            get
            {
                return (short[])GetValue(shortWaveProperty); ;
            }
            set
            {
                var pd = ParsingData.deelwaves(value);
                List<Tuple<int, int>> tuples = new List<Tuple<int, int>>();
                tuples.Add(CutWave(pd[0]));
                tuples.Add(CutWave(pd[1]));
                tuples.Add(CutWave(pd[2]));
                var skip = tuples.Select(x => x.Item1).Min();
                var take = tuples.Select(x => x.Item2).Max();

                wave1.cutTake = take;
                wave1.cutSkip = skip;
                wave2.cutTake = take;
                wave2.cutSkip = skip;
                wave3.cutTake = take;
                wave3.cutSkip = skip;

                wave1.shortWave = pd[0];
                wave2.shortWave = pd[1];
                wave3.shortWave = pd[2];
                SetValue(shortWaveProperty, value);
            }
        }

        public static readonly DependencyProperty shortWaveProperty =
            DependencyProperty.Register("shortWave", typeof(short[]), typeof(ThreeChart), new PropertyMetadata(null, null, coreValueCallback));




        private Tuple<int,int> CutWave(GearedValues<ObservablePoint> wave, int cutHeaderLever = 400)
        {
            double needSelectData = (wave.Select(x => x.Y).Max() + wave.Select(x => x.Y).Min()) / 2;
            var points = wave.Where(x => x.Y >= needSelectData * 0.95 && x.Y <= needSelectData * 1.05);
            var temp = points.ToArray();
            double Xmin = 0.00;
            double Ymin = 300.00;
            for (int i = 1; i < points.Count(); i++)
            {
                if (temp[i].X > temp[i - 1].X * 1.5 || temp[i].X < temp[i - 1].X * 0.5)
                {
                    if (points.ToArray()[i - 1].X > points.ToArray()[i].X)
                    {
                        Xmin = points.ToArray()[i].X;
                        Ymin = Xmin = points.ToArray()[i - 1].X;
                    }
                    else
                    {
                        Ymin = points.ToArray()[i].X;
                        Xmin = points.ToArray()[i - 1].X;
                    }
                }
            }
            int skip = 0;
            int take = 0;
            if (Xmin < 20)
            {
                skip = 0;
            }
            else
            {
                skip = (int)(Xmin * 20 - cutHeaderLever);
            }

            if (Ymin > 280)
            {
                take = (int)((300 - Xmin) * 20);
            }
            else
            {
                take = (int)((Ymin - Xmin) * 20 + cutHeaderLever);
            }
        
            return new Tuple<int, int>(skip,take) ;
        }

        private static object coreValueCallback(DependencyObject d, object baseValue)
        {
            return (short[])baseValue;
        }
    }
}
