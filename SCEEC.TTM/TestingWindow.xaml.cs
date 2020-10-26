using System;
using System.Collections.Generic;
using System.Globalization;
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
using System.Windows.Shapes;
using SCEEC.MI.TZ3310;
using System.ComponentModel;
using SCEEC.Numerics;
using System.Threading;
using System.Drawing;
using System.Drawing.Drawing2D;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using System.IO;

namespace SCEEC.TTM
{
    public delegate void RefreshStata(TestingWorkerSender stata);
    /// <summary>
    /// WindowTesting.xaml 的交互逻辑
    /// </summary>
    public partial class WindowTesting : Window
    {
        public bool inited = false;
        public bool IsStable = false;//
        BackgroundWorker TestingWorker;

        JobList currentJob;
        TestingWorkerSender worker;

        public WindowTesting(string transformerSerialNo, string jobName, JobInformation job, int testID = -1, bool istcp = false)
        {
            InitializeComponent();
            //   InitCreateChart();
            this.DataContext = this;

            currentJob = WorkingSets.local.getJob(transformerSerialNo, jobName);
            TestingWorker = new BackgroundWorker();
            TestingWorker.WorkerReportsProgress = true;
            TestingWorker.WorkerSupportsCancellation = true;
            TestingWorker.DoWork += TestingWorker_DoWork;
            TestingWorker.ProgressChanged += TestingWorker_ProgressChanged;
            TestingWorker.RunWorkerCompleted += TestingWorker_RunWorkerCompleted;
            if (testID < 0)
                worker = new TestingWorkerSender()
                {
                    Transformer = currentJob.Transformer,
                    job = currentJob,
                    MeasurementItems = Translator.JobList2MeasurementItems(currentJob).ToArray(),
                    CurrentItemIndex = 0,
                    ProgressPercent = 0
                };
            else
            {
                worker = TestingWorkerSender.FromDatabaseRows(testID);
            }
            StatusRefresh(worker);
            if (istcp)
            {
                currentJob.Information = job;
            }
            else
            {
                TestingInfoWindow testingInfoWindow = new TestingInfoWindow(worker);

                if (testingInfoWindow.ShowDialog() != true)
                {
                    // currentJob.Information = testingInfoWindow.Information;

                    inited = false;
                }
                else
                {
                    currentJob.Information = testingInfoWindow.Information;

                    var a = currentJob.Information.GetHashCode();
                    worker.job = currentJob;
                    inited = true;
                }
            }


        }

        private short[] getWaveData(string path = "C:\\wave\\2.txt")
        {
            var data = File.ReadAllText(path).Trim().Replace("\n", "").Replace("\r", "").Replace(" ", "");
            var by = data.Split(']', '[');
            List<short> ret = new List<short>();
            for (int i = 0; i < by.Length; i++)
            {
                if (i != 0 && i % 2 == 0)
                {
                    ret.Add((short)Convert.ToInt32(by[i]));
                }
            }
            return ret.ToArray();
        }

        private void TestingWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            var worker = e.Argument as TestingWorkerSender;
            WorkingSets.local.TestResults.Rows.Add(worker.job.Information.ToDataRow(worker.job));
            WorkingSets.local.saveTestResults();
            while (worker.CurrentItemIndex < worker.MeasurementItems.Length)
            {
                if (worker.CurrentItemIndex + 1 == worker.MeasurementItems.Length &&
                  worker.MeasurementItems[worker.CurrentItemIndex].completed == true)
                {
                    TestingWorker.ReportProgress(100, worker);
                    Thread.Sleep(100);
                    return;
                }
                if (TestingWorker.CancellationPending == true)
                {
                    while (!Measurement.CancelWork(ref worker))
                    {
                        TestingWorker.ReportProgress(0, worker);
                        System.Threading.Thread.Sleep(500);
                    }
                    //worker.CurrentItemIndex--;
                    //worker.MeasurementItems[worker.CurrentItemIndex].completed = false;
                    return;
                }
                else
                {
                    Measurement.DoWork(ref worker);
                }
                TestingWorker.ReportProgress(worker.ProgressPercent, worker);
                Thread.Sleep(100);
            }
            //WorkingSets.local.getTestResultsFromJobID(worker.job.id);
            //InsertDataTodatabase.UpdataDatabase(WorkingSets.local.getTestResultsFromJobID(worker.job.id));

        }

        public void Refresh()
        {
            StatusRefresh(worker);
        }

