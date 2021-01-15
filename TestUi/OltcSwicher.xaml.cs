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
    /// OltcSwicher.xaml 的交互逻辑
    /// </summary>
    public partial class OltcSwicher : UserControl
    {
        public OltcSwicher()
        {
            InitializeComponent();
            this.DataContext = this;
        }

        public PhysicalVariable[] ThreeResistanceValue
        {
            get { return (PhysicalVariable[])GetValue(ThreeResistanceValueProperty); }
            set
            {
                if (value[1] != null && value[1].value != null)
                {
                    AOltc.NextValue = (double)value[1].value;
                    AOltc.tuple = new Tuple<string, string>("A相电流", value[1].OriginText);
                }
                if (value[4] != null && value[4].value != null)
                {
                    BOltc.NextValue = (double)value[4].value;
                    BOltc.tuple = new Tuple<string, string>("B相电流", value[4].OriginText);
                }
                if (value[7] != null && value[7].value != null)
                {
                    COltc.NextValue = (double)value[7].value;
                    COltc.tuple = new Tuple<string, string>("C相电流", value[7].OriginText);
                }
                SetValue(ThreeResistanceValueProperty, value);
            }
        }
        // Using a DependencyProperty as the backing store for ThreeResistanceValue.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ThreeResistanceValueProperty =
            DependencyProperty.Register("ThreeResistanceValue", typeof(PhysicalVariable[]), typeof(OltcSwicher),
                new PropertyMetadata(null, null, corevalueCallBack));

        private static object corevalueCallBack(DependencyObject d, object baseValue)
        {
            return (PhysicalVariable[])baseValue;
        }
    }



}
