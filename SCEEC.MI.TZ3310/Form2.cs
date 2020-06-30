using SCEEC.MI.TZ3310;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.Windows.Media.Imaging;

namespace SCEEC.TTM
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
            chart1.ChartAreas[0].CursorX.IsUserEnabled = false;
            chart1.ChartAreas[0].CursorY.IsUserEnabled = false;
        }
        short[] Tempdata = new short[24008];
        public int Testcode { get; set; }
        public string OLTCLABEL { get; set; }
        public Form2(short[] Tdata, string oltclable, int testcode)
        {
            InitializeComponent();
            Testcode = testcode;
            OLTCLABEL = oltclable;
            HideLabel(oltclable);
            Tempdata = Tdata;
        }

        public int[] OltcNum { get; set; }
        private void HideLabel(string oltclable)
        {
            OltcNum = new int[2];
            if (oltclable == "12")
            {
                label8.Text = "1-->2";
                OltcNum[0] = 1;
                OltcNum[1] = 2;
            }
            if (oltclable == "21")
            {
                label8.Text = "2-->1";
                OltcNum[0] = 2;
                OltcNum[1] = 1;
            }

            if (oltclable == "23")
            {
                OltcNum[0] = 2;
                OltcNum[1] = 3;
                label8.Text = "2-->3";
            }
            if (oltclable == "32")
            {
                label8.Text = "3-->2";
                OltcNum[0] = 3;
                OltcNum[1] = 2;
            }
            if (oltclable == "34")
            {
                label8.Text = "3-->4";
                OltcNum[0] = 3;
                OltcNum[1] = 4;
            }
            if (oltclable == "43")
            {
                label8.Text = "4-->3";
                OltcNum[0] = 4;
                OltcNum[1] = 3;
            }
            if (oltclable == "45")
            {
                label8.Text = "4-->5";
                OltcNum[0] = 4;
                OltcNum[1] = 5;
            }
            if (oltclable == "54")
            {
                label8.Text = "5-->4";
                OltcNum[0] = 5;
                OltcNum[1] = 4;
            }
            if (oltclable == "56")
            {
                label8.Text = "5-->6";
                OltcNum[0] = 5;
                OltcNum[1] = 6;
            }
            if (oltclable == "65")
            {
                label8.Text = "6-->5";
                OltcNum[0] = 6;
                OltcNum[1] = 5;
            }
            if (oltclable == "67")
            {
                label8.Text = "6-->7";
                OltcNum[0] = 6;
                OltcNum[1] = 7;
            }
            if (oltclable == "76")
            {
                label8.Text = "7-->6";
                OltcNum[0] = 7;
                OltcNum[1] = 6;
            }
            if (oltclable == "78")
            {
                label8.Text = "7-->8";
                OltcNum[0] = 7;
                OltcNum[1] = 8;
            }
            if (oltclable == "87")
            {
                label8.Text = "8-->7";
                OltcNum[0] = 8;
                OltcNum[1] = 7;
            }
            if (oltclable == "89")
            {
                label8.Text = "8-->9";
                OltcNum[0] = 8;
                OltcNum[1] = 9;
            }
            if (oltclable == "98")
            {
                label8.Text = "9-->8";
                OltcNum[0] = 9;
                OltcNum[1] = 8;
            }
            if (oltclable == "910")
            {
                label8.Text = "9-->10";
                OltcNum[0] = 9;
                OltcNum[1] = 10;
            }
            if (oltclable == "109")
            {
                label8.Text = "10-->9";
                OltcNum[0] = 10;
                OltcNum[1] = 9;
            }
            if (oltclable == "1011")
            {
                label8.Text = "10-->11";
                OltcNum[0] = 10;
                OltcNum[1] = 11;
            }
            if (oltclable == "1110")
            {
                label8.Text = "11-->10";
                OltcNum[0] = 11;
                OltcNum[1] = 10;
            }
            if (oltclable == "1112")
            {
                label8.Text = "11-->12";
                OltcNum[0] = 11;
                OltcNum[1] = 12;
            }
            if (oltclable == "1211")
            {
                label8.Text = "12-->11";
                OltcNum[0] = 12;
                OltcNum[1] = 11;
            }
            if (oltclable == "1213")
            {
                label8.Text = "12-->13";
                OltcNum[0] = 12;
                OltcNum[1] = 13;
            }
            if (oltclable == "1312")
            {
                label8.Text = "13-->12";
                OltcNum[0] = 13;
                OltcNum[1] = 12;
            }
            if (oltclable == "1314")
            {
                label8.Text = "13-->14";
                OltcNum[0] = 13;
                OltcNum[1] = 14;
            }
            if (oltclable == "1413")
            {
                label8.Text = "14-->13";
                OltcNum[0] = 14;
                OltcNum[1] = 13;
            }
            if (oltclable == "1415")
            {
                label8.Text = "14-->15";
                OltcNum[0] = 14;
                OltcNum[1] = 15;
            }
            if (oltclable == "1514")
            {
                label8.Text = "15-->14";
                OltcNum[0] = 15;
                OltcNum[1] = 14;
            }
            if (oltclable == "1516")
            {
                label8.Text = "15-->16";
                OltcNum[0] = 15;
                OltcNum[1] = 16;
            }
            if (oltclable == "1615")
            {
                label8.Text = "16-->15";
                OltcNum[0] = 16;
                OltcNum[1] = 15;
            }
            if (oltclable == "1617")
            {
                label8.Text = "16-->17";
                OltcNum[0] = 16;
                OltcNum[1] = 17;
            }
            if (oltclable == "1716")
            {
                label8.Text = "17-->16";
                OltcNum[0] = 17;
                OltcNum[1] = 16;
            }
            if (oltclable == "1718")
            {
                label8.Text = "17-->18";
                OltcNum[0] = 17;
                OltcNum[1] = 18;
            }
            if (oltclable == "1817")
            {
                label8.Text = "18-->17";
                OltcNum[0] = 18;
                OltcNum[1] = 17;
            }
            if (oltclable == "1819")
            {
                label8.Text = "18-->19";
                OltcNum[0] = 18;
                OltcNum[1] = 19;

            }
            if (oltclable == "1920")
            {
                label8.Text = "19-->20";
                OltcNum[0] = 19;
                OltcNum[1] = 20;
            }

        }

        private void Form2_Load(object sender, EventArgs e)
        {

            panel1.Visible = false;
            panel2.Visible = false;
            panel3.Visible = false;
            panel4.Visible = false;
            panel5.Visible = false;
            panel6.Visible = false;
            panel7.Visible = false;
            panel8.Visible = false;
            panel9.Visible = false;
            panel10.Visible = false;
            panel11.Visible = false;
            panel12.Visible = false;
            #region
            var chart = chart1.ChartAreas[0];
            chart.AxisX.IntervalType = DateTimeIntervalType.Number;
            chart.AxisX.LabelStyle.Format = "";
            chart.AxisY.LabelStyle.Format = "";
            chart.AxisY.LabelStyle.IsEndLabelVisible = true;
            chart1.ChartAreas[0].AxisX.MajorGrid.LineColor = Color.BurlyWood;
            chart1.ChartAreas[0].AxisY.MajorGrid.LineColor = Color.BurlyWood;
            chart1.Series[0].IsVisibleInLegend = false;
            this.chart1.ChartAreas[0].AxisY.IsReversed = true;
            chart1.ChartAreas[0].CursorX.IsUserEnabled = true;
            chart1.ChartAreas[0].CursorX.AutoScroll = false;
            chart1.ChartAreas[0].CursorX.IsUserSelectionEnabled = true;
            chart1.ChartAreas[0].CursorY.IsUserEnabled = true;
            chart1.ChartAreas[0].CursorY.AutoScroll = false;
            chart1.ChartAreas[0].CursorY.IsUserSelectionEnabled = true;
            chart1.ChartAreas[0].AxisX.ScaleView.Zoomable = true;
            chart1.ChartAreas[0].AxisY.ScaleView.Zoomable = true;
            chart1.ChartAreas[0].AxisX.ScrollBar.IsPositionedInside = true;
            chart1.ChartAreas[0].AxisY.ScrollBar.IsPositionedInside = true;
            chart1.ChartAreas[0].AxisX.ScrollBar.Size = 15;
            chart1.ChartAreas[0].AxisY.ScrollBar.Size = 15;
            chart1.ChartAreas[0].AxisX.ScrollBar.ButtonStyle = ScrollBarButtonStyles.ResetZoom;
            chart1.ChartAreas[0].AxisX.ScrollBar.ButtonColor = Color.SkyBlue;
            chart1.ChartAreas[0].AxisY.ScrollBar.ButtonStyle = ScrollBarButtonStyles.ResetZoom;
            chart1.ChartAreas[0].AxisY.ScrollBar.ButtonColor = Color.SkyBlue;
            chart1.ChartAreas[0].AxisX.ScaleView.SmallScrollSize = double.NaN;
            chart1.ChartAreas[0].AxisX.ScaleView.SmallScrollMinSize = 1;
            chart1.ChartAreas[0].AxisY.ScaleView.SmallScrollSize = double.NaN;
            chart1.ChartAreas[0].AxisY.ScaleView.SmallScrollMinSize = 1;
            chart1.ChartAreas[0].AxisX.MajorGrid.Enabled = true;
            chart1.ChartAreas[0].AxisY.Title = "m Ω";
            try
            {
                chart1.Series.Add("A相");
                chart1.Series["A相"].ChartType = SeriesChartType.Line;
                chart1.Series["A相"].Color = Color.Red;

            }
            catch
            {

                MessageBox.Show("line已经绘制完成");
            }
            for (int i = 0; i < 6000; i++)
            {
                if (Tempdata[6000].ToString() == "1")
                    Tempdata[i] = Convert.ToInt16(Tempdata[i] * 500 / 32768);
                if (Tempdata[6000].ToString() == "2")
                    Tempdata[i] = Convert.ToInt16(Tempdata[i] * 1000 / 32768);
                if (Tempdata[6000].ToString() == "3")
                    Tempdata[i] = Convert.ToInt16(Tempdata[i] * 5000 / 32768);
                if (Tempdata[6000].ToString() == "4")
                    Tempdata[i] = Convert.ToInt16(Tempdata[i] * 10000 / 32768);
                if (Tempdata[6000].ToString() == "5")
                    Tempdata[i] = Convert.ToInt16(Tempdata[i] * 50000 / 32768);
                chart1.Series["A相"].Points.AddXY(i + 1, Tempdata[i]);


            }
            #endregion
            #region
            var chartT2 = chart2.ChartAreas[0];
            chartT2.AxisX.IntervalType = DateTimeIntervalType.Number;
            chartT2.AxisX.LabelStyle.Format = "";
            chartT2.AxisY.LabelStyle.Format = "";
            chartT2.AxisY.LabelStyle.IsEndLabelVisible = true;
            chart2.ChartAreas[0].AxisX.MajorGrid.LineColor = Color.BurlyWood;
            chart2.ChartAreas[0].AxisY.MajorGrid.LineColor = Color.BurlyWood;
            chart2.Series[0].IsVisibleInLegend = false;
            this.chart2.ChartAreas[0].AxisY.IsReversed = true;
            chart2.ChartAreas[0].CursorX.IsUserEnabled = true;
            chart2.ChartAreas[0].CursorX.AutoScroll = false;
            chart2.ChartAreas[0].CursorX.IsUserSelectionEnabled = true;
            chart2.ChartAreas[0].CursorY.IsUserEnabled = true;
            chart2.ChartAreas[0].CursorY.AutoScroll = false;
            chart2.ChartAreas[0].CursorY.IsUserSelectionEnabled = true;
            chart2.ChartAreas[0].AxisX.ScaleView.Zoomable = true;
            chart2.ChartAreas[0].AxisY.ScaleView.Zoomable = true;
            chart2.ChartAreas[0].AxisX.ScrollBar.IsPositionedInside = true;
            chart2.ChartAreas[0].AxisY.ScrollBar.IsPositionedInside = true;
            chart2.ChartAreas[0].AxisX.ScrollBar.Size = 15;
            chart2.ChartAreas[0].AxisY.ScrollBar.Size = 15;
            chart2.ChartAreas[0].AxisX.ScrollBar.ButtonStyle = ScrollBarButtonStyles.ResetZoom;
            chart2.ChartAreas[0].AxisX.ScrollBar.ButtonColor = Color.SkyBlue;
            chart2.ChartAreas[0].AxisY.ScrollBar.ButtonStyle = ScrollBarButtonStyles.ResetZoom;
            chart2.ChartAreas[0].AxisY.ScrollBar.ButtonColor = Color.SkyBlue;
            chart2.ChartAreas[0].AxisX.ScaleView.SmallScrollSize = double.NaN;
            chart2.ChartAreas[0].AxisX.ScaleView.SmallScrollMinSize = 1;
            chart2.ChartAreas[0].AxisY.ScaleView.SmallScrollSize = double.NaN;
            chart2.ChartAreas[0].AxisY.ScaleView.SmallScrollMinSize = 1;
            chart2.ChartAreas[0].AxisX.MajorGrid.Enabled = true;
            chart2.ChartAreas[0].AxisY.Title = "m Ω";
            try
            {

                chart2.Series.Add("B相");
                chart2.Series["B相"].ChartType = SeriesChartType.Line;
                chart2.Series["B相"].Color = Color.Green;


            }
            catch
            {

                MessageBox.Show("line已经绘制完成");
            }

            for (int i = 6002; i < 12002; i++)
            {
                if (Tempdata[12002].ToString() == "1")
                    Tempdata[i] = Convert.ToInt16(Tempdata[i] * 500 / 32768);
                if (Tempdata[12002].ToString() == "2")
                    Tempdata[i] = Convert.ToInt16(Tempdata[i] * 1000 / 32768);
                if (Tempdata[12002].ToString() == "3")
                    Tempdata[i] = Convert.ToInt16(Tempdata[i] * 5000 / 32768);
                if (Tempdata[12002].ToString() == "4")
                    Tempdata[i] = Convert.ToInt16(Tempdata[i] * 10000 / 32768);
                if (Tempdata[12002].ToString() == "5")
                    Tempdata[i] = Convert.ToInt16(Tempdata[i] * 50000 / 32768);
                chart2.Series["B相"].Points.AddXY(i - 6002 + 1, Tempdata[i]);


            }


            #endregion
            #region

            var chartT3 = chart3.ChartAreas[0];
            chartT3.AxisX.IntervalType = DateTimeIntervalType.Number;
            chartT3.AxisX.LabelStyle.Format = "";
            chartT3.AxisY.LabelStyle.Format = "";
            chartT3.AxisY.LabelStyle.IsEndLabelVisible = true;
            chart3.ChartAreas[0].AxisX.MajorGrid.LineColor = Color.BurlyWood;
            chart3.ChartAreas[0].AxisY.MajorGrid.LineColor = Color.BurlyWood;
            chart3.Series[0].IsVisibleInLegend = false;
            this.chart3.ChartAreas[0].AxisY.IsReversed = true;
            chart3.ChartAreas[0].CursorX.IsUserEnabled = true;
            chart3.ChartAreas[0].CursorX.AutoScroll = false;
            chart3.ChartAreas[0].CursorX.IsUserSelectionEnabled = true;
            chart3.ChartAreas[0].CursorY.IsUserEnabled = true;
            chart3.ChartAreas[0].CursorY.AutoScroll = false;
            chart3.ChartAreas[0].CursorY.IsUserSelectionEnabled = true;
            chart3.ChartAreas[0].AxisX.ScaleView.Zoomable = true;
            chart3.ChartAreas[0].AxisY.ScaleView.Zoomable = true;
            chart3.ChartAreas[0].AxisX.ScrollBar.IsPositionedInside = true;
            chart3.ChartAreas[0].AxisY.ScrollBar.IsPositionedInside = true;
            chart3.ChartAreas[0].AxisX.ScrollBar.Size = 15;
            chart3.ChartAreas[0].AxisY.ScrollBar.Size = 15;
            chart3.ChartAreas[0].AxisX.ScrollBar.ButtonStyle = ScrollBarButtonStyles.ResetZoom;
            chart3.ChartAreas[0].AxisX.ScrollBar.ButtonColor = Color.SkyBlue;
            chart3.ChartAreas[0].AxisY.ScrollBar.ButtonStyle = ScrollBarButtonStyles.ResetZoom;
            chart3.ChartAreas[0].AxisY.ScrollBar.ButtonColor = Color.SkyBlue;
            chart3.ChartAreas[0].AxisX.ScaleView.SmallScrollSize = double.NaN;
            chart3.ChartAreas[0].AxisX.ScaleView.SmallScrollMinSize = 1;
            chart3.ChartAreas[0].AxisY.ScaleView.SmallScrollSize = double.NaN;
            chart3.ChartAreas[0].AxisY.ScaleView.SmallScrollMinSize = 1;
            chart3.ChartAreas[0].AxisX.MajorGrid.Enabled = true;
            chart3.ChartAreas[0].AxisY.Title = "m Ω";
            try
            {

                chart3.Series.Add("C相");
                chart3.Series["C相"].ChartType = SeriesChartType.Line;
                //绘制曲线图
                // chart1.Series["line1"].ChartType = SeriesChartType.Spline;
                // chart1.Series["line1"].XValueMember = Tempdata;
                chart3.Series["C相"].Color = Color.Black;
            }
            catch
            {

                MessageBox.Show("line已经绘制完成");
            }
            for (int i = 12004; i < 18004; i++)
            {
                if (Tempdata[18004].ToString() == "1")
                    Tempdata[i] = Convert.ToInt16(Tempdata[i] * 500 / 32768);
                if (Tempdata[18004].ToString() == "2")
                    Tempdata[i] = Convert.ToInt16(Tempdata[i] * 1000 / 32768);
                if (Tempdata[18004].ToString() == "3")
                    Tempdata[i] = Convert.ToInt16(Tempdata[i] * 5000 / 32768);
                if (Tempdata[18004].ToString() == "4")
                    Tempdata[i] = Convert.ToInt16(Tempdata[i] * 10000 / 32768);
                if (Tempdata[18004].ToString() == "5")
                    Tempdata[i] = Convert.ToInt16(Tempdata[i] * 50000 / 32768);
                chart3.Series["C相"].Points.AddXY(i - 12004 + 1, Tempdata[i]);


            }
            #endregion
            #region
            //this.panel1.Parent = chart1;
            //this.panel2.Parent = chart1;
            //this.panel7.Parent = chart1;
            //this.panel8.Parent = chart1;
            //this.panel3.Parent = chart2;
            //this.panel4.Parent = chart2;
            //this.panel9.Parent = chart2;
            //this.panel10.Parent = chart2;
            //this.panel5.Parent = chart3;
            //this.panel6.Parent = chart3;
            //this.panel11.Parent = chart3;
            //this.panel12.Parent = chart3;
            this.panel1.BackColor = Color.FromArgb(150, 255, 0, 0);
            this.panel2.BackColor = Color.FromArgb(150, 255, 0, 0);
            this.panel3.BackColor = Color.FromArgb(150, 255, 0, 0);
            this.panel4.BackColor = Color.FromArgb(150, 255, 0, 0);
            this.panel5.BackColor = Color.FromArgb(150, 255, 0, 0);
            this.panel6.BackColor = Color.FromArgb(150, 255, 0, 0);
            this.panel7.BackColor = Color.FromArgb(150, 255, 0, 0);
            this.panel8.BackColor = Color.FromArgb(150, 255, 0, 0);
            this.panel9.BackColor = Color.FromArgb(150, 255, 0, 0);
            this.panel10.BackColor = Color.FromArgb(150, 255, 0, 0);
            this.panel11.BackColor = Color.FromArgb(150, 255, 0, 0);
            this.panel12.BackColor = Color.FromArgb(150, 255, 0, 0);
            #endregion
        }

        private int aX, aX1;

        private void Panel1_MouseMove(object sender, MouseEventArgs e)
        {
            panel1.BringToFront();
            if (e.Button == MouseButtons.Left)
            {
                movepanel(panel1, e, aX);

            }
            SetChart1();
            SetChart2();
            SetChart3();



        }

        private void Panel2_MouseDown(object sender, MouseEventArgs e)
        {
            aX1 = e.X;

        }


        private void Panel2_MouseMove(object sender, MouseEventArgs e)
        {
            panel2.BringToFront();
            if (e.Button == MouseButtons.Left)
            {
                movepanel(panel2, e, aX1);
                SetChart1();
            }
        }


        private void movepanel(Panel panel, MouseEventArgs e, int ax)
        {
            if (panel.Left > 92 && panel.Left < 592)
                panel.Left = panel.Left + (e.X - ax);
            else
            {
                if (panel.Left <= 92)
                {
                    if ((e.X - ax) >= 0)
                        panel.Left = panel.Left + (e.X - ax);
                }
                else
                {
                    if ((e.X - ax) <= 0)
                        panel.Left = panel.Left + (e.X - ax);
                }

            }
        }

        private void Panel1_MouseUp(object sender, MouseEventArgs e)
        {
            SetChart1();
            SetChart2();
            SetChart3();

        }


        private int ax4, ax3, ax5, ax6;
        private int ax7, ax8, ax9, ax10, ax11, ax12;
        private void Panel4_MouseDown(object sender, MouseEventArgs e)
        {

            //md 
            ax4 = e.X;
            //
        }

        private void Panel4_MouseMove(object sender, MouseEventArgs e)
        {
            panel4.BringToFront();


            if (e.Button == MouseButtons.Left)
            {
                movepanel(panel4, e, ax4);

            }
            SetChart1();
            SetChart2();
            SetChart3();
            //
        }

        private void Panel3_MouseMove(object sender, MouseEventArgs e)
        {
            panel3.BringToFront();
            if (e.Button == MouseButtons.Left)
            {
                movepanel(panel3, e, ax3);

            }

            SetChart1();
            SetChart2();
            SetChart3();

            //
        }

        private void Panel3_MouseDown(object sender, MouseEventArgs e)
        {
            ax3 = e.X;
        }

        private void Panel5_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                movepanel(panel5, e, ax5);
            }
            //md  aX1 = e.X;
            SetChart1();
            SetChart2();
            SetChart3();
            //
        }

        private int XViewMax { get; set; }
        private int XViewMin { get; set; }
        private int YViewMin { get; set; }
        private int YViewMax { get; set; }


        private int GetYvalue(int xvalue, Chart chart11)
        {
            try
            {
                return Convert.ToInt32(chart11.Series[1].Points[xvalue].YValues[0]);
            }
            catch
            {
                return 0;
            }
        }

        private void SetChart1()
        {
            XViewMax = Convert.ToInt32(chart1.ChartAreas[0].AxisX.ScaleView.ViewMaximum);
            XViewMin = Convert.ToInt32(chart1.ChartAreas[0].AxisX.ScaleView.ViewMinimum);
            YViewMax = Convert.ToInt32(chart1.ChartAreas[0].AxisY.ScaleView.ViewMaximum);
            YViewMin = Convert.ToInt32(chart1.ChartAreas[0].AxisY.ScaleView.ViewMinimum);
            int Left1Pane1, Left1Pane2, Left1Pane3, Left1Pane4;
            int[] panelleftdata = new int[4] { panel1.Left, panel2.Left, panel7.Left, panel8.Left };
            Array.Sort(panelleftdata);
            Left1Pane1 = panelleftdata[0] - 92;
            Left1Pane2 = panelleftdata[1] - 92;
            Left1Pane3 = panelleftdata[2] - 92;
            Left1Pane4 = panelleftdata[3] - 92;
            int P1Xnum = XViewMin + Left1Pane1 * (XViewMax - XViewMin) / 500;
            int P2Xnum = XViewMin + Left1Pane2 * (XViewMax - XViewMin) / 500;
            int P3Xnum = XViewMin + Left1Pane3 * (XViewMax - XViewMin) / 500;
            int P4Xnum = XViewMin + Left1Pane4 * (XViewMax - XViewMin) / 500;
            int P1YValue = GetYvalue(P1Xnum, chart1);
            int P2YValue = GetYvalue(P2Xnum, chart1);
            int P3YValue = GetYvalue(P3Xnum, chart1);
            int P4YValue = GetYvalue(P4Xnum, chart1);
            List<int> needtestda = new List<int>();
            var a = Tempdata.Skip(1).Take(6000);
            foreach (var b in a)
            {
                needtestda.Add(b);
            }
            label5.Text = "过渡时间:" + Convert.ToString(Math.Abs(P4Xnum - P1Xnum) * 0.05) + "ms" + "\n\t"
                + "过渡电阻R1:" + GetElevationMode(needtestda.Skip(P1Xnum).Take(P2Xnum - P1Xnum).ToList()) + "mΩ"
                + "\t\n过渡电阻R2:" + GetElevationMode(needtestda.Skip(P3Xnum).Take(P4Xnum - P3Xnum).ToList()) + "mΩ";

            or.AOverTime = Math.Abs(P4Xnum - P1Xnum) * 0.05f;
            or.AOverResistanceOne = GetElevationMode(needtestda.Skip(P1Xnum).Take(P2Xnum - P1Xnum).ToList());
            or.AOverResistanceTwo = GetElevationMode(needtestda.Skip(P3Xnum).Take(P4Xnum - P3Xnum).ToList());
        }
        private void SetChart2()
        {


            XViewMax = Convert.ToInt32(chart1.ChartAreas[0].AxisX.ScaleView.ViewMaximum);
            XViewMin = Convert.ToInt32(chart1.ChartAreas[0].AxisX.ScaleView.ViewMinimum);
            YViewMax = Convert.ToInt32(chart1.ChartAreas[0].AxisY.ScaleView.ViewMaximum);
            YViewMin = Convert.ToInt32(chart1.ChartAreas[0].AxisY.ScaleView.ViewMinimum);
            int Left1Pane1, Left1Pane2, Left1Pane3, Left1Pane4;
            int[] panelleftdata = new int[4] { panel3.Left, panel4.Left, panel9.Left, panel10.Left };
            Array.Sort(panelleftdata);
            Left1Pane1 = panelleftdata[0] - 92;
            Left1Pane2 = panelleftdata[1] - 92;
            Left1Pane3 = panelleftdata[2] - 92;
            Left1Pane4 = panelleftdata[3] - 92;
            int P1Xnum = XViewMin + Left1Pane1 * (XViewMax - XViewMin) / 500;
            int P2Xnum = XViewMin + Left1Pane2 * (XViewMax - XViewMin) / 500;
            int P3Xnum = XViewMin + Left1Pane3 * (XViewMax - XViewMin) / 500;
            int P4Xnum = XViewMin + Left1Pane4 * (XViewMax - XViewMin) / 500;
            int P1YValue = GetYvalue(P1Xnum, chart2);
            int P2YValue = GetYvalue(P2Xnum, chart2);
            int P3YValue = GetYvalue(P3Xnum, chart2);
            int P4YValue = GetYvalue(P4Xnum, chart2);
            List<int> needtestda = new List<int>();
            var a = Tempdata.Skip(1).Take(6000);
            foreach (var b in a)
            {
                needtestda.Add(b);
            }
            label6.Text = "过渡时间:" + Convert.ToString(Math.Abs(P4Xnum - P1Xnum) * 0.05) + "ms" + "\n\t"
                + "过渡电阻R1:" + GetElevationMode(needtestda.Skip(P1Xnum).Take(P2Xnum - P1Xnum).ToList()) + "mΩ"
                + "\t\n过渡电阻R2:" + GetElevationMode(needtestda.Skip(P3Xnum).Take(P4Xnum - P3Xnum).ToList()) + "mΩ";
            or.BOverTime = Math.Abs(P4Xnum - P1Xnum) * 0.05f; ;
            or.BOverResistanceOne = GetElevationMode(needtestda.Skip(P1Xnum).Take(P2Xnum - P1Xnum).ToList());
            or.BOverResistanceTwo = GetElevationMode(needtestda.Skip(P3Xnum).Take(P4Xnum - P3Xnum).ToList());
        }
        private void SetChart3()
        {

            XViewMax = Convert.ToInt32(chart1.ChartAreas[0].AxisX.ScaleView.ViewMaximum);
            XViewMin = Convert.ToInt32(chart1.ChartAreas[0].AxisX.ScaleView.ViewMinimum);
            YViewMax = Convert.ToInt32(chart1.ChartAreas[0].AxisY.ScaleView.ViewMaximum);
            YViewMin = Convert.ToInt32(chart1.ChartAreas[0].AxisY.ScaleView.ViewMinimum);
            int Left1Pane1, Left1Pane2, Left1Pane3, Left1Pane4;
            int[] panelleftdata = new int[4] { panel5.Left, panel6.Left, panel11.Left, panel12.Left };
            Array.Sort(panelleftdata);
            Left1Pane1 = panelleftdata[0] - 92;
            Left1Pane2 = panelleftdata[1] - 92;
            Left1Pane3 = panelleftdata[2] - 92;
            Left1Pane4 = panelleftdata[3] - 92;
            int P1Xnum = XViewMin + Left1Pane1 * (XViewMax - XViewMin) / 500;
            int P2Xnum = XViewMin + Left1Pane2 * (XViewMax - XViewMin) / 500;
            int P3Xnum = XViewMin + Left1Pane3 * (XViewMax - XViewMin) / 500;
            int P4Xnum = XViewMin + Left1Pane4 * (XViewMax - XViewMin) / 500;
            int P1YValue = GetYvalue(P1Xnum, chart3);
            int P2YValue = GetYvalue(P2Xnum, chart3);
            int P3YValue = GetYvalue(P3Xnum, chart3);
            int P4YValue = GetYvalue(P4Xnum, chart3);
            List<int> needtestda = new List<int>();
            var a = Tempdata.Skip(1).Take(6000);
            foreach (var b in a)
            {
                needtestda.Add(b);
            }
            label7.Text = "过渡时间:" + Convert.ToString(Math.Abs(P4Xnum - P1Xnum) * 0.05) + "ms" + "\n\t"
                + "过渡电阻R1:" + GetElevationMode(needtestda.Skip(P1Xnum).Take(P2Xnum - P1Xnum).ToList()) + "mΩ"
                + "\t\n过渡电阻R2:" + GetElevationMode(needtestda.Skip(P3Xnum).Take(P4Xnum - P3Xnum).ToList()) + "mΩ";
            or.COverTime = Math.Abs(P4Xnum - P1Xnum) * 0.05f; ;
            or.COverResistanceOne = GetElevationMode(needtestda.Skip(P1Xnum).Take(P2Xnum - P1Xnum).ToList());
            or.COverResistanceTwo = GetElevationMode(needtestda.Skip(P3Xnum).Take(P4Xnum - P3Xnum).ToList());
        }
        private void Panel3_MouseUp(object sender, MouseEventArgs e)
        {
            SetChart1();
            SetChart2();
            SetChart3();
        }
        private void Panel4_MouseUp(object sender, MouseEventArgs e)
        {
            SetChart1();
            SetChart2();
            SetChart3();
        }
        private void Panel5_MouseUp(object sender, MouseEventArgs e)
        {
            SetChart1();
            SetChart2();
            SetChart3();
        }
        private void Panel6_MouseUp(object sender, MouseEventArgs e)
        {
            SetChart1();
            SetChart2();
            SetChart3();
        }
        private void Button2_Click_1(object sender, EventArgs e)
        {
            panel1.Visible = false;
            panel2.Visible = false;
            panel3.Visible = false;
            panel4.Visible = false;
            panel5.Visible = false;
            panel6.Visible = false;
            panel7.Visible = false;
            panel8.Visible = false;
            panel9.Visible = false;
            panel10.Visible = false;
            panel11.Visible = false;
            panel12.Visible = false;
        }
        /// <summary>
        /// 获取众数，如果多个求平均值，float转int Auto
        /// </summary>
        /// <param name="elevationList"></param>
        /// <returns></returns>
        private static int GetElevationMode(List<int> elevationList)
        {
            try
            {
                int count;
                bool flag = false;
                Dictionary<int, int> dictionary = new Dictionary<int, int>();
                for (int i = 0; i < elevationList.Count; i++)
                {
                    if (dictionary.TryGetValue(elevationList[i], out count))
                    {
                        flag = true;
                        dictionary[elevationList[i]]++;
                    }
                    else
                        dictionary.Add(elevationList[i], 1);
                }
                //如果没有众数，返回空
                if (!flag)
                    return 0;
                int max = 0;
                int position = 0;
                int[] modeArray = new int[elevationList.Count];//众数数组
                //遍历hash表
                foreach (KeyValuePair<int, int> myKey in dictionary)
                {
                    if (myKey.Value > max)
                    {
                        max = myKey.Value;
                        position = 0;
                        modeArray[0] = myKey.Key;
                    }
                    else if (myKey.Value == max)
                        modeArray[++position] = myKey.Key;
                }
                Array.Resize(ref modeArray, position + 1);
                int mode = 0;
                //如果众数不唯一，求平均数
                if (modeArray.Length > 1)
                {
                    for (int i = 0; i < modeArray.Length; i++)
                    {
                        mode += modeArray[i];
                    }
                    double elevationMode = mode / modeArray.Length;
                    return (int)elevationMode;
                }
                //如果众数唯一，返回众数
                else
                {
                    mode = modeArray[0];
                }
                return mode;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return 0;
            }
        }

        private void Button1_Click_1(object sender, EventArgs e)
        {
            panel2.Visible = true;
            panel1.Visible = true;
            panel3.Visible = true;
            panel4.Visible = true;
            panel5.Visible = true;
            panel6.Visible = true;
            panel7.Visible = true;
            panel8.Visible = true;
            panel9.Visible = true;
            panel10.Visible = true;
            panel11.Visible = true;
            panel12.Visible = true;
            SetChart1();
            SetChart2();
            SetChart3();
        }
        private void saveimages(string name)
        {
            Bitmap bit = new Bitmap(this.Width, this.Height);
            Graphics g = Graphics.FromImage(bit);
            g.CopyFromScreen(new Point(this.Location.X + 10, this.Location.Y), new Point(20, 0), bit.Size);
            if (!Directory.Exists("D:\\waveImage"))
                WriteDataToFile.DeelDirectoryInfo("D:\\waveImage", Mode.Create);
            bit.Save("D:\\waveImage\\" + name + ".jpg");
            g.Dispose();
        }
        private void Panel2_MouseUp(object sender, MouseEventArgs e)
        {

        }

        private void Panel8_MouseDown(object sender, MouseEventArgs e)
        {
            ax8 = e.X;
        }

        private void Panel8_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                movepanel(panel8, e, ax8);

            }
            //md  aX1 = e.X;
            SetChart1();
            SetChart2();
            SetChart3();
        }
        private void Panel9_MouseDown(object sender, MouseEventArgs e)
        {
            ax9 = e.X;
        }

        private void Panel9_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                movepanel(panel9, e, ax9);

            }
            //md  aX1 = e.X;
            SetChart1();
            SetChart2();
            SetChart3();
        }
        private void Panel7_MouseDown(object sender, MouseEventArgs e)
        {
            ax7 = e.X;
        }

        private void Panel7_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                movepanel(panel7, e, ax7);
            }
            //md  aX1 = e.X;
            SetChart1();
            SetChart2();
            SetChart3();
        }
        private void Panel10_MouseDown(object sender, MouseEventArgs e)
        {
            ax10 = e.X;
        }

        private void Label6_Click(object sender, EventArgs e)
        {

        }

        private void Label7_Click(object sender, EventArgs e)
        {

        }
        OverResult or = new OverResult();
        private void Button3_Click(object sender, EventArgs e)
        {
            //if (!WorkingSets.local.IsCreate)
            //{
            //    InsertDataTodatabase.Deelwavedatabase("");
            //    WorkingSets.local.IsCreate = true;
            //}
            or.OLTcNum = OltcNum;
            or.WaveName = DateTime.Now.ToFileTimeUtc().ToString();
            saveimages(or.WaveName);
            InsertDataTodatabase.CreateWaveResult(or, Testcode);
            this.Close();

        }

        private void Chart1_Click(object sender, EventArgs e)
        {

        }

        private void Panel10_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                movepanel(panel10, e, ax10);

            }
            SetChart1();
            SetChart2();
            SetChart3();
        }

        private void Panel11_MouseDown(object sender, MouseEventArgs e)
        {
            ax11 = e.X;
        }

        private void Panel11_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                movepanel(panel11, e, ax11);

            }
            //md  aX1 = e.X;
            SetChart1();
            SetChart2();
            SetChart3();
        }

        private void Panel12_MouseDown(object sender, MouseEventArgs e)
        {
            ax12 = e.X;
        }

        private void Panel12_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                movepanel(panel12, e, ax12);

            }
            //md  aX1 = e.X;
            SetChart1();
            SetChart2();
            SetChart3();
        }

        private void Panel5_MouseDown(object sender, MouseEventArgs e)
        {
            ax5 = e.X;
        }
        private void Panel6_MouseDown(object sender, MouseEventArgs e)
        {
            ax6 = e.X;
        }

        private void Panel6_MouseMove(object sender, MouseEventArgs e)
        {
            panel6.BringToFront();


            if (e.Button == MouseButtons.Left)
            {
                movepanel(panel6, e, ax6);

            }
            SetChart1();
            SetChart2();
            SetChart3();

        }

        private void Panel1_MouseDown(object sender, MouseEventArgs e)
        {
            aX = e.X;
        }


    }


}
