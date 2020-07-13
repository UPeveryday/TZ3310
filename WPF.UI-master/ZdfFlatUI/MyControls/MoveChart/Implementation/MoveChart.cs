using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ZdfFlatUI.MyControls.MoveChart.Implementation
{
    public class MoveChart : TextBox
    {
        static MoveChart()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MoveChart), new System.Windows.FrameworkPropertyMetadata(typeof(MoveChart)));
        }
    }
}
