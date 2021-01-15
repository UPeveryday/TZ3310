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
    /// ThreeResistance.xaml 的交互逻辑
    /// </summary>
    public partial class ThreeResistance : UserControl
    {
        public ThreeResistance()
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
                    ACurrent.NextValue = (double)value[1].value;
                    ACurrent.tuple = new Tuple<string, string>("A相电流", value[1].OriginText);
                }
                if (value[4] != null && value[4].value != null)
                {
                    BCurrent.NextValue = (double)value[4].value;
                    BCurrent.tuple = new Tuple<string, string>("B相电流", value[4].OriginText);
                }
                if (value[7] != null && value[7].value != null)
                {
                    CCurrent.NextValue = (double)value[7].value;
                    CCurrent.tuple = new Tuple<string, string>("C相电流", value[7].OriginText);
                }
                if (value[2] != null && value[2].value != null)
                {
                    AResistance.NextValue =(double) value[2].value * 1000;
                    AResistance.tuple = new Tuple<string, string>("A相电阻", value[2].OriginText);
                }
                if (value[5] != null && value[5].value != null)
                {
                    BResistance.NextValue = (double)value[5].value * 1000;
                    BResistance.tuple = new Tuple<string, string>("B相电阻", value[5].OriginText);
                }
                if (value[8] != null && value[8].value != null)
                {
                    CResistance.NextValue = (double)value[8].value * 1000;
                    CResistance.tuple = new Tuple<string, string>("C相电阻", value[8].OriginText);
                }
                SetValue(ThreeResistanceValueProperty, value);
            }
        }

        // Using a DependencyProperty as the backing store for ThreeResistanceValue.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ThreeResistanceValueProperty =
            DependencyProperty.Register("ThreeResistanceValue", typeof(PhysicalVariable[]), typeof(ThreeResistance),
                new PropertyMetadata(null, null, corevalueCallBack));

        private static object corevalueCallBack(DependencyObject d, object baseValue)
        {
            return (PhysicalVariable[])baseValue;
        }
    }



}
