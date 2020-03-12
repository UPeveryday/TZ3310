using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace SCEEC.TTM
{
    public class TextBoxNumber : TextBox
    {
        ///// <summary>
        ///// 获取或设置最大值
        ///// </summary>
        //public bool IsOnNumKB
        //{
        //    get;
        //    set;
        //}
        /// <summary>
        /// 获取或设置最大值
        /// </summary>
        public double MaxValue
        {
            get { return (double)GetValue(MaxValueProperty); }
            set { SetValue(MaxValueProperty, value); }
        }

        /// <summary>
        /// 获取或设置最小值
        /// </summary>
        public double MinValue
        {
            get { return (double)GetValue(MinValueProperty); }
            set { SetValue(MinValueProperty, value); }
        }
        /// <summary>
        /// 超值
        /// </summary>
        public bool IsErrData
        {
            get { return (bool)GetValue(IsErrDataProperty); }
            set { SetValue(IsErrDataProperty, value); }
        }
        /// <summary>
        /// 最大值属性
        /// </summary>
        public static readonly DependencyProperty MaxValueProperty = DependencyProperty.Register("MaxValue", typeof(double),
            typeof(TextBoxNumber), new PropertyMetadata(double.MaxValue));

        /// <summary>
        /// 最小值属性
        /// </summary>
        public static readonly DependencyProperty MinValueProperty = DependencyProperty.Register("MinValue", typeof(double),
            typeof(TextBoxNumber), new PropertyMetadata(double.MinValue));

        /// <summary>
        /// 最小值属性
        /// </summary>
        public static readonly DependencyProperty IsErrDataProperty = DependencyProperty.Register("IsErrData", typeof(bool),
            typeof(TextBoxNumber), new PropertyMetadata(false));

        ToolTip toolTip = new ToolTip();
        TextBlock tbMessage = new TextBlock();
        ToolTip NewToolTip()
        {
            toolTip = new ToolTip();
            StackPanel sp = new StackPanel();
            TextBlock tb = new TextBlock();
            tb.FontWeight = FontWeights.Heavy;
            tb.Text = "提示输入";
            sp.Children.Add(tb);
            tbMessage = new TextBlock();
            tbMessage.Foreground = Brushes.Red;
            sp.Children.Add(tbMessage);
            toolTip.Content = sp;
            return toolTip;
        }
        void ShowToolTip(string msg)
        {
            tbMessage.Text = msg;
            toolTip.IsOpen = true;
            this.ToolTip = toolTip;
        }
        private void tb_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            try
            {
                this.ToolTip = null;
                TextBoxNumber tb = sender as TextBoxNumber;
                bool bInput = Regex.IsMatch(e.Text, @"[0-9.+-]");
                if (!bInput)
                {
                    e.Handled = true;
                    ShowToolTip($"输入的当前值{e.Text}格式不正确！");
                }
                else
                {
                    string value = tb.Text + e.Text;

                    if (tb.MinValue >= 0 && e.Text == "-")
                    {
                        e.Handled = true;
                        ShowToolTip($"输入的当前值{value}不能小于最小值{tb.MinValue}");
                    }
                    if (Convert.ToDouble(value) > tb.MaxValue)
                    {
                        e.Handled = true;
                        ShowToolTip($"输入的当前值{value}不能大于最大值{tb.MaxValue}");
                    }
                    //if (Convert.ToDouble(value) < tb.MinValue)
                    //{
                    //    e.Handled = true;
                    //}
                }
            }
            catch { e.Handled = true; }
        }
        public TextBoxNumber()
        {
            this.PreviewTextInput += tb_PreviewTextInput;
            this.LostFocus += TextBoxNumber_LostFocus;
            this.GotFocus += TextBoxNumber_GotFocus;
            this.PreviewMouseDown += TextBoxNumber_PreviewMouseDown;
            toolTip.Closed += ToolTip_Closed;
            toolTip = NewToolTip();
        }

        private void TextBoxNumber_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            this.Focus();
            e.Handled = true;
        }

        private void TextBoxNumber_GotFocus(object sender, RoutedEventArgs e)
        {
            this.SelectAll();
        }

        private void ToolTip_Closed(object sender, RoutedEventArgs e)
        {
            toolTip.IsOpen = false;
            this.ToolTip = null;
        }

        private void TextBoxNumber_LostFocus(object sender, RoutedEventArgs e)
        {

            TextBoxNumber tb = sender as TextBoxNumber;
            string value = tb.Text;
            toolTip.IsOpen = false;
            this.ToolTip = null;
            try
            {

                if (Convert.ToDouble(value) < tb.MinValue)
                {
                    tb.Text = tb.MinValue.ToString();
                    ShowToolTip($"输入的当前值{value}不能大于最大值{tb.MaxValue}");
                }
                if (Convert.ToDouble(value) > tb.MaxValue)
                {
                    tb.Text = tb.MaxValue.ToString();
                    ShowToolTip($"输入的当前值{value}不能大于最大值{tb.MaxValue}");
                }
            }
            catch
            { tb.Text = ""; }
            //if (!isValue)
            //{
            //    MessageBox.Show(tb.Tag + "数据不正确！" , "数据配置提示", MessageBoxButton.OK, MessageBoxImage.Error);
            //    //tb.Focus();
            //    //tb.SelectionStart = tb.Text.Length;
            //} 
        }

        /// <summary>
        /// 静态构造函数
        /// </summary>
        static TextBoxNumber()
        {

            FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(TextBoxNumber), new FrameworkPropertyMetadata(typeof(TextBoxNumber)));
        }
    }
}
