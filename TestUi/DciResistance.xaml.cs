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
using SCEEC.Numerics;

namespace TestUi
{
    /// <summary>
    /// DciResistance.xaml 的交互逻辑
    /// </summary>
    public partial class DciResistance : UserControl
    {
        public DciResistance()
        {
            InitializeComponent();
            this.DataContext = this;
        }
        public Tuple<string, string> location { get; set; } = new Tuple<string, string>("电压(kV)", "电阻");


        //if (testResult.values[0] != null && testResult.values[0].value != null)
        //{
        //    dciboard_volate.Value = (double) (testResult.values[0].value / 1000);
        //    dciboard_volate_value.Text = testResult.values[0].OriginText;
        //}
        //if (testResult.values[1] != null && testResult.values[1].value != null)
        //{
        //    dciboard_resistance.Value = (double) (testResult.values[1].value / 1000000000);
        //    dciboard_resistance_value.Text = testResult.values[1].OriginText + "Ω";
        //}

        public PhysicalVariable[] SignalValue
        {
            get { return (PhysicalVariable[])GetValue(SignalValueProperty); }
            set
            {
                if (value != null && value[0] != null && value[0].value != null)
                {
                    Current.NextValue = (double)(value[0].value / 1000);
                    Current.tuple = new Tuple<string, string>(location.Item1, value[0].OriginText);
                }
                if (value != null && value[1] != null && value[1].value != null)
                {
                    Resistance.NextValue = (double)(value[1].value / 1000000000);
                    Resistance.tuple = new Tuple<string, string>(location.Item2, value[1].OriginText + "Ω");
                }
                SetValue(SignalValueProperty, value);
            }
        }

        // Using a DependencyProperty as the backing store for ThreeResistanceValue.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SignalValueProperty =
            DependencyProperty.Register("SignalValue", typeof(PhysicalVariable[]), typeof(DciResistance), new PropertyMetadata(null, null, corevalueCallBack));

        private static object corevalueCallBack(DependencyObject d, object baseValue)
        {
            return (PhysicalVariable[])baseValue;
        }
    }
}
