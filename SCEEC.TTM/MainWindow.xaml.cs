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
using SCEEC.MI.TZ3310;
using System.Data;
using System.IO;
using Newtonsoft.Json;
using SCEEC.NET.TCPSERVER;

namespace SCEEC.TTM
{

    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();


            (new SplashScreen()).ShowDialog();
            //(new LoginWindow()).ShowDialog();

            LocationListBox.ItemsSource = WorkingSets.local.getLocationName();
            TransformerListBox.ItemsSource = WorkingSets.local.getTransformerSerialNo();
            StartTcp();
        }
        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        public bool TcpIsRunning { get; set; } = false;
        public bool StartTcp()
        {
            if (!TcpIsRunning)
            {
                TcpTask.TcpServer.CloseAllClient();//待测试
                TcpTask.TcpServer.DataReceived += TcpServer_DataReceived;
                TcpTask.TcpServer.ClientConnected += TcpServer_ClientConnected;
                TcpTask.TcpServer.ClientDisconnected += TcpServer_ClientDisconnected;
                TcpTask.TcpServer.CompletedSend += TcpServer_CompletedSend;
                TcpIsRunning = true;
            }
            return true;

        }

        private void TcpServer_CompletedSend(object sender, AsyncEventArgs e)
        {
            // throw new NotImplementedException();
        }

        private void TcpServer_ClientDisconnected(object sender, AsyncEventArgs e)
        {
            // throw new NotImplementedException();
        }

        private void TcpServer_ClientConnected(object sender, AsyncEventArgs e)
        {
            //  throw new NotImplementedException();
        }
        public int Flag = -1;
        private void TcpServer_DataReceived(object sender, AsyncEventArgs e)
        {
            byte[] a = e._state.Buffer;
            int length = e._state.RecLength;
            var Temp = a.Skip(0).Take(length).ToArray();
            if (Encoding.Default.GetString(Temp) == "Flag")
            {
                SendBytcp(new byte[] { (byte)Flag });
            }
            SendDataToclient();
            string jsondata = JsonSoltion.Getjsonstr(Temp, ref Flag);
            if (null != jsondata && Flag == -1)
            {
                RefreshUi(jsondata);
                WorkingSets.local.updateJob();
                StartTcptest(jsondata);
            }
            else
            {
                SendBytcp(new byte[] { (byte)Flag });
            }
        }

        public bool SendBytcp(byte[] data)
        {
            try
            {
                var temp = TcpTask.TcpServer._clients.ToArray();
                foreach (var p in temp)
                {
                    TcpTask.TcpServer.Send((TCPClientState)p, data);
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
        public void RefreshUi(string jsondata)
        {
            RefreshLocation();
            Refreshtrs(jsondata);
        }

        public void StartTcptest(string jsondata)
        {
            if (jsondata.StartsWith("JobTest"))
            {
                TcpJobTest tct = JsonConvert.DeserializeObject<TcpJobTest>(jsondata.Remove(0, 7));
                if (tct.JobName != null && tct.TransformerID != null)
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        WindowTesting windowTesting = new WindowTesting(tct.TransformerID, tct.JobName, tct.Information, istcp: true);
                        if (windowTesting.inited == true)
                        {
                            windowTesting.Show();
                            windowTesting.Outstata += WindowTesting_Outstata;
                            windowTesting.StartButton_Click(null, null);
                        }
                    });
                }
                else
                {
                    var temp = TcpTask.TcpServer._clients.ToArray();
                    foreach (var p in temp)
                    {
                        TcpTask.TcpServer.Send((TCPClientState)p, new byte[] { (int)Errorsymbol.StartTestFalse });
                    }
                }
            }
        }

        private void WindowTesting_Outstata(TestingWorkerSender stata)
        {
            //throw new NotImplementedException();
        }

        public void Refreshtrs(string jsondata)
        {
            try
            {
                Transformer ts = JsonConvert.DeserializeObject<Transformer>(jsondata);
                Application.Current.Dispatcher.Invoke(() =>
                {
                    TransformerListBox.ItemsSource = refreshTransformerList(ts.Location);
                });
            }
            catch
            {
                // throw new Exception("TransformerListBoxUpdata");
            }

        }
        /// <summary>
        /// 测试用例Location
        /// </summary>
        public void SendDataToclient()
        {
            var temp = TcpTask.TcpServer._clients.ToArray();
            foreach (var p in temp)
            {
                #region
                Transformer ts = new Transformer()
                {
                    SerialNo = "SerialNo",
                    Location = "方巷",
                    ApparatusID = "ApparatusID",
                    Manufacturer = "Manufacturer",
                    ProductionYear = "ProductionYear",
                    AssetSystemCode = "AssetSystemCode",
                    MethonID = "MethonID",
                    WindingNum = 3,
                    WindingConfig = new TransformerWindingConfigStruct
                    {
                        HV = TransformerWindingConfigName.Yn,
                        MV = TransformerWindingConfigName.Yn,
                        LV = TransformerWindingConfigName.D,
                        MVLabel = 0,
                        LVLabel = 11
                    },
                    RatingFrequency = 50,
                    PhaseNum = 3,
                    VoltageRating = new TransformerRatingStruct
                    {
                        HV = 220,
                        MV = 110,
                        LV = 35
                    },
                    PowerRating = new TransformerRatingStruct
                    {
                        HV = 240,
                        MV = 240,
                        LV = 120
                    },
                    Bushing = new BushingStruct
                    {
                        HVContained = true,
                        MVContained = true
                    },
                    OLTC = new OLTCStruct
                    {
                        Contained = true,
                        TapMainNum = 1,
                        WindingPositions = 0,
                        TapNum = 1,
                        SerialNo = "SerialNo",
                        ModelType = "ModelType",
                        Manufacturer = "Manufacturer",
                        ProductionYear = "ProductionYear",
                        Step = 0.02
                    },
                    Coupling = false

                };
                var trs = Encoding.Default.GetBytes("Transformer" + JsonSoltion.GetJsonByclass(ts));
                var q = Encoding.Default.GetBytes("Location" + JsonSoltion.GetJsonByclass(new Location() { company = "上海思创", address = "鹿吉路401", name = "变压器001", operatorName = "xw" }));
                var job = Encoding.Default.GetBytes("JobList" + JsonSoltion.GetJsonByclass(
                    new JobList
                    {
                        Name = "testname",
                        Transformer = ts,
                        DCInsulation = new WindingJobList
                        {
                            HVEnabled = true,
                            MVEnabled = true,
                            LVEnabled = true,
                            ZcEnable = true,
                            Enabled = false
                        },
                        Capacitance = new WindingJobList
                        {
                            HVEnabled = true,
                            MVEnabled = true,
                            LVEnabled = true,
                            ZcEnable = true,
                            Enabled = true
                        },
                        DCResistance = new WindingJobList
                        {
                            HVEnabled = true,
                            MVEnabled = true,
                            LVEnabled = true,
                            ZcEnable = true,
                            Enabled = true
                        },
                        Bushing = new BushingJobList
                        {
                            DCInsulation = true,
                            Capacitance = true
                        },
                        OLTC = new OLTCJobList
                        {
                            Range = 1,
                            DCResistance = true,
                            SwitchingCharacter = true,
                            Enabled = true
                        },
                        Parameter = new JobParameter
                        {
                            DCInsulationVoltage = 1000,
                            DCInsulationResistance = 3,
                            DCInsulationAbsorptionRatio = 1.3,
                            CapacitanceVoltage = 1000,
                            DCResistanceCurrent = 15,
                            DCHvResistanceCurrent = 5,
                            DCLvResistanceCurrent = 5,
                            DCMvResistanceCurrent = 5,
                            BushingCapacitanceVoltage = 1000,
                            BushingDCInsulationVoltage = 1000
                        },
                        Information = new JobInformation
                        {
                            testingTime = DateTime.Now,
                            oilTemperature = 20.6,
                            testingName = "InformationtestingName",
                            tester = "xuwei",
                            testingAgency = "testingAgency",
                            auditor = "auditor",
                            approver = "approver",
                            weather = "weather",
                            temperature = "25",
                            humidity = "50",
                            principal = "principal"
                        }
                    }
                    ));
                #endregion
                var str = Encoding.Default.GetBytes("JobTest" + JsonSoltion.GetJsonByclass(new TcpJobTest
                {
                    TransformerID = ts.SerialNo,
                    JobName = "testname",
                    Information = new JobInformation
                    {
                        testingTime = DateTime.Now,
                        oilTemperature = 20.6,
                        testingName = "InformationtestingName",
                        tester = "xuwei",
                        testingAgency = "testingAgency",
                        auditor = "auditor",
                        approver = "approver",
                        weather = "weather",
                        temperature = "25",
                        humidity = "50",
                        principal = "principal"
                    }
                }));


                TcpTask.TcpServer.Send((TCPClientState)p, trs);
                TcpTask.TcpServer.Send((TCPClientState)p, q);
                TcpTask.TcpServer.Send((TCPClientState)p, job);
                TcpTask.TcpServer.Send((TCPClientState)p, str);
            }
        }


        public void RefreshLocation()
        {
            WorkingSets.local.refreshLocations();
            List<string> locations = WorkingSets.local.getLocationName();
            Application.Current.Dispatcher.Invoke(() =>
            {
                LocationListBox.ItemsSource = locations;
            });

            //List<string> sn = refreshTransformerList();
            //TransformerListBox.ItemsSource = sn;
            //if (transformerHelper.serialno != string.Empty)
            //    TransformerListBox.SelectedIndex = sn.IndexOf(transformerHelper.serialno);
            //else TransformerListBox.SelectedIndex = -1;
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

        private void LocationListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.LocationListBox.SelectedIndex >= 0)
            {
                ModifyLocationButton.Visibility = Visibility.Visible;
                WorkingSets.local.updateTransformer();
                TransformerListBox.ItemsSource = WorkingSets.local.getTransformerSerialNo(LocationListBox.SelectedItem.ToString());
            }
            else
            {
                WorkingSets.local.updateTransformer();
                ModifyLocationButton.Visibility = Visibility.Hidden;
                TransformerListBox.ItemsSource = WorkingSets.local.getTransformerSerialNo();
            }
        }

        private void NewLocationButton_Click(object sender, RoutedEventArgs e)
        {
            LocationSettingWindow locationHelper = new LocationSettingWindow();
            locationHelper.ShowDialog();
            WorkingSets.local.refreshLocations();
            List<string> locations = WorkingSets.local.getLocationName();
            LocationListBox.ItemsSource = locations;
            LocationListBox.SelectedIndex = locations.IndexOf(locationHelper.name);
        }

        private void ModifyLocationButton_Click(object sender, RoutedEventArgs e)
        {
            LocationSettingWindow locationHelper = new LocationSettingWindow(this.LocationListBox.SelectedItem.ToString());
            locationHelper.ShowDialog();
            WorkingSets.local.refreshLocations();
            List<string> locations = WorkingSets.local.getLocationName();
            LocationListBox.ItemsSource = locations;
            LocationListBox.SelectedIndex = locations.IndexOf(locationHelper.name);
        }

        private void LocationListBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

            if (LocationListBox.IsMouseCaptured)
                if (LocationListBox.SelectedIndex > -1)
                {
                    ModifyLocationButton_Click(null, null);
                    return;
                }
            LocationListBox.SelectedIndex = -1;
        }

        private List<string> refreshTransformerList()
        {
            List<string> sn = new List<string>();
            if (LocationListBox.SelectedIndex >= 0)
            {
                DataRow[] rows = WorkingSets.local.Transformers.Select("location = '" + WorkingSets.local.getLocationName()[LocationListBox.SelectedIndex] + "'");
                foreach (DataRow row in rows)
                {
                    sn.Add(row["serialno"].ToString());
                }
            }
            else
            {
                foreach (DataRow row in WorkingSets.local.Transformers.Rows)
                {
                    sn.Add(row["serialno"].ToString());
                }
            }
            return sn;
        }

        private List<string> refreshTransformerList(string trsname)
        {
            List<string> sn = new List<string>();
            LocationListBox.SelectedItem = trsname;
            WorkingSets.local.updateTransformer();
            DataRow[] rows = WorkingSets.local.Transformers.Select("location = '" + trsname + "'");
            foreach (DataRow row in rows)
            {
                sn.Add(row["serialno"].ToString());
            }
            WorkingSets.local.updateTransformer();
            return sn;
        }

        private void NewTransformerButton_Click(object sender, RoutedEventArgs e)
        {
            TransformerSettingWindow transformerHelper = new TransformerSettingWindow();
            transformerHelper.ShowDialog();
            List<string> sn = refreshTransformerList();
            TransformerListBox.ItemsSource = sn;
            if (transformerHelper.serialno != string.Empty)
                TransformerListBox.SelectedIndex = sn.IndexOf(transformerHelper.serialno);
            else TransformerListBox.SelectedIndex = -1;
        }

        private void TransformerListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.TransformerListBox.SelectedIndex >= 0)
            {
                ModifyTransformerButton.Visibility = Visibility.Visible;
                NewJobButton.IsEnabled = true;
                NewJobButton.Opacity = 1;
            }
            else
            {
                ModifyTransformerButton.Visibility = Visibility.Hidden;
                NewJobButton.IsEnabled = false;
                NewJobButton.Opacity = 0.3;
            }
            JobListBox.ItemsSource = new List<string>();
            TestListBox.ItemsSource = new List<string>();
            refreshJobList();
        }

        private int[] JobListBoxlist;
        private void refreshJobList()
        {
            if (this.TransformerListBox.SelectedIndex > -1)
            {
                JobListBox.ItemsSource = WorkingSets.local.getJobNames(TransformerListBox.SelectedItem.ToString());
                JobListBoxlist = WorkingSets.local.getJobNameids(TransformerListBox.SelectedItem.ToString()).ToArray();
            }
            else
            {
                JobListBox.ItemsSource = new List<string>();
            }
        }

        private void TransformerListBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (TransformerListBox.IsMouseCaptured)
                if (TransformerListBox.SelectedIndex > -1)
                {
                    ModifyTransformerButton_Click(null, null);
                    return;
                }
            TransformerListBox.SelectedIndex = -1;
        }

        private void ModifyTransformerButton_Click(object sender, RoutedEventArgs e)
        {
            TransformerSettingWindow transformerHelper = new TransformerSettingWindow(TransformerListBox.SelectedItem.ToString());
            transformerHelper.ShowDialog();
            List<string> sn = refreshTransformerList();
            TransformerListBox.ItemsSource = sn;
            if (transformerHelper.serialno != string.Empty)
                TransformerListBox.SelectedIndex = sn.IndexOf(transformerHelper.serialno);
            else TransformerListBox.SelectedIndex = -1;
        }

        private void NewJobButton_Click(object sender, RoutedEventArgs e)
        {
            if (TransformerListBox.SelectedIndex > -1)
            {
                JobSettingWindow jobHelper = new JobSettingWindow(TransformerListBox.SelectedItem.ToString());
                jobHelper.ShowDialog();
                refreshJobList();
            }
        }

        private void NewTestButton_Click(object sender, RoutedEventArgs e)
        {
            if (TransformerListBox.SelectedIndex < 0) return;
            if (JobListBox.SelectedIndex < 0) return;
            WindowTesting windowTesting = new WindowTesting(TransformerListBox.SelectedItem.ToString(), JobListBox.SelectedItem.ToString(), new JobInformation());
            if (windowTesting.inited == true) windowTesting.ShowDialog();
        }

        private void JobListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.JobListBox.SelectedIndex >= 0)
            {
                ModifyJobButton.Visibility = Visibility.Visible;
                NewTestButton.IsEnabled = true;
                NewTestButton.Opacity = 1;
                TestListBox.ItemsSource = WorkingSets.local.getTestResultsFromJobID(WorkingSets.local.getJob(TransformerListBox.SelectedItem.ToString(), JobListBox.SelectedItem.ToString()).id);
            }
            else
            {
                ModifyJobButton.Visibility = Visibility.Hidden;
                NewTestButton.IsEnabled = false;
                NewTestButton.Opacity = 0.3;
            }
        }

        private void JobListBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (JobListBox.IsMouseCaptured)
                if (JobListBox.SelectedIndex > -1)
                {
                    ModifyJobButton_Click(null, null);
                    return;
                }
            JobListBox.SelectedIndex = -1;
        }

        private void ModifyJobButton_Click(object sender, RoutedEventArgs e)
        {
            if ((TransformerListBox.SelectedIndex > -1) && (JobListBox.SelectedIndex > -1))
            {
                JobSettingWindow jobHelper = new JobSettingWindow(TransformerListBox.SelectedItem.ToString(), JobListBox.SelectedItem.ToString());
                jobHelper.ShowDialog();
                refreshJobList();
                JobListBox.SelectedIndex = JobListBox.Items.IndexOf(jobHelper.jobName);
            }
        }

        private void ExportButton_Click(object sender, RoutedEventArgs e)
        {
            InsertDataTodatabase.UpdataDatabase(TestListBox.SelectedItem.ToString());/* }*/
            InsertDataTodatabase.ShowExport(TestListBox.SelectedItem.ToString());
        }

        private void ListBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                ExportButton.IsEnabled = true;
                ExportButton.Opacity = 1;
                if (TestListBox.IsMouseCaptured)
                    if (TestListBox.SelectedIndex > -1)
                    {
                        TestButton_Click(null, null);
                        return;
                    }
                TestListBox.SelectedIndex = -1;
            }
            catch
            {
                MessageBox.Show("未发现报告，请检查!", "ERROR");
            }

        }



        private void TestListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.TestListBox.SelectedIndex >= 0)
            {
                List<string> ReportNames = new List<string>();
                string reportname = TestListBox.SelectedItem.ToString();
                string[] tpd = reportname.Split('(');
                ReportNames.Add("报告： " + tpd[0]);
                NewReportButton.IsEnabled = true;
                NewReportButton.Opacity = 1;
                ReportListBox.ItemsSource = ReportNames.ToArray();
                ReportNames.Clear();
            }
            else
            {
                ReportListBox.ItemsSource = "";
                NewReportButton.IsEnabled = false;
                NewReportButton.Opacity = 0.3;
            }
        }

        private void TestButton_Click(object sender, RoutedEventArgs e)
        {
            if ((TestListBox.SelectedIndex > -1) && (JobListBox.SelectedIndex > -1))
            {
                ExportDataWindow Exp = new ExportDataWindow(TestListBox.SelectedItem.ToString());
                Exp.ShowDialog();
            }
        }

        private void NewReportButton_click(object sender, RoutedEventArgs e)
        {
            ExportButton_Click(null, null);
        }

        private void Fbutton_click(object sender, RoutedEventArgs e)
        {
            // HNReport.DoReport.Run(HNReport.ReportOperator.Design, "88888888", "配电变压器");
            //JsonSerializerSettings jsetting = new JsonSerializerSettings()
            //{
            //    NullValueHandling = NullValueHandling.Include,
            //    StringEscapeHandling = StringEscapeHandling.EscapeNonAscii,
            //};
            //var datatablejson = JsonConvert.SerializeObject(WorkingSets.local.Getjobs(), Formatting.Indented, jsetting);
            //var trans = JsonConvert.SerializeObject(WorkingSets.local.Transformers, Formatting.Indented, jsetting);
            //var local = JsonConvert.SerializeObject(WorkingSets.local.Locations, Formatting.Indented, jsetting);
            //JobList ts = JsonConvert.DeserializeObject<JobList>(datatablejson);


            WorkingSets.local.getTransformer(13);
        }

        private void ExportList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                InsertDataTodatabase.ShowExport(TestListBox.SelectedItem.ToString());

            }
            catch
            {
                MessageBox.Show("打开报告错误");
            }
        }

        private void ExportList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (ReportListBox.IsMouseCaptured)
                if (ReportListBox.SelectedIndex > -1)
                {
                    ExportButton_Click(null, null);
                    return;
                }
            ReportListBox.SelectedIndex = -1;
        }

        private void deleteTransform_click(object sender, RoutedEventArgs e)
        {
            if (TransformerListBox.SelectedIndex > -1)
            {
                WorkingSets.local.DeleteDataBase("Tz3310", "transformer", "serialno", TransformerListBox.SelectedItem.ToString());
                if (LocationListBox.Items.Count > 0)
                {
                    LocationListBox.SelectedIndex = 0;
                    WorkingSets.local.updateTransformer();
                    refreshTransformerList();
                    TransformerListBox.ItemsSource = WorkingSets.local.getTransformerSerialNo(LocationListBox.SelectedItem.ToString());
                }
            }
        }

        private void deleteTransforeTask(object sender, RoutedEventArgs e)
        {
            if (TransformerListBox.SelectedIndex > -1)
            {
                WorkingSets.local.DeleteDataBase("Tz3310", "transformer", "serialno", TransformerListBox.SelectedItem.ToString());
                if (LocationListBox.Items.Count > 0)
                {
                    LocationListBox.SelectedIndex = 0;
                    WorkingSets.local.updateTransformer();
                    refreshTransformerList();
                    TransformerListBox.ItemsSource = WorkingSets.local.getTransformerSerialNo(LocationListBox.SelectedItem.ToString());
                }
            }
        }

        private void deletelocationTask(object sender, RoutedEventArgs e)
        {
            if (LocationListBox.SelectedIndex > -1)
            {
                WorkingSets.local.DeleteDataBase("Tz3310", "location", "name", LocationListBox.SelectedItem.ToString());
                WorkingSets.local.updateLocation();
                LocationListBox.ItemsSource = WorkingSets.local.getLocationName();

            }
        }

        private void jobdelete(object sender, RoutedEventArgs e)
        {
            if (JobListBox.SelectedIndex > -1)
            {
                WorkingSets.local.DeleteDataBase("Tz3310", "measurementjob", "id", JobListBoxlist[JobListBox.SelectedIndex].ToString());
                if (TransformerListBox.Items.Count > 0)
                {
                    TransformerListBox.SelectedIndex = 0;
                    WorkingSets.local.updateJob();
                    refreshJobList();
                    //JobListBox.ItemsSource = WorkingSets.local.getJobs(TransformerListBox.SelectedItem.ToString());
                }
            }
        }

        private void TestListItemdelete(object sender, RoutedEventArgs e)
        {
            if (TestListBox.SelectedIndex > -1)
            {

                var data = TestListBox.SelectedItem.ToString().Split('=')[1].Replace(")", "").Trim();
                WorkingSets.local.DeleteDataBase("Tz3310", "testresult", "id", data);

                //  WorkingSets.local.DeleteDatabseS("Tz3310", "testresult", "testname", TestListBox.SelectedItem.ToString().Split('(')[0], data[1].Replace(")", "").Trim());
                if (JobListBox.Items.Count > 0)
                {
                    JobListBox.SelectedIndex = 0;
                    WorkingSets.local.refreshTestResults();
                    if (this.JobListBox.SelectedIndex > -1)
                    {
                        TestListBox.ItemsSource = WorkingSets.local.getTestResultsFromJobID(WorkingSets.local.getJob(TransformerListBox.SelectedItem.ToString(), JobListBox.SelectedItem.ToString()).id);
                    }
                    else
                    {
                        TestListBox.ItemsSource = new List<string>();
                    }
                }
            }
        }

        private void LocationListBox_TouchUp(object sender, TouchEventArgs e)
        {
            LocationListBox_MouseDoubleClick(null, null);
        }

        private void TransformerListBox_TouchUp(object sender, TouchEventArgs e)
        {
            TransformerListBox_MouseDoubleClick(null, null);
        }

        private void JobListBox_TouchUp(object sender, TouchEventArgs e)
        {
            JobListBox_MouseDoubleClick(null, null);
        }

        private void TestListBox_TouchUp(object sender, TouchEventArgs e)
        {
            ListBox_MouseDoubleClick(null, null);
        }

        private void ReportListBox_TouchUp(object sender, TouchEventArgs e)
        {
            ExportButton_Click(null, null);
        }
    }
}
