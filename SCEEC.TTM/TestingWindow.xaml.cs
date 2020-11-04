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
        BackgroundWorker TestingWorker;

        JobList currentJob;
        TestingWorkerSender worker;

        public WindowTesting(string transformerSerialNo, string jobName, JobInformation job, int testID = -1, bool istcp = false)
        {
            InitializeComponent();
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
                            if (!WorkingSets.local.Testwave)
                            {
                                SetgroupboxVisible(Whichgroupbox.NONE);
                                var data = ParsingData.deelwaves(status.MeasurementItems[status.CurrentItemIndex].Result.waves);
                                chartWave.shortWave = data[0];
                                chartWave2.shortWave = data[1];
                                chartWave3.shortWave = data[2];
                                WorkingSets.local.Testwave = true;
                            }
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
            //WorkingStatusLabel.Text = "已经停止测量，\t\n如需重测请点击开始测量";
            //WorkingSets.local.IsCancer = true;
            //TestingWorker.CancelAsync();
            string name = worker.MeasurementItems[2].TapLabel[0] + "-" + worker.MeasurementItems[2].TapLabel[1];
            SaveFrameworkElementToImage(Oltcgroupbox, worker.job.Information.GetHashCode().ToString(), name);
        }

        private void ConfireIsOk_Click(object sender, MouseButtonEventArgs e)
        {
            if (worker.MeasurementItems[worker.CurrentItemIndex].Function == MeasurementFunction.OLTCSwitchingCharacter)
            {
                WorkingSets.local.IsVisible = false;
                ConfireIsOk.Visibility = Visibility.Collapsed;
                worker.MeasurementItems[worker.CurrentItemIndex].Result.values =
                    new PhysicalVariable[] { chartWave.R1Ret, chartWave.R2Ret, chartWave.R1AndR2Ret,
                        chartWave2.R1Ret, chartWave2.R2Ret, chartWave2.R1AndR2Ret, chartWave3.R1Ret,
                        chartWave3.R2Ret, chartWave2.R1AndR2Ret };
                string name = worker.MeasurementItems[worker.CurrentItemIndex].TapLabel[0] + "-" + worker.MeasurementItems[worker.CurrentItemIndex].TapLabel[1];
                SaveFrameworkElementToImage(Oltcgroupbox, worker.job.Information.GetHashCode().ToString(), name);

                WorkingSets.local.IsCompeleteSaveWave = true;
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
            string fs = "C:\\waveimgs\\"+ filelocation;
            if (!File.Exists(fs))
            {
                Directory.CreateDirectory(fs);
            }
            File.Copy(filename, fs + "\\" + filename + ".jpg", true);
        }
    }


}
