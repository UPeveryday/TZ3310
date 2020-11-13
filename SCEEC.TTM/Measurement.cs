using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCEEC.MI.TZ3310;
using System.Data;
using System.Windows;

namespace SCEEC.TTM
{
    public static class Measurement
    {
        public static void DoWork(ref TestingWorkerSender sender)
        {

            MeasurementInterface.DoWork(ref sender);
            //只有完成才会进去下一相
            if ((sender.MeasurementItems[sender.CurrentItemIndex].completed) && (!sender.MeasurementItems[sender.CurrentItemIndex].failed))
            {
                //WorkingSets.local.refreshTestResults();
                int ft = (int)sender.MeasurementItems[sender.CurrentItemIndex].Function;
                if ((ft > 0) && (ft < 10))
                    WorkingSets.local.TestResults.Rows.Add(sender.MeasurementItems[sender.CurrentItemIndex].ToDataRow(sender.job));
                WorkingSets.local.saveTestResults();
                if (sender.MeasurementItems.Length != (sender.CurrentItemIndex + 1))
                    sender.CurrentItemIndex++;
            }

            if ((sender.MeasurementItems[sender.CurrentItemIndex].completed) && (sender.MeasurementItems[sender.CurrentItemIndex].failed))
            {
                int ft = (int)sender.MeasurementItems[sender.CurrentItemIndex].Function;
                if ((ft > 0) && (ft < 10))
                    WorkingSets.local.TestResults.Rows.Add(sender.MeasurementItems[sender.CurrentItemIndex].ToDataRow(sender.job));
                WorkingSets.local.saveTestResults();
            }


            int id = sender.Transformer.ID; int jobid = sender.job.id; int code = sender.job.Information.GetHashCode(); string tname = sender.job.Information.testingName;
            WSCoreInsu wsc = null; WSLeakCur wsl = null; WSShortImp wss = null;
            if (sender.MeasurementItems[sender.CurrentItemIndex].Function == MeasurementFunction.Coreinsulation)
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    wsc = new WSCoreInsu(id, jobid, code, tname);
                    wsc.ShowDialog();
                });
                if (wsc != null)
                {
                    if (wsc.Confire)
                    {
                        sender.MeasurementItems[sender.CurrentItemIndex].completed = true;
                        sender.MeasurementItems[sender.CurrentItemIndex].failed = false;
                    }
                    else
                    {
                        sender.MeasurementItems[sender.CurrentItemIndex].completed = true;
                        sender.MeasurementItems[sender.CurrentItemIndex].failed = true;
                    }
                }

            }
            if (sender.MeasurementItems[sender.CurrentItemIndex].Function == MeasurementFunction.Leakagecurrent)
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    wsl = new WSLeakCur(id, jobid, code, tname);
                    wsl.ShowDialog();
                });
                if (wsl != null)
                {
                    if (wsl.Confire)
                    {
                        sender.MeasurementItems[sender.CurrentItemIndex].completed = true;
                        sender.MeasurementItems[sender.CurrentItemIndex].failed = false;
                    }
                    else
                    {
                        sender.MeasurementItems[sender.CurrentItemIndex].completed = true;
                        sender.MeasurementItems[sender.CurrentItemIndex].failed = true;
                    }
                }
            }
            if (sender.MeasurementItems[sender.CurrentItemIndex].Function == MeasurementFunction.Shortcircuitimpedance)
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    wss = new WSShortImp(id, jobid, code, tname);
                    wss.ShowDialog();
                });
                if (wss != null)
                {
                    if (wss.Confire)
                    {
                        sender.MeasurementItems[sender.CurrentItemIndex].completed = true;
                        sender.MeasurementItems[sender.CurrentItemIndex].failed = false;
                    }
                    else
                    {
                        sender.MeasurementItems[sender.CurrentItemIndex].completed = true;
                        sender.MeasurementItems[sender.CurrentItemIndex].failed = true;
                    }
                }
            }

        }
        public static bool CancelWork(ref TestingWorkerSender sender)
        {
            return MeasurementInterface.CancelWork(ref sender);
        }

    }
}
