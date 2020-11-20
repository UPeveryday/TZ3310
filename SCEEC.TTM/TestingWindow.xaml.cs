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
using System.IO;
using ChartsWave;

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

        public bool Started = false;
        BackgroundWorker TestingWorker;

        JobList currentJob;
        TestingWorkerSender worker;

        public WindowTesting(string transformerSerialNo, string jobName, JobInformation job, int testID = -1, bool istcp = false)
        {
            InitializeComponent();

            Started = false;
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
        private void TestingWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            var worker = e.Argument as TestingWorkerSender;
            if (!Started)
            {
                WorkingSets.local.TestResults.Rows.Add(worker.job.Information.ToDataRow(worker.job));
                WorkingSets.local.saveTestResults();
                Started = true;
            }
            while (worker.CurrentItemIndex < worker.MeasurementItems.Length)
            {
                if (worker.CurrentItemIndex + 1 == worker.MeasurementItems.Length &&
                  worker.MeasurementItems[worker.CurrentItemIndex].completed == true)
                {
                    worker.ProgressPercent = (worker.MeasurementItems.Where(p => p.completed == true).Count()) * 100 / worker.MeasurementItems.Length;
                    TestingWorker.ReportProgress(worker.ProgressPercent, worker);
                    //   TestingWorker.ReportProgress(100, worker);
                    if (worker.MeasurementItems.Where(p => p.redo == true).Count() > 0)
                    {
                        if (reDoMark(worker)) continue;
                        else return;
                    }
                    else
                    {
                        return;
                    }
                }
                if (TestingWorker.CancellationPending == true)
                {
                    while (!Measurement.CancelWork(ref worker))
                    {
                        TestingWorker.ReportProgress(0, worker);
                        Thread.Sleep(50);
                    }
                    return;
                }
                else
                {
                    if (!worker.MeasurementItems[worker.CurrentItemIndex].completed)
                    {
                        Measurement.DoWork(ref worker);
                        showMassege(worker);
                    }
                    else
                    {
                        worker.CurrentItemIndex++;
                    }
                }
                var process = worker.MeasurementItems.Where(p => p.completed == true);
                worker.ProgressPercent = process.Count() * 100 / worker.MeasurementItems.Length;
                TestingWorker.ReportProgress(worker.ProgressPercent, worker);
                Thread.Sleep(100);
            }
        }

        public void Refresh()
        {
            StatusRefresh(worker);
        }

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
            {
                ThirdButton.Text = "确认稳定";
                dcrgroupbox.Visibility = Visibility.Visible;
            }
            if (num == Whichgroupbox.CAP)
                capgroupbox.Visibility = Visibility.Visible;
            if (num == Whichgroupbox.OLTC)
            {
                ThirdButton.Text = "确认波形";
                oltcgroupbox.Visibility = Visibility.Visible;
            }
            if (num == Whichgroupbox.NONE)
            {
                Oltcgroupbox.Visibility = Visibility.Visible;
            }

        }

        private bool reDoMark(TestingWorkerSender status)
        {
            bool? ret = null;
            Application.Current.Dispatcher.Invoke(() =>
            {
                ret = UMessageBox.Show("提示", "试验已经结束，\t\n是否重做标记的试验", "结束试验，不重做", firstButtonVisible: true, firstButtonMessage: "立即重做标记试验");
            });
            if (ret != null && !(bool)ret)
            {
                status.CurrentItemIndex = 0;
                return true;
            }
            return false;
        }

        private void showMassege(TestingWorkerSender status)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                var temp = status.MeasurementItems[status.CurrentItemIndex];
                if (temp.Function == MeasurementFunction.Description &&
               !string.IsNullOrEmpty(temp.needSwitchTapNum))
                {
                    var ret = UMessageBox.Show("提示", "该步骤需要人工完成\t\n" + "请将有载分接开关切换到（分接" + temp.needSwitchTapNum + "）位置", "确定", false);
                    if ((bool)ret)
                    {
                        var confireRet = UMessageBox.Show("提示", "确认已经切换至" + temp.needSwitchTapNum + "位置。\t\n立即进入试验?", "确定", false);
                        if ((bool)confireRet)
                            status.CurrentItemIndex++;
                    }
                }
                if (status.MeasurementItems[status.CurrentItemIndex].failed && status.MeasurementItems[status.CurrentItemIndex].completed == true)
                {
                    var ret = UMessageBox.Show("警告", "试验出错\t\n" + status.StatusText, "跳过");
                    if (!(bool)ret)
                    {
                        //重做
                        status.MeasurementItems[status.CurrentItemIndex].failed = false;
                        status.MeasurementItems[status.CurrentItemIndex].completed = false;
                        status.MeasurementItems[status.CurrentItemIndex].state = 0;
                        status.MeasurementItems[status.CurrentItemIndex].redo = true;
                        if (status.CurrentItemIndex > 0)
                            status.CurrentItemIndex--;
                        TestFunction.TZ3310.CommunicationQuery(0x00);
                    }
                    else
                    {
                        status.MeasurementItems[status.CurrentItemIndex].failed = true;
                        status.MeasurementItems[status.CurrentItemIndex].completed = true;
                        if (status.MeasurementItems.Length != status.CurrentItemIndex + 1)
                            status.CurrentItemIndex++;
                        TestFunction.TZ3310.CommunicationQuery(0x00);
                    }
                }
            });
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
            if (status.MeasurementItems[status.CurrentItemIndex].Result != null && status.MeasurementItems[status.CurrentItemIndex].Result.values != null)
            {
                if (status.MeasurementItems[status.CurrentItemIndex].Function == MeasurementFunction.DCResistance)
                {
                    SetgroupboxVisible(Whichgroupbox.DCR);
                    var testResult = status.MeasurementItems[status.CurrentItemIndex].Result;
                    if (testResult.values[2] != null && testResult.values[2].value != null)
                        A_resistance_dashboad.Value = (double)testResult.values[2].value;
                    if (testResult.values[1] != null && testResult.values[1].value != null)
                        A_current_dashboad.Value = (double)testResult.values[1].value;
                    if (testResult.values[2] != null && testResult.values[2].value != null)
                        A_resistance_value.Text = testResult.values[2].OriginText;
                    if (testResult.values[1] != null && testResult.values[1].value != null)
                        A_Current_value.Text = testResult.values[1].OriginText;
                    if (testResult.values[5] != null && testResult.values[5].value != null)
                        B_resistance_dashboad.Value = (double)testResult.values[5].value;
                    if (testResult.values[4] != null && testResult.values[4].value != null)
                        B_current_dashboad.Value = (double)testResult.values[4].value;
                    if (testResult.values[5] != null && testResult.values[5].value != null)
                        B_resistance_value.Text = testResult.values[5].OriginText;
                    if (testResult.values[4] != null && testResult.values[4].value != null)
                        B_Current_value.Text = testResult.values[4].OriginText;
                    if (testResult.values[8] != null && testResult.values[8].value != null)
                        C_resistance_dashboad.Value = (double)testResult.values[8].value;
                    if (testResult.values[7] != null && testResult.values[7].value != null)
                        C_current_dashboad.Value = (double)testResult.values[7].value;
                    if (testResult.values[8] != null && testResult.values[8].value != null)
                        C_resistance_value.Text = testResult.values[8].OriginText;
                    if (testResult.values[7] != null && testResult.values[7].value != null)
                        C_Current_value.Text = testResult.values[7].OriginText;
                }
                if (status.MeasurementItems[status.CurrentItemIndex].Function == MeasurementFunction.DCInsulation ||
                    status.MeasurementItems[status.CurrentItemIndex].Function == MeasurementFunction.BushingDCInsulation)
                {
                    SetgroupboxVisible(Whichgroupbox.DCI);
                    var testResult = status.MeasurementItems[status.CurrentItemIndex].Result;
                    if (testResult.values[0] != null && testResult.values[0].value != null)
                    {
                        dciboard_volate.Value = (double)(testResult.values[0].value / 1000);
                        dciboard_volate_value.Text = testResult.values[0].OriginText;
                    }
                    if (testResult.values[1] != null && testResult.values[1].value != null)
                    {
                        dciboard_resistance.Value = (double)(testResult.values[1].value / 1000000000);
                        dciboard_resistance_value.Text = testResult.values[1].OriginText + "Ω";
                    }
                }
                if (status.MeasurementItems[status.CurrentItemIndex].Function == MeasurementFunction.Capacitance ||
                   status.MeasurementItems[status.CurrentItemIndex].Function == MeasurementFunction.BushingCapacitance)
                {
                    SetgroupboxVisible(Whichgroupbox.CAP);
                    var testResult = status.MeasurementItems[status.CurrentItemIndex].Result;
                    if (testResult.values[1] != null && testResult.values[1].value != null)
                    {
                        captance.Value = (double)(testResult.values[1].value / 1000);
                        captancevalue.Text = testResult.values[1].OriginText;
                    }
                }
                if (status.MeasurementItems[status.CurrentItemIndex].Function == MeasurementFunction.OLTCSwitchingCharacter)
                {
                    var testResult = status.MeasurementItems[status.CurrentItemIndex].Result;

                    if (!WorkingSets.local.TestDCI)
                    {
                        SetgroupboxVisible(Whichgroupbox.OLTC);
                        WorkingSets.local.TestDCI = true;
                    }
                    if (testResult.values[1] != null && testResult.values[1].value != null)
                    {
                        Aoltcdashboard.Value = (double)testResult.values[1].value;
                        Aoltcavalue.Text = testResult.values[1].OriginText;
                    }
                    if (testResult.values[4] != null && testResult.values[4].value != null)
                    {
                        Boltcdashboard.Value = (double)testResult.values[4].value;
                        Boltcavalue.Text = testResult.values[4].OriginText;
                    }

                    if (testResult.values[7] != null && testResult.values[7].value != null)
                    {

                        Coltcdashboard.Value = (double)testResult.values[7].value;
                        Coltcavalue.Text = testResult.values[7].OriginText;
                    }
                    if (status.MeasurementItems[status.CurrentItemIndex].Result.waves != null)
                    {
                        if (!WorkingSets.local.Testwave)
                        {
                            SetgroupboxVisible(Whichgroupbox.NONE);
                            threechart.shortWave = status.MeasurementItems[status.CurrentItemIndex].Result.waves;
                            WorkingSets.local.Testwave = true;
                        }
                    }
                }
            }
            progressBar.Value = status.ProgressPercent;
            RemainingTestNumLabel.Text = status.RemainingItemsCount.ToString();
            WorkingSets.local.status = status;
            GC.Collect();
        }

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

        public void StartButton_Click(object sender, MouseButtonEventArgs e)
        {
            if (!TestingWorker.IsBusy)
            {
                TestingWorker.RunWorkerAsync(worker);
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Refresh();
        }
        private void StopButton_Click(object sender, MouseButtonEventArgs e)
        {
            WorkingStatusLabel.Text = "已经停止测量，\t\n如需重测请点击开始测量";
            WorkingSets.local.IsCancer = true;
            TestingWorker.CancelAsync();
        }

        private void ConfireIsOk_Click(object sender, MouseButtonEventArgs e)
        {
            if (worker.MeasurementItems[worker.CurrentItemIndex].Function == MeasurementFunction.OLTCSwitchingCharacter)
            {
                WorkingSets.local.IsVisible = false;
                ConfireIsOk.Visibility = Visibility.Collapsed;
                worker.MeasurementItems[worker.CurrentItemIndex].Result.values =
                    new PhysicalVariable[] { threechart.WaveResults[0].r1, threechart.WaveResults[0].r2, threechart.WaveResults[0].r12, threechart.WaveResults[1].r1,
                    threechart.WaveResults[1].r2, threechart.WaveResults[1].r12, threechart.WaveResults[2].r1, threechart.WaveResults[2].r2, threechart.WaveResults[2].r12, };
                string name = worker.MeasurementItems[worker.CurrentItemIndex].TapLabel[0] + "-" + worker.MeasurementItems[worker.CurrentItemIndex].TapLabel[1];
                SaveFrameworkElementToImage(Oltcgroupbox, worker.job.Information.GetHashCode().ToString(), name);

                WorkingSets.local.IsCompeleteSaveWave = true;

                //按下确认波形 就跳转到测量位置UI
                WorkingSets.local.TestDCI = false;

            }

            if (worker.MeasurementItems[worker.CurrentItemIndex].Function == MeasurementFunction.DCResistance)
            {
                WorkingSets.local.IsStable = true;
                WorkingSets.local.IsVisible = false;
                ConfireIsOk.Visibility = Visibility.Collapsed;
            }
        }

        public void SaveFrameworkElementToImage(FrameworkElement ui, string filelocation, string filename)
        {
            System.IO.FileStream ms = new System.IO.FileStream(filename, System.IO.FileMode.Create);
            System.Windows.Media.Imaging.RenderTargetBitmap bmp = new System.Windows.Media.Imaging.RenderTargetBitmap
                ((int)ui.ActualWidth, (int)ui.ActualHeight, 96d, 96d, System.Windows.Media.PixelFormats.Pbgra32);
            bmp.Render(ui);
            System.Windows.Media.Imaging.JpegBitmapEncoder encoder = new System.Windows.Media.Imaging.JpegBitmapEncoder();
            encoder.Frames.Add(System.Windows.Media.Imaging.BitmapFrame.Create(bmp));
            encoder.Save(ms);
            ms.Close();
            string fs = AppDomain.CurrentDomain.BaseDirectory + "\\waveimage\\" + filelocation;
            if (!File.Exists(fs))
            {
                Directory.CreateDirectory(fs);
            }
            File.Copy(filename, fs + "\\" + filename + ".jpg", true);
        }

        private void redo_Click(object sender, RoutedEventArgs e)
        {
            var index = TestItemListBox.SelectedIndex;
            if (worker.CurrentItemIndex > index)
            {
                worker.MeasurementItems[index].redo = true;
                worker.MeasurementItems[index].completed = false;
                worker.MeasurementItems[index].failed = false;
                worker.MeasurementItems[index].state = 0;
                TestItemListBox.ItemsSource = worker.GetList();
            }
        }


    }


}
