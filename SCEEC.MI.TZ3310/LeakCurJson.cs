using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCEEC.TTM
{
    public class LeakCurJson : INotifyPropertyChanged
    {
        #region 界面UI
        private double _H_L10kV;
        /// <summary>
        /// 施加电压10kV
        /// </summary>
        public double H_L10kV
        {
            get { return _H_L10kV; }
            set
            {
                _H_L10kV = value;
                UpdateProperty(nameof(H_L10kV));
            }
        }

        private double _L_H10kV;
        /// <summary>
        /// 施加电压10kV
        /// </summary>
        public double L_H10kV
        {
            get { return _L_H10kV; }
            set
            {
                _L_H10kV = value;
                UpdateProperty(nameof(L_H10kV));
            }
        }

        private double _HL10kV;
        /// <summary>
        /// 施加电压10kV
        /// </summary>
        public double HL10kV
        {
            get { return _HL10kV; }
            set
            {
                _HL10kV = value;
                UpdateProperty(nameof(HL10kV));
            }
        }

        private double _H_L20kV;
        /// <summary>
        /// 施加电压10kV
        /// </summary>
        public double H_L20kV
        {
            get { return _H_L20kV; }
            set
            {
                _H_L20kV = value;
                UpdateProperty(nameof(H_L20kV));
            }
        }

        private double _L_H20kV;
        /// <summary>
        /// 施加电压10kV
        /// </summary>
        public double L_H20kV
        {
            get { return _L_H20kV; }
            set
            {
                _L_H20kV = value;
                UpdateProperty(nameof(L_H20kV));
            }
        }

        private double _HL20kV;
        /// <summary>
        /// 施加电压10kV
        /// </summary>
        public double HL20kV
        {
            get { return _HL20kV; }
            set
            {
                _HL20kV = value;
                UpdateProperty(nameof(HL20kV));
            }
        }

        private double _H_L40kV;
        /// <summary>
        /// 施加电压10kV
        /// </summary>
        public double H_L40kV
        {
            get { return _H_L40kV; }
            set
            {
                _H_L40kV = value;
                UpdateProperty(nameof(H_L40kV));
            }
        }

        private double _L_H40kV;
        /// <summary>
        /// 施加电压10kV
        /// </summary>
        public double L_H40kV
        {
            get { return _L_H40kV; }
            set
            {
                _L_H40kV = value;
                UpdateProperty(nameof(L_H40kV));
            }
        }

        private double _HL40kV;
        /// <summary>
        /// 施加电压10kV
        /// </summary>
        public double HL40kV
        {
            get { return _HL40kV; }
            set
            {
                _HL40kV = value;
                UpdateProperty(nameof(HL40kV));
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
                _InstNum = value;
                UpdateProperty(nameof(InstNum));
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

    }
}
