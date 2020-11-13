using LiveCharts.Defaults;
using LiveCharts.Geared;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChartsWave
{
  public static  class ParsingData
    {
        public static GearedValues<ObservablePoint>[] deelwaves(short[] waves, int current = 1)
        {
            GearedValues<ObservablePoint>[] ret = new GearedValues<ObservablePoint>[4];
            for (int i = 0; i < 4; i++)
            {
                ret[i] = new GearedValues<ObservablePoint>();
            }
            for (int i = 0; i < 4; i++)
            {
                int start = 0; int end = 6000;
                if (i == 0)
                {
                    start = 0;
                    end = 6000;
                }
                if (i == 1)
                {
                    start = 6002;
                    end = 12002;
                }
                if (i == 2)
                {
                    start = 12004;
                    end = 18004;
                }
                if (i == 3)
                {
                    start = 18006;
                    end = 24006;
                }

                for (int s = start; s < end; s++)
                {
                    double pdata = 0;
                    if (waves[6000].ToString() == "1")
                        pdata = waves[s] * 500d / 32768d / 1000 / current;
                    if (waves[6000].ToString() == "2")
                        pdata = waves[s] * 1000d / 32768d / 1000 / current;
                    if (waves[6000].ToString() == "3")
                        pdata = waves[s] * 5000d / 32768d / 1000 / current;
                    if (waves[6000].ToString() == "4")
                        pdata = waves[s] * 10000d / 32768d / 1000 / current;
                    if (waves[6000].ToString() == "5")
                        pdata = waves[s] * 50000d / 32768d / 1000 / current;
                    else
                    {
                        pdata = waves[s] * 5000d / 32768d / 1000 / current;
                    }
                    ret[i].Add(new ObservablePoint { X = (s - 6002 * i + 1) * 0.05, Y = pdata });
                }

            }
            return ret;
        }

        public static short[] getWaveData(string path = "C:\\wave\\2.txt")
        {
            var data = File.ReadAllText(path).Trim().Replace("\n", "").Replace("\r", "").Replace(" ", "");
            var by = data.Split(']', '[');
            List<short> ret = new List<short>();
            for (int i = 0; i < by.Length; i++)
            {
                if (i != 0 && i % 2 == 0)
                {
                    ret.Add((short)(Convert.ToInt32(by[i])));
                }
            }
            return ret.ToArray();
        }
    }
}
