using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using SCEEC.Numerics;

namespace SCEEC.MI.TZ3310
{
    public static class MeasurementInterface
    {

        public static MeasurementResult DoMeasurement(ref MeasurementItemStruct mi, Transformer transformer, JobList Job, TestingWorkerSender sender)
        {
            switch (mi.Function)
            {
                case MeasurementFunction.DCInsulation:
                    TestFunction.DoDCInsulation(ref mi, transformer, Job);
                    break;
                case MeasurementFunction.Capacitance:
                    TestFunction.Capacitance(ref mi, transformer, Job);
                    break;
                case MeasurementFunction.DCResistance://直流电阻
                    TestFunction.DCResistance(ref mi, transformer, Job, sender);
                    break;
                case MeasurementFunction.BushingDCInsulation:
                    TestFunction.BushingDCInsulation(ref mi, transformer, Job);
                    break;
                case MeasurementFunction.BushingCapacitance:
                    TestFunction.BushingCapacitance(ref mi, transformer, Job);
                    break;
                case MeasurementFunction.OLTCSwitchingCharacter:
                    TestFunction.OLTCSwitchingCharacter(ref mi, transformer, Job);
                    break;
                case MeasurementFunction.Information:
                    TestFunction.Information(ref mi, transformer, Job);
                    break;
                case MeasurementFunction.DCCharge://充电
                    break;
                case MeasurementFunction.Coreinsulation://充电
                    TestFunction.Coredci(ref mi, transformer, Job);
                    break;
                case MeasurementFunction.Leakagecurrent://充电
                    TestFunction.Losscurrent(ref mi, transformer, Job);
                    break;
                case MeasurementFunction.Shortcircuitimpedance://充电
                    TestFunction.Shortvolate(ref mi, transformer, Job);
                    break;
                case MeasurementFunction.Description:
                    mi.completed = true;
                    break;
            }
            return null;
        }



        public static void DoWork(ref TestingWorkerSender sender)
        {
            if (WorkingSets.local.IsCancer)
            {
                sender.MeasurementItems[sender.CurrentItemIndex].completed = false;
                sender.MeasurementItems[sender.CurrentItemIndex].failed = false;
                sender.MeasurementItems[sender.CurrentItemIndex].state = 0;
                // sender.CurrentItemIndex = sender.CurrentItemIndex - 1;
                WorkingSets.local.IsCancer = false;
            }
            //  sender.MeasurementItems[sender.CurrentItemIndex].state = 0;
            DoMeasurement(ref sender.MeasurementItems[sender.CurrentItemIndex], sender.Transformer, sender.job, sender);
            sender.StatusText = sender.MeasurementItems[sender.CurrentItemIndex].stateText;
            if (sender.CurrentItemIndex >= sender.MeasurementItems.Length)
                TestFunction.Closecurrent(0);
        }


        public static bool CancelWork(ref TestingWorkerSender sender)
        {
            return TestFunction.CancalWork(ref sender);
        }
    }
}

