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
    /// SignalResistance.xaml 的交互逻辑
    /// </summary>
    public partial class SignalResistance : UserControl
    {
        public SignalResistance()
        {
            InitializeComponent();
            this.DataContext = this;
        }
        public Tuple<string, string> location { get; set; } = new Tuple<string, string>("A相电流", "A相电阻");

        public PhysicalVariable[] SignalValue
        {
            get { return (PhysicalVariable[])GetValue(SignalValueProperty); }
            set
            {
                if (value != null && value[0] != null && value[0].value != null)
                {
                    Current.NextValue = (double)value[0].value;
                    Current.tuple = new Tuple<string, string>(location.Item1, value[0].OriginText);
                }
                if (value != null && value[1] != null && value[1].value != null)
                {
                    Resistance.NextValue = (double)value[1].value * 1000;
                    Resistance.tuple = new Tuple<string, string>(location.Item2, value[1].OriginText);
                }
                SetValue(SignalValueProperty, value);
            }
        }

        // Using a DependencyProperty as the backing store for ThreeResistanceValue.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SignalValueProperty =
            DependencyProperty.Register("SignalValue", typeof(PhysicalVariable[]), typeof(SignalResistance), new PropertyMetadata(null, null, corevalueCallBack));

        private static object corevalueCallBack(DependencyObject d, object baseValue)
        {
            return (PhysicalVariable[])baseValue;
        }
    }
}
