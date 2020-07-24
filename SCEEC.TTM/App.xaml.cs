using log4net;
using SCEEC.MI.TZ3310;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace SCEEC.TTM
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        public static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        protected override void OnStartup(StartupEventArgs e)
        {
            log4net.Config.XmlConfigurator.Configure();
            base.OnStartup(e);
            //  WorkingSets.local.log = log;
            SCEEC.Data.Logger.log = log;
            log.Info(DateTime.Now.ToString()+ "==Startup=====================>>>");
        }
        protected override void OnExit(ExitEventArgs e)
        {
            log.Info(DateTime.Now.ToString() + "<<<========================End==");
            base.OnExit(e);
        }
    }
}

