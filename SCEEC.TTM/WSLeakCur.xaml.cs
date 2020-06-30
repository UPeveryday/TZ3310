using Newtonsoft.Json;
using SCEEC.MI.TZ3310;
using System;
using System.Data;
using System.Windows;
using System.Windows.Input;


namespace SCEEC.TTM
{
    /// <summary>
    /// WSLeakCur.xaml 的交互逻辑
    /// </summary>
    public partial class WSLeakCur : Window
    {
        public WSLeakCur()
        {
            InitializeComponent();
            this.DataContext = _LeakCurJson;
            TestResults = SCEEC.MI.TZ3310.WorkingSets.local.TestResults;
            // -------- dimmy ---------------
#if DEBUG
            WorkingSets.local.Connect();
            WorkingSets.local.refreshTestResults();
            TestResults = WorkingSets.local.TestResults;
#endif
            //-----------------------------
            NewRowTestResults = TestResults.NewRow();

        }
        #region 上层模块传参方式
        private SCEEC.TTM.LeakCurJson _LeakCurJson = new SCEEC.TTM.LeakCurJson();
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
      
        public WSLeakCur(int transformerid, int mj_id, int testid, string testname) : this()
        {
            this.transformerid = transformerid;
            this.mj_id = mj_id;
            this.testid = testid;
            this.testname = testname;
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
            NewRowTestResults["function"] = 12;
            NewRowTestResults["failed"] = 0;
            NewRowTestResults["completed"] = 1;
            NewRowTestResults["recordtime"] = DateTime.Now;
            NewRowTestResults["windingtype"] = 0;
            NewRowTestResults["windingconfig"] = 0;
            NewRowTestResults["waves"] = JsonConvert.SerializeObject(_LeakCurJson);
            WorkingSets.local.TestResults.Rows.Add(NewRowTestResults);
            SCEEC.MI.TZ3310.WorkingSets.local.saveTestResults();
            this.Close();
            this.Confire = true;
        }
    }
}