        //dashboard2.Value = 7.56;
        //    DashboardValue2.Text = dashboard2.Value.ToString("0.00") + "kV";

        enum Whichgroupbox
        {
            DCI, DCR, CAP, OLTC, NONE
        }
        private void SetgroupboxVisible(Whichgroupbox num)
        {
            dcigroupbox.Visibility = Visibility.Hidden;
            dcrgroupbox.Visibility = Visibility.Hidden;
            capgroupbox.Visibility = Visibility.Hidden;
            oltcgroupbox.Visibility = Visibility.Hidden;
            Oltcgroupbox.Visibility = Visibility.Hidden;

            if (num == Whichgroupbox.DCI)
                dcigroupbox.Visibility = Visibility.Visible;
            if (num == Whichgroupbox.DCR)
                dcrgroupbox.Visibility = Visibility.Visible;
            if (num == Whichgroupbox.CAP)
                capgroupbox.Visibility = Visibility.Visible;
            if (num == Whichgroupbox.OLTC)
                oltcgroupbox.Visibility = Visibility.Visible;
            if (num == Whichgroupbox.NONE)
            {
                Oltcgroupbox.Visibility = Visibility.Visible;
            }

        }
        private void StatusRefresh(TestingWorkerSender status)
        {
            if (WorkingSets.local.IsVisible == true && WorkingSets.local.IsVisible1 == true)
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    ConfireIsOk.Visibility = Visibility;
                });
            }
            WorkingStatusLabel.Text = status.StatusText;
            int itemIndex = TestItemListBox.SelectedIndex;
            TestItemListBox.ItemsSource = status.GetList();
            ResultListBox.ItemsSource = TestingWorkerUtility.getFinalResultsText(status);
            if (itemIndex < TestItemListBox.Items.Count)
                TestItemListBox.SelectedIndex = itemIndex;
            if (status.MeasurementItems[status.CurrentItemIndex].Result != null)
            {
                if (status.MeasurementItems[status.CurrentItemIndex].Result.values != null)
                {
                    if (status.MeasurementItems[status.CurrentItemIndex].Function == MeasurementFunction.DCResistance)
                    {


                        SetgroupboxVisible(Whichgroupbox.DCR);
                        if (status.MeasurementItems[status.CurrentItemIndex].Result.values[2].value != null)
                            A_resistance_dashboad.Value = (double)status.MeasurementItems[status.CurrentItemIndex].Result.values[2].value;
                        if (status.MeasurementItems[status.CurrentItemIndex].Result.values[1].value != null)
                            A_current_dashboad.Value = (double)status.MeasurementItems[status.CurrentItemIndex].Result.values[1].value;
                        if (status.MeasurementItems[status.CurrentItemIndex].Result.values[2].value != null)
                            A_resistance_value.Text = status.MeasurementItems[status.CurrentItemIndex].Result.values[2].OriginText;
                        if (status.MeasurementItems[status.CurrentItemIndex].Result.values[1].value != null)
                            A_Current_value.Text = status.MeasurementItems[status.CurrentItemIndex].Result.values[1].OriginText;
                        if (status.MeasurementItems[status.CurrentItemIndex].Result.values[5].value != null)
                            B_resistance_dashboad.Value = (double)status.MeasurementItems[status.CurrentItemIndex].Result.values[5].value;
                        if (status.MeasurementItems[status.CurrentItemIndex].Result.values[4].value != null)
                            B_current_dashboad.Value = (double)status.MeasurementItems[status.CurrentItemIndex].Result.values[4].value;
                        if (status.MeasurementItems[status.CurrentItemIndex].Result.values[5].value != null)
                            B_resistance_value.Text = status.MeasurementItems[status.CurrentItemIndex].Result.values[5].OriginText;
                        if (status.MeasurementItems[status.CurrentItemIndex].Result.values[4].value != null)
                            B_Current_value.Text = status.MeasurementItems[status.CurrentItemIndex].Result.values[4].OriginText;
                        if (status.MeasurementItems[status.CurrentItemIndex].Result.values[8].value != null)
                            C_resistance_dashboad.Value = (double)status.MeasurementItems[status.CurrentItemIndex].Result.values[8].value;
                        if (status.MeasurementItems[status.CurrentItemIndex].Result.values[7].value != null)
                            C_current_dashboad.Value = (double)status.MeasurementItems[status.CurrentItemIndex].Result.values[7].value;
                        if (status.MeasurementItems[status.CurrentItemIndex].Result.values[8].value != null)
                            C_resistance_value.Text = status.MeasurementItems[status.CurrentItemIndex].Result.values[8].OriginText;
                        if (status.MeasurementItems[status.CurrentItemIndex].Result.values[7].value != null)
                            C_Current_value.Text = status.MeasurementItems[status.CurrentItemIndex].Result.values[7].OriginText;
                    }
                    if (status.MeasurementItems[status.CurrentItemIndex].Function == MeasurementFunction.DCInsulation ||
                        status.MeasurementItems[status.CurrentItemIndex].Function == MeasurementFunction.BushingDCInsulation)
                    {
                        SetgroupboxVisible(Whichgroupbox.DCI);
                        if (status.MeasurementItems[status.CurrentItemIndex].Result.values[0].value != null)
                            dciboard_volate.Value = (double)(status.MeasurementItems[status.CurrentItemIndex].Result.values[0].value / 1000);
                        if (status.MeasurementItems[status.CurrentItemIndex].Result.values[0].value != null)
                            dciboard_volate_value.Text = status.MeasurementItems[status.CurrentItemIndex].Result.values[0].OriginText;
                        if (status.MeasurementItems[status.CurrentItemIndex].Result.values[1].value != null)
                            dciboard_resistance.Value = (double)(status.MeasurementItems[status.CurrentItemIndex].Result.values[1].value / 1000000000);
                        if (status.MeasurementItems[status.CurrentItemIndex].Result.values[1].value != null)
                            dciboard_resistance_value.Text = status.MeasurementItems[status.CurrentItemIndex].Result.values[1].OriginText + "Ω";
                    }

                    if (status.MeasurementItems[status.CurrentItemIndex].Function == MeasurementFunction.Capacitance ||
                       status.MeasurementItems[status.CurrentItemIndex].Function == MeasurementFunction.BushingCapacitance)
                    {
                        SetgroupboxVisible(Whichgroupbox.CAP);
                        if (status.MeasurementItems[status.CurrentItemIndex].Result.values[1].value != null)
                            captance.Value = (double)(status.MeasurementItems[status.CurrentItemIndex].Result.values[1].value / 1000);
                        if (status.MeasurementItems[status.CurrentItemIndex].Result.values[1].value != null)
                            captancevalue.Text = status.MeasurementItems[status.CurrentItemIndex].Result.values[1].OriginText;

                    }
                    if (status.MeasurementItems[status.CurrentItemIndex].Function == MeasurementFunction.OLTCSwitchingCharacter)
                    {
                        SetgroupboxVisible(Whichgroupbox.OLTC);
                        if (status.MeasurementItems[status.CurrentItemIndex].Result.values[1] != null)
                            Aoltcdashboard.Value = (double)status.MeasurementItems[status.CurrentItemIndex].Result.values[1].value;
                        if (status.MeasurementItems[status.CurrentItemIndex].Result.values[1] != null)
                            Aoltcavalue.Text = status.MeasurementItems[status.CurrentItemIndex].Result.values[1].OriginText;
                        if (status.MeasurementItems[status.CurrentItemIndex].Result.values[4] != null)
                            Boltcdashboard.Value = (double)status.MeasurementItems[status.CurrentItemIndex].Result.values[4].value;
                        if (status.MeasurementItems[status.CurrentItemIndex].Result.values[4] != null)
                            Boltcavalue.Text = status.MeasurementItems[status.CurrentItemIndex].Result.values[4].OriginText;
                        if (status.MeasurementItems[status.CurrentItemIndex].Result.values[7] != null)
                            Coltcdashboard.Value = (double)status.MeasurementItems[status.CurrentItemIndex].Result.values[7].value;
                        if (status.MeasurementItems[status.CurrentItemIndex].Result.values[7] != null)
                            Boltcavalue.Text = status.MeasurementItems[status.CurrentItemIndex].Result.values[7].OriginText;
                        if (status.MeasurementItems[status.CurrentItemIndex].Result.waves != null)
                        {
                            SetgroupboxVisible(Whichgroupbox.NONE);
                            if (!WorkingSets.local.Testwave)
                            {
                                InitAllWavesbydata(status.MeasurementItems[status.CurrentItemIndex].Result.waves);
                                WorkingSets.local.Testwave = true;
                            }
                           //  TanEleVolatevalue = deelwaves(status.MeasurementItems[status.CurrentItemIndex].Result.waves)[wavelocation.SelectedIndex];
                        }
                    }


                }
            }
            progressBar.Value = status.ProgressPercent;
            RemainingTestNumLabel.Text = status.RemainingItemsCount.ToString();
            WorkingSets.local.status = status;
            GC.Collect();
        }
        private ChartValues<ObservablePoint>[] deelwaves(short[] waves, int current = 1)
        {
            ChartValues<ObservablePoint>[] ret = new ChartValues<ObservablePoint>[4];
            for (int i = 0; i < 4; i++)
            {
                ret[i] = new ChartValues<ObservablePoint>();
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
                        pdata = waves[s] * 500d / 32768d /1000/ current;
                    if (waves[6000].ToString() == "2")
                        pdata =waves[s] * 1000d / 32768d /1000/ current;
                    if (waves[6000].ToString() == "3")
                        pdata = waves[s] * 5000d /32768d /1000/ current;
                    if (waves[6000].ToString() == "4")
                        pdata = waves[s] * 10000d / 32768d /1000/ current;
                    if (waves[6000].ToString() == "5")
                        pdata =waves[s] * 50000d / 32768d / 1000/current;
                    ret[i].Add(new ObservablePoint { X = (s - 6002 * i + 1) * 0.05, Y = pdata });
                }

            }
            return ret;
        }



        public event RefreshStata Outstata;
        private void TestingWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            var status = e.UserState as TestingWorkerSender;
            worker = status;
            StatusRefresh(status);
        }

        private void TestingWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled == true)
            {
                return;
            }
        }

        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (this.WindowState != WindowState.Normal)
            {
                this.WindowState = WindowState.Normal;
                this.Top = 0;
            }
            this.DragMove();
        }

        private void MinimumButton_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void MaximumButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.WindowState == WindowState.Maximized)
            {
                this.WindowState = WindowState.Normal;
                maximumButtonImage.Source = new BitmapImage(new Uri("Resources/maximum.png", UriKind.Relative));
            }
            else
            {
                this.WindowState = WindowState.Maximized;
                maximumButtonImage.Source = new BitmapImage(new Uri("Resources/maximum2.png", UriKind.Relative));
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            WorkingSets.local.IsVisible1 = false;
            this.Close();
        }

        private void Window_StateChanged(object sender, EventArgs e)
        {
            switch (this.WindowState)
            {
                case WindowState.Minimized:
                    break;
                default:
                    this.Show();
                    this.Activate();
                    break;
            }
        }


        private void StopButton_Click(object sender, EventArgs e)
        {
            TestingWorker.CancelAsync();

        }

        public void StartButton_Click(object sender, MouseButtonEventArgs e)
        {

            //if (!WaveThread.IsAlive)
            //{
            //    WaveThread.IsBackground = true;
            //    WaveThread.Start();
            //}

            if (!TestingWorker.IsBusy)
            {
                TestingWorker.RunWorkerAsync(worker);
                //if (worker.MeasurementItems[worker.CurrentItemIndex].Function == MeasurementFunction.Information)
                //    StarText.Text = "确认稳定";
                //else if (worker.MeasurementItems[worker.CurrentItemIndex].Function == MeasurementFunction.DCResistance)
                //    StarText.Text = "确认稳定";
            }



        }
        private void Redo()
        {
            if (!TestingWorker.IsBusy)
            {
                worker.CurrentItemIndex = 0;
                TestingWorker.RunWorkerAsync(worker);
            }
        }

        Thread WaveThread = new Thread(() =>
        {
            int i = 0;
            while (true)
            {
                if (WorkingSets.local.ShowWaveForm)
                {
                    Form2 f2 = new Form2(WorkingSets.local.WaveFormSwicth, WorkingSets.local.OlTcLable);
                    f2.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
                    f2.ShowDialog();
                    WorkingSets.local.ShowWaveForm = false;
                    Thread.Sleep(500);
                    Bitmap bit = new Bitmap((int)f2.Width, (int)f2.Height);//实例化一个和窗体一样大的bitmap
                    Graphics g = Graphics.FromImage(bit);
                    g.CompositingQuality = CompositingQuality.HighQuality;//质量设为最高
                    g.CopyFromScreen((int)f2.Left + 10, (int)f2.Top + 5, 0, 0, new System.Drawing.Size((int)f2.Width - 20, (int)f2.Height - 20));//保存整个窗体为图片
                    bit.Save("WaveFormImage" + i.ToString() + ".png");//默认保存格式为PNG，保存成jpg格式质量不是很好
                    i++;
                }
            }
        });
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Refresh();
        }
        public bool ISStop { get; set; } = false;
        private void StopButton_Click(object sender, MouseButtonEventArgs e)
        {
            WorkingStatusLabel.Text = "已经停止测量，\t\n如需重测请点击开始测量";
            WorkingSets.local.IsCancer = true;
            TestingWorker.CancelAsync();
        }

        private void ConfireIsOk_Click(object sender, MouseButtonEventArgs e)
        {
            WorkingSets.local.IsStable = true;
            WorkingSets.local.IsVisible = false;
            ConfireIsOk.Visibility = Visibility.Collapsed;
        }

        bool[] line = new bool[4];
        LiveCharts.ChartPoint[] points = new LiveCharts.ChartPoint[4];
        private void control5_DataClick(object sender, LiveCharts.ChartPoint chartPoint)
        {
            if (!line[0])
            {
                deelwave(line1, 0);
                points[0] = chartPoint;
            }
            else if (!line[1])
            {
                points[1] = chartPoint;
                deelwave(line2, 1);
            }
            else if (!line[2])
            {
                points[2] = chartPoint;
                deelwave(line3, 2);
            }
            else if (!line[3])
            {
                points[3] = chartPoint;
                deelwave(line4, 3);
            }
            else
            {
                switch (whichline.SelectedIndex)
                {
                    case 0:
                        deelwave(line1, 0); break;
                    case 1:
                        deelwave(line2, 1); break;
                    case 2:
                        deelwave(line3, 2); break;
                    case 3:
                        deelwave(line4, 3); break;
                    default:
                        break;
                }
            }
        }
        void deelwave(Line Lline, int lineindex)
        {
            var mouselocation = Mouse.GetPosition(control5);
            Lline.X1 = mouselocation.X;
            Lline.X2 = mouselocation.X;
            Lline.Y1 = 10;
            Lline.Y2 = canvs.ActualHeight - 20;
            line[lineindex] = true;
        }

        private void control5_MouseDown(object sender, MouseButtonEventArgs e)
        {
        }

        public Func<double, string> XFormatter { get; set; }
        public Func<double, string> YFormatter { get; set; }

        public SeriesCollection LcCurrentVolate { get; set; }
        public SeriesCollection wave_b { get; set; }
        public SeriesCollection wave_c { get; set; }
        public ChartValues<ObservablePoint> TanEleVolatevalue { get; set; } = new ChartValues<ObservablePoint>();


        public void InitAllWaves()
        {
            var data = deelwaves(getWaveData());
            chart_a.Series.Add(IniLineSeries(data[0]));
            chart_b.Series.Add(IniLineSeries(data[1]));
            chart_c.Series.Add(IniLineSeries(data[2]));
        }

        public void InitAllWavesbydata(short[] tem)
        {
            var data = deelwaves(tem);
            chart_a.Series.Add(IniLineSeries(data[0]));
            chart_b.Series.Add(IniLineSeries(data[1]));
            chart_c.Series.Add(IniLineSeries(data[2]));
        }

        public LineSeries IniLineSeries(ChartValues<ObservablePoint> wave)
        {
            XFormatter = val => (val).ToString() + "ms";
            YFormatter = val => (val).ToString() + "mΩ";
            return new LineSeries
            {
                StrokeThickness = 2,
                Stroke = new SolidColorBrush(System.Windows.Media.Color.FromRgb(28, 142, 196)),
                Fill = System.Windows.Media.Brushes.Transparent,
                LineSmoothness = 10,//0为折现样式
                PointGeometrySize = 0,
                PointForeground = new SolidColorBrush(System.Windows.Media.Color.FromRgb(34, 46, 49)),
                Values = wave
            };

        }


        public void InitCreateChart()
        {
            LineSeries t1 = new LineSeries
            {
                StrokeThickness = 2,
                Stroke = new SolidColorBrush(System.Windows.Media.Color.FromRgb(28, 142, 196)),
                Fill = System.Windows.Media.Brushes.Transparent,
                LineSmoothness = 10,//0为折现样式
                PointGeometrySize = 0,
                PointForeground = new SolidColorBrush(System.Windows.Media.Color.FromRgb(34, 46, 49)),
                Values = TanEleVolatevalue
            };
            LcCurrentVolate = new SeriesCollection { };
            LcCurrentVolate.Add(t1);
            XFormatter = val => (val).ToString("N2") + "ms";
            YFormatter = val => (val).ToString("N2") + " V";
        }

        private void UIElement_OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            WorkingSets.local.IsCompeleteSaveWave = true;
        }
    }


}
