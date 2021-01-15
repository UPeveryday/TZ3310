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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TestUi
{
    /// <summary>
    /// DashBoard.xaml 的交互逻辑
    /// </summary>
    public partial class DashBoard : UserControl
    {
        public DashBoard()
        {
            InitializeComponent();
            this.DataContext = this;

        }
        private double preValue = 0;
        public bool IsResistance
        {
            get
            {
                var IsRes = (bool)GetValue(IsResistanceProperty);
                return IsRes;
            }
            set
            {
                SetValue(IsResistanceProperty, value);
            }
        }
        public static readonly DependencyProperty IsResistanceProperty =
            DependencyProperty.Register("IsResistance", typeof(Boolean), typeof(DashBoard), new PropertyMetadata(false, null, valueCallback));

        private static object valueCallback(DependencyObject d, object baseValue)
        {
            return (bool)baseValue;
        }




        public Tuple<string, string> tuple
        {
            get { return (Tuple<string, string>)GetValue(tupleProperty); }
            set
            {
                hideTest.Text = value.Item1;
                currentValue.Text = value.Item2;
                SetValue(tupleProperty, value);
            }
        }

        // Using a DependencyProperty as the backing store for tuple.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty tupleProperty =
            DependencyProperty.Register("tuple", typeof(Tuple<string, string>), typeof(DashBoard), new PropertyMetadata(null));




        public int MaxValue
        {
            get { return (int)GetValue(MaxValueProperty); }
            set
            {
                SetValue(MaxValueProperty, value);

            }
        }
        public static readonly DependencyProperty MaxValueProperty =
            DependencyProperty.Register("MaxValue", typeof(int), typeof(DashBoard), new PropertyMetadata(200, maxValueChange, MaxValueCallback));

        private static void maxValueChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var element = (d as DashBoard);
            if (element != null)
            {
                element.MaxValue = (int)e.NewValue;
            }
        }

        private static object MaxValueCallback(DependencyObject d, object baseValue)
        {
            return (int)baseValue;
        }

        public double NextValue
        {
            get
            {
                return (double)GetValue(NextValueProperty);
            }
            set
            {
                Move(preValue, value);
                preValue = value;
                SetValue(NextValueProperty, value);
            }
        }
        public static readonly DependencyProperty NextValueProperty =
            DependencyProperty.Register("NextValue", typeof(double), typeof(DashBoard), new PropertyMetadata(0d, nextValueChange, coerceValueCallback));

        private static void nextValueChange(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            var element = (obj as DashBoard);
            if (element != null)
            {
                element.NextValue = (double)e.NewValue;
            }
        }

        private static object coerceValueCallback(DependencyObject d, object baseValue)
        {
            return (double)baseValue;
        }
        private void Move(double preValue, double NextValue)
        {
            if (!IsResistance)
            {
            maxLocation: if (NextValue <= MaxValue)
                {
                    RotateTransform rt = new RotateTransform();
                    rt.CenterX = 125;
                    rt.CenterY = 125;
                    this.indicatorPin.RenderTransform = rt;
                    double timeAnimation = Math.Abs(preValue - NextValue) * 8;
                    DoubleAnimation da = new DoubleAnimation(preValue * 180 / MaxValue, NextValue * 180 / MaxValue, new Duration(TimeSpan.FromMilliseconds(timeAnimation)));
                    da.AccelerationRatio = 1;
                    rt.BeginAnimation(RotateTransform.AngleProperty, da);
                    // currentValue.Text = string.Format("{0}A", NextValue);
                }
                else
                {
                    preValue = MaxValue;
                    NextValue = MaxValue;
                    goto maxLocation;
                }
            }
            else
            {
                RotateTransform rt = new RotateTransform();
                rt.CenterX = 125;
                rt.CenterY = 125;
                this.indicatorPin.RenderTransform = rt;
                double timeAnimation = Math.Abs(preValue - NextValue) * 8;
                double startAngel = 0d;
                double endAngel = 0d;
                for (int i = 0; i < 11; i++)
                {
                    if (i > 0 && i != 10 && NextValue <= doublePontns[i] && NextValue > doublePontns[i - 1] && NextValue <= 3000)
                    {
                        startAngel = (i - 1) * Math.PI / 10d + Math.PI / 10d * (preValue - doublePontns[i - 1]) / (doublePontns[i] - doublePontns[i - 1]);
                        endAngel = (i - 1) * Math.PI / 10d + Math.PI / 10d * (NextValue - doublePontns[i - 1]) / (doublePontns[i] - doublePontns[i - 1]);
                    }
                    if (i == 10 && NextValue > 3000)
                    {
                        if (preValue > 3000)
                            startAngel = (i - 1) * Math.PI / 10d + Math.PI / 10d * (1d - 3d / Math.Log10(NextValue));
                        else
                        {
                            startAngel = (i - 1) * Math.PI / 10d + Math.PI / 10d * (preValue - doublePontns[i - 1]) / (doublePontns[i] - doublePontns[i - 1]);
                        }
                        endAngel = (i - 1) * Math.PI / 10d + Math.PI / 10d * (1d - 3d / Math.Log10(NextValue));
                    }
                }
                DoubleAnimation da = new DoubleAnimation(startAngel / Math.PI * 180d, endAngel / Math.PI * 180d, new Duration(TimeSpan.FromMilliseconds(timeAnimation)));
                da.AccelerationRatio = 1;
                rt.BeginAnimation(RotateTransform.AngleProperty, da);
            }

        }

        /// <summary>
        /// 画表盘的刻度
        /// </summary>
        private void DrawScale()
        {
            object[] tempdata = new object[this.gaugeCanvas.Children.Count];
            this.gaugeCanvas.Children.CopyTo(tempdata, 0);
            foreach (var item in tempdata)
            {
                if (item is TextBlock && (item as TextBlock).Name != "currentValue" && (item as TextBlock).Name != "hideTest")
                {
                    this.gaugeCanvas.Children.Remove((item as UIElement));
                }
            }
            for (int i = 0; i <= 1800; i += 36)
            {
                //添加刻度线
                Line lineScale = new Line();
                if (i % 180 == 0)
                {
                    lineScale.X1 = 125 - 113 * Math.Cos(i * Math.PI / 1800);
                    lineScale.Y1 = 125 - 113 * Math.Sin(i * Math.PI / 1800);
                    lineScale.Stroke = new SolidColorBrush(Color.FromRgb(0x00, 0xFF, 0));
                    lineScale.StrokeThickness = 3;

                    //添加刻度值
                    TextBlock txtScale = new TextBlock();
                    txtScale.Text = Math.Round((i / 1800d * MaxValue), 1).ToString();
                    txtScale.FontSize = 8;
                    txtScale.Foreground = new SolidColorBrush(Color.FromRgb(0xd9, 0xdF, 0xee));
                    if (i <= 900)//对坐标值进行一定的修正
                    {
                        Canvas.SetLeft(txtScale, 120 - 105 * Math.Cos(i * Math.PI / 1800));
                        Canvas.SetTop(txtScale, 120 - 105 * Math.Sin(i * Math.PI / 1800));
                    }
                    else
                    {
                        var moveLength = MaxValue.ToString().Length;
                        Canvas.SetLeft(txtScale, 120 - (moveLength - 2) * 4 - 100 * Math.Cos(i * Math.PI / 1800));
                        Canvas.SetTop(txtScale, 116 - 100 * Math.Sin(i * Math.PI / 1800));
                    }
                    this.gaugeCanvas.Children.Add(txtScale);
                }
                else
                {
                    if (i % 36 == 0)
                    {
                        lineScale.X1 = 125 - 115 * Math.Cos(i * Math.PI / 1800);
                        lineScale.Y1 = 125 - 115 * Math.Sin(i * Math.PI / 1800);
                        lineScale.Stroke = new SolidColorBrush(Color.FromRgb(0xFF, 0x00, 0));
                        lineScale.StrokeThickness = 1;
                    }
                }
                lineScale.X2 = 125 - 120 * Math.Cos(i * Math.PI / 1800);
                lineScale.Y2 = 125 - 120 * Math.Sin(i * Math.PI / 1800);

                this.gaugeCanvas.Children.Add(lineScale);
            }
        }
        static string[] Pontns = new string[] { "0.1", "0.3", "1", "3", "10", "30", "100", "300", "1000", "3000", "∞" };
        static double[] doublePontns = new double[] { 0.1d, 0.3d, 1d, 3d, 10d, 30d, 100d, 300d, 1000d, 3000d, 0d };

        private void DrawResistanceScale()
        {
            object[] tempdata = new object[this.gaugeCanvas.Children.Count];
            this.gaugeCanvas.Children.CopyTo(tempdata, 0);
            foreach (var item in tempdata)
            {
                if (item is TextBlock && (item as TextBlock).Name != "currentValue" && (item as TextBlock).Name != "hideTest")
                {
                    this.gaugeCanvas.Children.Remove((item as UIElement));
                }
            }
            for (int i = 0; i < 11; i++)
            {
                Line lineScale = new Line();
                lineScale.X1 = 125 - 113 * Math.Cos(Math.PI * (i / 10d));
                lineScale.Y1 = 125 - 113 * Math.Sin(Math.PI * (i / 10d));
                lineScale.Stroke = new SolidColorBrush(Color.FromRgb(0x00, 0xFF, 0));
                lineScale.StrokeThickness = 3;
                lineScale.X2 = 125 - 120 * Math.Cos(Math.PI * (i / 10d));
                lineScale.Y2 = 125 - 120 * Math.Sin(Math.PI * (i / 10d));
                this.gaugeCanvas.Children.Add(lineScale);

                TextBlock txtScale = new TextBlock();
                txtScale.Text = Pontns[i];
                double changeAngel = Pontns[i].Length > 3 ? changeAngel = (Pontns[i].Length - 3) * 8 : changeAngel = 0;
                txtScale.FontSize = 8;
                txtScale.Foreground = new SolidColorBrush(Color.FromRgb(0xd9, 0xdF, 0xee));
                Canvas.SetLeft(txtScale, 120 - changeAngel - 105 * Math.Cos(i * Math.PI / 10d));
                Canvas.SetTop(txtScale, 120 - 105 * Math.Sin(i * Math.PI / 10d));
                this.gaugeCanvas.Children.Add(txtScale);

                for (int j = 1; j < 5; j++)
                {
                    if (i != 10)
                    {
                        Line lineScalesmall = new Line();
                        var agnel = Math.PI * (i / 10d) + Math.PI * (j / 10d / 5d);
                        lineScalesmall.X1 = 125 - 115 * Math.Cos(agnel);
                        lineScalesmall.Y1 = 125 - 115 * Math.Sin(agnel);
                        lineScalesmall.Stroke = new SolidColorBrush(Color.FromRgb(0xFF, 0x00, 0));
                        lineScalesmall.StrokeThickness = 1;
                        lineScalesmall.X2 = 125 - 120 * Math.Cos(agnel);
                        lineScalesmall.Y2 = 125 - 120 * Math.Sin(agnel);
                        this.gaugeCanvas.Children.Add(lineScalesmall);
                    }
                }
            }
        }
        private void lineLocation(double rectan)
        {
            Line lineScalelow = new Line();
            lineScalelow.X1 = 125 - 115 * Math.Cos(rectan);
            lineScalelow.Y1 = 125 - 115 * Math.Sin(rectan);
            lineScalelow.Stroke = new SolidColorBrush(Color.FromRgb(0xFF, 0x00, 0));
            lineScalelow.StrokeThickness = 1;
            lineScalelow.X2 = 125 - 120 * Math.Cos(rectan);
            lineScalelow.Y2 = 125 - 120 * Math.Sin(rectan);
            this.gaugeCanvas.Children.Add(lineScalelow);
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (!IsResistance)
                DrawScale();
            else
                DrawResistanceScale();

            firtsColor.Color =FiristColor.Color;
            secondColor.Color = SecondColor.Color;
            thirdColor.Color = ThirdColor.Color;
        }
    }



    public partial class DashBoard
    {

        public SolidColorBrush FiristColor
        {
            get { return (SolidColorBrush)GetValue(FiristColorProperty); }
            set { SetValue(FiristColorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for FiristColor.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FiristColorProperty =
            DependencyProperty.Register("FiristColor", typeof(SolidColorBrush), typeof(DashBoard), new PropertyMetadata(new SolidColorBrush(Color.FromRgb(0x00, 0xFF, 0x00))));



        public SolidColorBrush SecondColor
        {
            get { return (SolidColorBrush)GetValue(SecondColorProperty); }
            set { SetValue(SecondColorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SecondColor.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SecondColorProperty =
            DependencyProperty.Register("SecondColor", typeof(SolidColorBrush), typeof(DashBoard), new PropertyMetadata(new SolidColorBrush(Color.FromRgb(0x00, 0x00, 0xFF))));



        public SolidColorBrush ThirdColor
        {
            get { return (SolidColorBrush)GetValue(ThirdColorProperty); }
            set { SetValue(ThirdColorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ThirdColor.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ThirdColorProperty =
            DependencyProperty.Register("ThirdColor", typeof(SolidColorBrush), typeof(DashBoard), new PropertyMetadata(new SolidColorBrush(Color.FromRgb(0xFF, 0x00, 0x00))));


    }
}
