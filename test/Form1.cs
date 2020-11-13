using HNReport;
using SCEEC.MI.TZ3310;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace test
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            SCEEC.NET.SerialClass sc = new SCEEC.NET.SerialClass("COM35", 115200, System.IO.Ports.Parity.None, 8, System.IO.Ports.StopBits.One);


        }

        private void button1_Click(object sender, EventArgs e)
        {
          
        }
    }
}
