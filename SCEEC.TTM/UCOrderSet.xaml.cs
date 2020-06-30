using SCEEC.MI.TZ3310;
using System;
using System.ComponentModel;
using System.Data;
using System.Windows;
using System.Windows.Controls;


namespace SCEEC.TTM
{
    /// <summary>
    /// UCOrderSet.xaml 的交互逻辑
    /// </summary>
    public partial class UCOrderSet : UserControl, INotifyPropertyChanged
    {
        public UCOrderSet()
        {
            InitializeComponent();


            // -------- dimmy ---------------
#if DEBUG
            WorkingSets.local.Connect();
            WorkingSets.local.CreateLocalDatabase(); ;
            Transformermassage = WorkingSets.local.Transformermassage;
#endif
            //-----------------------------

            this.DataContext = this;
        }
        private int transformerID;
        /// <summary>
        /// 变压器id
        /// </summary>
        public int TransformerID
        {
            get
            {
                return transformerID;
            }
            set
            {
                transformerID = value;
                GetViewData();
            }
        }
        /// <summary>
        /// 结果数据表
        /// </summary>
        public DataTable Transformermassage;
        /// <summary>
        /// 结果数据行
        /// </summary>
        public DataRow NewRowTransformermassage;
        /// <summary>
        /// 提交数据到数据库中
        /// </summary>
        public void SubmitData()
        {
            NewRowTransformermassage["transformerid"] = TransformerID;
            SCEEC.MI.TZ3310.WorkingSets.local.SaveCreateLocateDatabase();
        }
        /// <summary>
        /// 显示界面上参数值
        /// </summary>
        void GetViewData()
        {
            DataRow[] drArr = Transformermassage.Select($"transformerid='{TransformerID}'");
            if (drArr.Length > 0)
            {
                NewRowTransformermassage = drArr[drArr.Length - 1];
                HMLoadLoss = GetDrArrValueByField("theloadlosshv");
                HLLoadLoss = GetDrArrValueByField("theloadlossmv");
                MLLoadLoss = GetDrArrValueByField("theloadlosslv");
                HMImpVol = GetDrArrValueByField("impedancevoltagehv");
                HLImpVol = GetDrArrValueByField("impedancevoltagemv");
                MLImpVol = GetDrArrValueByField("impedancevoltagelv");
                NoLoadLoss = GetDrArrValueByField("noloadloss");
                NoLoadCur = GetDrArrValueByField("Noloadcurrent");
            }
            else
            {
                NewRowTransformermassage = Transformermassage.NewRow();
                Transformermassage.Rows.Add(NewRowTransformermassage);
            }
        }
        /// <summary>
        /// 数据转double
        /// </summary>
        /// <param name="drs">DataTable列</param>
        /// <param name="field">字段</param>
        /// <returns>数据值</returns>
        double GetDrArrValueByField(string field)
        {

            string strDr = NewRowTransformermassage[field].ToString();
            if (!string.IsNullOrEmpty(strDr))
            {
                return Convert.ToDouble(strDr);
            }
            return 0;
        }
        #region 界面不显示
        private Visibility _needShow;
        /// <summary>
        /// 界面是否显示
        /// </summary>
        public Visibility NeedShow
        {
            get { return _needShow; }
            set { _needShow = value; UpdateProperty(nameof(NeedShow)); }
        }
        #endregion
        #region 负载损耗
        private double _HMLoadLoss;
        /// <summary>
        /// 负载损耗（高-中）
        /// </summary>
        public double HMLoadLoss
        {
            get { return _HMLoadLoss; }
            set
            {
                _HMLoadLoss = value;
                UpdateProperty(nameof(HMLoadLoss));
                NewRowTransformermassage["theloadlosshv"] = value;
            }
        }
        private double _HLLoadLoss;
        /// <summary>
        /// 负载损耗（高-低）
        /// </summary>
        public double HLLoadLoss
        {
            get { return _HLLoadLoss; }
            set
            {
                _HLLoadLoss = value;
                UpdateProperty(nameof(HLLoadLoss));
                NewRowTransformermassage["theloadlossmv"] = value;
            }
        }
        private double _MLLoadLoss;
        /// <summary>
        /// 负载损耗（中-低）
        /// </summary>
        public double MLLoadLoss
        {
            get { return _MLLoadLoss; }
            set
            {
                _MLLoadLoss = value;
                UpdateProperty(nameof(MLLoadLoss));
                NewRowTransformermassage["theloadlosslv"] = value;
            }
        }
        #endregion
        #region 阻抗电压
        private double _HMImpVol;
        /// <summary>
        /// 阻抗电压（高-中）
        /// </summary>
        public double HMImpVol
        {
            get { return _HMImpVol; }
            set
            {
                _HMImpVol = value;
                UpdateProperty(nameof(HMImpVol));
                NewRowTransformermassage["impedancevoltagehv"] = value;
            }
        }
        private double _HLImpVol;
        /// <summary>
        /// 阻抗电压（高-低）
        /// </summary>
        public double HLImpVol
        {
            get { return _HLImpVol; }
            set
            {
                _HLImpVol = value;
                UpdateProperty(nameof(HLImpVol));
                NewRowTransformermassage["impedancevoltagemv"] = value;
            }
        }
        private double _MLImpVol;
        /// <summary>
        /// 阻抗电压（中-低）
        /// </summary>
        public double MLImpVol
        {
            get { return _MLImpVol; }
            set
            {
                _MLImpVol = value;
                UpdateProperty(nameof(MLImpVol));
                NewRowTransformermassage["impedancevoltagelv"] = value;
            }
        }
        #endregion
        #region 空载损耗
        private double _NoLoadLoss;
        /// <summary>
        /// 空载损耗
        /// </summary>
        public double NoLoadLoss
        {
            get { return _NoLoadLoss; }
            set
            {
                _NoLoadLoss = value;
                UpdateProperty(nameof(NoLoadLoss));
                NewRowTransformermassage["noloadloss"] = value;
            }
        }
        #endregion
        #region 空载电流
        private double _NoLoadCur;
        /// <summary>
        /// 空载电流
        /// </summary>
        public double NoLoadCur
        {
            get { return _NoLoadCur; }
            set
            {
                _NoLoadCur = value;
                UpdateProperty(nameof(NoLoadCur));
                NewRowTransformermassage["Noloadcurrent"] = value;
            }
        }
        #endregion
        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        private void UpdateProperty(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
