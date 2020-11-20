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
    /// 
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

        private Tuple<int, int> CutWave(GearedValues<ObservablePoint> wave, int cutHeaderLever = 400)
        {
            double needSelectData = Math.Round((wave.Select(x => x.Y).Max() + wave.Select(x => x.Y).Min()) / 2, 3);
            ObservablePoint[] twopoint = new ObservablePoint[2];
            double[] tempD = wave.Select(x => Math.Round(x.Y, 3)).ToArray();
            double Xmin = 0.00;
            double Xmax = 300.00;
            for (int i = 1; i < 6000; i++)
            {
                if ((tempD[i - 1] >= needSelectData && tempD[i] <= needSelectData) || (tempD[i] >= needSelectData && tempD[i - 1] <= needSelectData))
                {
                    twopoint[0] = wave[i];
                    Xmin = wave[i].X;
                    break;
                }
            }
            for (int i = 5999; i > 1; i--)
            {
                if ((tempD[i - 1] >= needSelectData && tempD[i] <= needSelectData) || (tempD[i] >= needSelectData && tempD[i - 1] <= needSelectData))
                {
                    Xmax= wave[i].X;
                    break;
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

            if (Xmax > 280)
            {
                take = (int)((300 - Xmin) * 20);
            }
            else
            {
                take = (int)((Xmax - Xmin) * 20 + cutHeaderLever);
            }

            return new Tuple<int, int>(skip, take);
        }

        private static object coreValueCallback(DependencyObject d, object baseValue)
        {
            return (short[])baseValue;
        }

        private WaveResult[] waveResults;

        public WaveResult[] WaveResults
        {
            get
            {
                waveResults = new WaveResult[3];
                waveResults[0] = new WaveResult { t1 = wave1.t1Ret, t2 = wave1.t2Ret, t3 = wave1.t3Ret, t4 = wave1.t4Ret, r1 = wave1.R1Ret, r2 = wave1.R2Ret, r12 = wave1.R1AndR2Ret };
                waveResults[1] = new WaveResult { t1 = wave2.t1Ret, t2 = wave2.t2Ret, t3 = wave2.t3Ret, t4 = wave2.t4Ret, r1 = wave2.R1Ret, r2 = wave2.R2Ret, r12 = wave2.R1AndR2Ret };
                waveResults[2] = new WaveResult { t1 = wave3.t1Ret, t2 = wave3.t2Ret, t3 = wave3.t3Ret, t4 = wave3.t4Ret, r1 = wave3.R1Ret, r2 = wave3.R2Ret, r12 = wave3.R1AndR2Ret };
                return waveResults;
            }
            private set { waveResults = value; }
        }


    }



    public class WaveResult
    {
        public string t1;
        public string t2;
        public string t3;
        public string t4;

        public string r1;
        public string r2;
        public string r12;
    }
}
