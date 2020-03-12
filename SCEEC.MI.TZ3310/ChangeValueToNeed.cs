using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCEEC.MI.TZ3310
{
    public static class ChangeValueToNeed
    {
        public static string DcResistans_To_75(Numerics.PhysicalVariable Value)
        {
            if (Value.value != null)
            {
                double TempValue = Convert.ToDouble(Value.value * 310 / 255);
                return Numerics.NumericsConverter.Value2Text(TempValue, 2, -6, " ", "Ω", false, false);
            }
            return string.Empty;

        }

        public static string UnBalance(double a, double b, double c)
        {
            return ((((b - c) / ((a + b + c) / 3))) * 100).ToString("F2") + "%";
        }

        public static string MutualDifference(double a, double b)
        {
            if (0 == Math.Min(a, b))
            {
                return "0.00%";
            }
            else
            return (((a - b) / Math.Min(a, b)) * 100.0).ToString("F2") + "%";
        }

        public static string MaxMutualDifference(double a, double b, double c)
        {
            double md = (a - b) / Math.Min(a, b);
            md = Math.Max(md, (b - c) / Math.Min(b, c));
            md = Math.Max(md, (c - a) / Math.Min(c, a));
            return (md * 100).ToString("F2") + "%";
        }

        public static Parameter.yzfjTap GetOLtcNum(MeasurementItemStruct mi)
        {
            switch (mi.TapLabel[0])
            {
                case "1":
                    if (mi.TapLabel[1] == "2")
                        return Parameter.yzfjTap._1To_2;
                    else
                        return Parameter.yzfjTap._1To_2;
                case "2":
                    if (mi.TapLabel[1] == "3")
                        return Parameter.yzfjTap._2To_3;
                    else
                        return Parameter.yzfjTap._2To_1;
                case "3":
                    if (mi.TapLabel[1] == "4")
                        return Parameter.yzfjTap._3To_4;
                    else
                        return Parameter.yzfjTap._3To_2;
                case "4":
                    if (mi.TapLabel[1] == "5")
                        return Parameter.yzfjTap._4To_5;
                    else
                        return Parameter.yzfjTap._4To_3;
                case "5":
                    if (mi.TapLabel[1] == "6")
                        return Parameter.yzfjTap._5To_6;
                    else
                        return Parameter.yzfjTap._5To_4;
                case "6":
                    if (mi.TapLabel[1] == "7")
                        return Parameter.yzfjTap._6To_7;
                    else
                        return Parameter.yzfjTap._6To_5;
                case "7":
                    if (mi.TapLabel[1] == "8")
                        return Parameter.yzfjTap._7To_8;
                    else
                        return Parameter.yzfjTap._7To_6;
                case "8":
                    if (mi.TapLabel[1] == "9")
                        return Parameter.yzfjTap._8To_9;
                    else
                        return Parameter.yzfjTap._8To_7;
                case "9":
                    if (mi.TapLabel[1] == "10")
                        return Parameter.yzfjTap._9To_10;
                    else
                        return Parameter.yzfjTap._9To_8;
                case "10":
                    if (mi.TapLabel[1] == "11")
                        return Parameter.yzfjTap._10To_11;
                    else
                        return Parameter.yzfjTap._10To_9;
                case "11":
                    if (mi.TapLabel[1] == "12")
                        return Parameter.yzfjTap._11To_12;
                    else
                        return Parameter.yzfjTap._11To_10;
                case "12":
                    if (mi.TapLabel[1] == "13")
                        return Parameter.yzfjTap._12To_13;
                    else
                        return Parameter.yzfjTap._12To_11;
                case "13":
                    if (mi.TapLabel[1] == "14")
                        return Parameter.yzfjTap._13To_14;
                    else
                        return Parameter.yzfjTap._13To_12;
                case "14":
                    if (mi.TapLabel[1] == "15")
                        return Parameter.yzfjTap._14To_15;
                    else
                        return Parameter.yzfjTap._14To_13;
                case "15":
                    if (mi.TapLabel[1] == "16")
                        return Parameter.yzfjTap._15To_16;
                    else
                        return Parameter.yzfjTap._15To_14;
                case "16":
                    if (mi.TapLabel[1] == "17")
                        return Parameter.yzfjTap._16To_17;
                    else
                        return Parameter.yzfjTap._16To_15;
                case "17":
                    if (mi.TapLabel[1] == "18")
                        return Parameter.yzfjTap._17To_18;
                    else
                        return Parameter.yzfjTap._17To_16;
                case "18":
                    if (mi.TapLabel[1] == "19")
                        return Parameter.yzfjTap._18To_19;
                    else
                        return Parameter.yzfjTap._18To_17;
                case "19":
                    if (mi.TapLabel[1] == "20")
                        return Parameter.yzfjTap._19To_20;
                    else
                        return Parameter.yzfjTap._19To_20;
                default:
                    return Parameter.yzfjTap._1To_2;

            }
        }

    }
}
