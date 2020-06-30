using SCEEC.MI.TZ3310;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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
namespace SCEEC.TTM
{
    /// <summary>
    /// WSShortImp.xaml 的交互逻辑
    /// </summary>
    public partial class WSShortImp : Window, INotifyPropertyChanged
    {
        public WSShortImp()
        {
            InitializeComponent();
            TestResults = SCEEC.MI.TZ3310.WorkingSets.local.TestResults;
            // -------- dimmy ---------------
#if DEBUG
            WorkingSets.local.Connect();
            WorkingSets.local.refreshTestResults();
            TestResults = WorkingSets.local.TestResults;
#endif
            //-----------------------------
            NewRowTestResults = TestResults.NewRow();
            this.DataContext = this;
        }
        #region 上层模块传参方式
        /// <summary>
        /// 结果数据表
        /// </summary>
        public DataTable TestResults;
        /// <summary>
        /// 结果数据行
        /// </summary>
        public DataRow NewRowTestResults;

        private int _transformerid;
        public int transformerid
        {
            get { return _transformerid; }
            set
            {
                _transformerid = value;
                NewRowTestResults["transformerid"] = value;
            }
        }
        private int _mj_id;
        public int mj_id
        {
            set
            {
                _mj_id = value;
                NewRowTestResults["mj_id"] = value;
            }
        }
        private int _testid;
        public int testid
        {
            set
            {
                _testid = value;
                NewRowTestResults["testid"] = value;
            }
        }
        private string _testname;
        public string testname
        {
            set
            {
                _testname = value;
                NewRowTestResults["testname"] = value;
            }
        }

        private string _InstName;
        /// <summary>
        /// 试验仪器
        /// </summary>
        public string InstName
        {
            get { return _InstName; }
            set
            {
                _InstName = value;
                UpdateProperty(nameof(InstName));
                NewRowTestResults["instname"] = value;
            }
        }

        private string _InstNum;
        /// <summary>
        /// 仪器编号
        /// </summary>
        public string InstNum
        {
            get { return _InstNum; }
            set
            {
                _InstNum = value; UpdateProperty(nameof(InstNum));
                NewRowTestResults["instnum"] = value;

            }
        }
        private Visibility _kbvisible = Visibility.Collapsed;

        public Visibility kbvisible
        {
            get
            {
                return _kbvisible;
            }
            set
            {
                _kbvisible = value;
                UpdateProperty(nameof(kbvisible));
            }
        }

        public bool kbckeck
        {
            get { return _kbckeck; }
            set
            {
                _kbckeck = value;
                UpdateProperty(nameof(kbckeck));
                if (_kbckeck == true)
                {
                    kbvisible = Visibility.Visible;
                }
                else
                    kbvisible = Visibility.Collapsed;

            }

        }

        private bool _kbckeck;

        public WSShortImp(int transformerid, int mj_id, int testid, string testname) : this()
        {
            this.transformerid = transformerid;
            this.mj_id = mj_id;
            this.testid = testid;
            this.testname = testname;
        }
        #endregion

        #region 界面UI
        private double _TestCur;
        /// <summary>
        /// 试验电流
        /// </summary>
        public double TestCur
        {
            get { return _TestCur; }
            set
            {
                _TestCur = value;
                UpdateProperty(nameof(TestCur));
                NewRowTestResults["result_pv1"] = value;
            }
        }

        private double _TestABVol;
        /// <summary>
        /// AB试验电压
        /// </summary>
        public double TestABVol
        {
            get { return _TestABVol; }
            set
            {
                _TestABVol = value;
                UpdateProperty(nameof(TestABVol));
                NewRowTestResults["result_pv2"] = value;
            }
        }

        private double _TestBCVol;
        /// <summary>
        /// BC试验电压
        /// </summary>
        public double TestBCVol
        {
            get { return _TestBCVol; }
            set
            {
                _TestBCVol = value;
                UpdateProperty(nameof(TestBCVol));
                NewRowTestResults["result_pv3"] = value;
            }
        }

        private double _TestCAVol;
        /// <summary>
        /// CA试验电压
        /// </summary>
        public double TestCAVol
        {
            get { return _TestCAVol; }
            set
            {
                _TestCAVol = value;
                UpdateProperty(nameof(TestCAVol));
                NewRowTestResults["result_pv4"] = value;
            }
        }

        private double _InitImp;
        /// <summary>
        /// 初始阻抗
        /// </summary>
        public double InitImp
        {
            get { return _InitImp; }
            set
            {
                _InitImp = value;
                UpdateProperty(nameof(InitImp));
                NewRowTestResults["result_pv5"] = value;
            }
        }

        private double _TestImp;
        /// <summary>
        /// 实测阻抗
        /// </summary>
        public double TestImp
        {
            get { return _TestImp; }
            set
            {
                _TestImp = value;
                UpdateProperty(nameof(TestImp));
                NewRowTestResults["result_pv6"] = value;
            }
        }
        #endregion 界面UI

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        private void UpdateProperty(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void btnCancle_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            this.Confire = false;
        }
        public bool Confire { get; set; } = false;

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            NewRowTestResults["function"] = 13;
            NewRowTestResults["failed"] = 0;
            NewRowTestResults["completed"] = 1;
            NewRowTestResults["windingtype"] = 0;
            NewRowTestResults["windingconfig"] = 0;
            NewRowTestResults["recordtime"] = DateTime.Now;
            WorkingSets.local.TestResults.Rows.Add(NewRowTestResults);
            SCEEC.MI.TZ3310.WorkingSets.local.saveTestResults();
            this.Close();

            this.Confire = true;
        }
    }
}
