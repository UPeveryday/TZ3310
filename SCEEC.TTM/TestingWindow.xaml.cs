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
            //Task.Factory.StartNew(() =>
            //{
            //    while (WorkingSets.local.IsVisible1)
            //    {
            //        if (WorkingSets.local.IsVisible)
            //        {
            //            Dispatcher.Invoke(new Action(delegate
            //            {
            //                ConfireIsOk.Visibility = Visibility.Visible;
            //            }));
            //        }
            //        else
            //        {
            //            Dispatcher.Invoke(new Action(delegate
            //            {
            //                ConfireIsOk.Visibility = Visibility.Collapsed;
            //            }));
            //        }
            //    }
            //});
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

            //for (int i = 0; i < status.MeasurementItems.Length; i++)
            //{
            if (status.MeasurementItems[status.CurrentItemIndex].Result != null)
            {
                if (status.MeasurementItems[status.CurrentItemIndex].Result.values != null)
                {
                    if (status.MeasurementItems[status.CurrentItemIndex].Function == MeasurementFunction.DCResistance)
                    {
                        vn1.Text = "A相电压";
                        v1.Text = status.MeasurementItems[status.CurrentItemIndex].Result.values[0].OriginText;
                        vn2.Text = "A相电流";
                        v2.Text = status.MeasurementItems[status.CurrentItemIndex].Result.values[1].OriginText;
                        vn3.Text = "A相电阻";
                        v3.Text = status.MeasurementItems[status.CurrentItemIndex].Result.values[2].OriginText;

                        vn4.Text = "B相电压";
                        v4.Text = status.MeasurementItems[status.CurrentItemIndex].Result.values[3].OriginText;
                        vn5.Text = "B相电流";
                        v5.Text = status.MeasurementItems[status.CurrentItemIndex].Result.values[4].OriginText;
                        vn6.Text = "B相电阻";
                        v6.Text = status.MeasurementItems[status.CurrentItemIndex].Result.values[5].OriginText;

                        vn7.Text = "C相电压";
                        v7.Text = status.MeasurementItems[status.CurrentItemIndex].Result.values[6].OriginText;
                        vn8.Text = "C相电流";
                        v8.Text = status.MeasurementItems[status.CurrentItemIndex].Result.values[7].OriginText;
                        vn9.Text = "C相电阻";
                        v9.Text = status.MeasurementItems[status.CurrentItemIndex].Result.values[8].OriginText;
                    }
                    if (status.MeasurementItems[status.CurrentItemIndex].Function == MeasurementFunction.DCInsulation ||
                        status.MeasurementItems[status.CurrentItemIndex].Function == MeasurementFunction.BushingDCInsulation)
                    {
                        vn4.Text = "电压：";
                        v4.Text = status.MeasurementItems[status.CurrentItemIndex].Result.values[0].OriginText;
                        vn6.Text = "阻值：";
                        v6.Text = status.MeasurementItems[status.CurrentItemIndex].Result.values[1].OriginText;
                    }

                    if (status.MeasurementItems[status.CurrentItemIndex].Function == MeasurementFunction.Capacitance ||
                       status.MeasurementItems[status.CurrentItemIndex].Function == MeasurementFunction.BushingCapacitance)
                    {
                        vn4.Text = "电压：";
                        v4.Text = status.MeasurementItems[status.CurrentItemIndex].Result.values[1].OriginText;
                        vn5.Text = "频率：";
                        v5.Text = status.MeasurementItems[status.CurrentItemIndex].Result.values[0].OriginText;
                        vn6.Text = "";
                        v6.Text = "";
                    }
                    if (status.MeasurementItems[status.CurrentItemIndex].Function == MeasurementFunction.OLTCSwitchingCharacter)
                    {
                        vn1.Text = "A相电压";
                        v1.Text = status.MeasurementItems[status.CurrentItemIndex].Result.values[0].OriginText;

                        vn3.Text = "A相电流";
                        v3.Text = status.MeasurementItems[status.CurrentItemIndex].Result.values[1].OriginText;

                        vn4.Text = "B相电压";
                        v4.Text = status.MeasurementItems[status.CurrentItemIndex].Result.values[3].OriginText;

                        vn6.Text = "B相电流";
                        v6.Text = status.MeasurementItems[status.CurrentItemIndex].Result.values[4].OriginText;

                        vn7.Text = "C相电压";
                        v7.Text = status.MeasurementItems[status.CurrentItemIndex].Result.values[6].OriginText;

                        vn9.Text = "C相电流";
                        v9.Text = status.MeasurementItems[status.CurrentItemIndex].Result.values[7].OriginText;
                    }


                }
                // }
            }
            progressBar.Value = status.ProgressPercent;
            RemainingTestNumLabel.Text = status.RemainingItemsCount.ToString();
            WorkingSets.local.status = status;
            GC.Collect();
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
            if (!WaveThread.IsAlive)
            {
                WaveThread.IsBackground = true;
                WaveThread.Start();
            }

            if (!TestingWorker.IsBusy)
            {
                TestingWorker.RunWorkerAsync(worker);
                //if (worker.MeasurementItems[worker.CurrentItemIndex].Function == MeasurementFunction.Information)
                //    StarText.Text = "确认稳定";
                //else if (worker.MeasurementItems[worker.CurrentItemIndex].Function == MeasurementFunction.DCResistance)
                //    StarText.Text = "确认稳定";
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
    }


}
