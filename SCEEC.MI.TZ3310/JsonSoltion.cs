using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCEEC.MI.TZ3310
{
    public class JsonSoltion
    {
        public static string Getjsonstr(byte[] jsons, ref int Flag)
        {
            string jsondata = Encoding.Default.GetString(jsons);
            if (jsondata.StartsWith("Location"))
            {
                Flag = -1;
                Location lc = new Location();
                try
                {
                    lc = JsonConvert.DeserializeObject<Location>(jsondata.Remove(0, 8));
                    if (LocationCheck(ref lc))
                    {
                        AddLocation(lc);
                        Flag = (int)Errorsymbol.AddLocationOk;
                        return jsondata.Remove(0, 8);
                    }
                    else
                    {
                        Flag = (int)Errorsymbol.AddLocationFalse;
                        return null;
                        //AddLocation(lc);
                    }

                }
                catch
                {
                    Flag = (int)Errorsymbol.AddLocationFalse;
                    return null;
                }

            }
            if (jsondata.StartsWith("Transformer"))
            {
                Flag = -1;
                Transformer lc = new Transformer();
                try
                {
                    lc = JsonConvert.DeserializeObject<Transformer>(jsondata.Remove(0, 11));
                    if (Errorsymbol.True == TransformerCheck(ref lc))
                    {

                        AddTransformer(lc);
                        Flag = (int)Errorsymbol.AddTransformerOk;
                        return jsondata.Remove(0, 11);

                    }
                    else
                    {
                        Flag = (int)Errorsymbol.AddTransformerFalse;
                        //AddTransformer(lc);
                        return null;
                    }

                }
                catch
                {

                    Flag = (int)Errorsymbol.AddTransformerFalse;
                    return null;
                }

            }
            if (jsondata.StartsWith("JobList"))
            {
                Flag = -1;
                JobList job = new JobList();
                try
                {
                    job = JsonConvert.DeserializeObject<JobList>(jsondata.Remove(0, 7));
                    if (Errorsymbol.True == JoblistCheck(ref job))
                    {
                        saveJob(job);
                        Flag = (int)Errorsymbol.AddJobListOk;
                        return jsondata.Remove(0, 7);

                    }
                    else
                    {
                        Flag = (int)Errorsymbol.AddJobListFalse;
                        return null;
                    }
                }
                catch
                {
                    Flag = Flag = (int)Errorsymbol.AddJobListFalse;
                    return null;

                }

            }
            if (jsondata.StartsWith("JobTest"))
            {
                Flag = -1;
                return Encoding.Default.GetString(jsons);
            }
            return null;
        }
        public static bool LocationCheck(ref Location lc)
        {
            if (lc.name == null)
                lc.name = "--未指定--";
            if (lc.company == null)
                lc.company = "--未指定--";
            if (lc.address == null)
                lc.address = "--未指定--";
            if (lc.operatorName == null)
                lc.operatorName = "--未指定--";
            List<int> Ids = WorkingSets.local.getAllLocationID();
            int[] array = Ids.ToArray();
            foreach (var item in Ids)
            {
                if (item == lc.id)
                {
                    Array.Sort(array);
                    lc.id = array[array.Length - 1] + 1;
                    return false;
                }
            }

            return true;
        }
        public static Errorsymbol TransformerCheck(ref Transformer trs, Errorsymbol err = Errorsymbol.True)
        {
            if (trs.SerialNo == null)
            {
                return Errorsymbol.TransformerSerialNoNull;
            }
            if (trs.Location == null)
            {
                return Errorsymbol.TransformerLocationNull;
            }
            else
            {
                if (WorkingSets.local.getLocation(trs.Location).name != trs.Location)
                    return Errorsymbol.TransformerLocationNull;
            }
            if (trs.ApparatusID == null)
            {
                return Errorsymbol.TransformerApparatusIDNull;
            }
            if (trs.Manufacturer == null)
            {

                return Errorsymbol.TransformerManufacturerNull;
            }
            if (trs.ProductionYear == null)
            {
                return Errorsymbol.TransformerProductionYearNull;
            }
            if (trs.AssetSystemCode == null)
            {
                return Errorsymbol.TransformerAssetSystemCodeNull;
            }
            if (trs.WindingNum == 3)
            {
                if (trs.VoltageRating.HV == 0 || trs.VoltageRating.MV == 0 || trs.VoltageRating.LV == 0)
                {
                    return Errorsymbol.TransformerVoltageRatingNull;
                }
                if (trs.PowerRating.HV == 0 || trs.PowerRating.MV == 0 || trs.PowerRating.LV == 0)
                {
                    return Errorsymbol.TransformerPowerRatingNull;
                }
            }
            else if (trs.WindingNum == 2)
            {
                if (trs.VoltageRating.HV == 0 || trs.VoltageRating.MV == 0)
                {
                    return Errorsymbol.TransformerVoltageRatingNull;
                }
                if (trs.PowerRating.HV == 0 || trs.PowerRating.MV == 0)
                {
                    return Errorsymbol.TransformerPowerRatingNull;
                }
            }
            else
            {
                return Errorsymbol.TransformerWindingNumFalse;
            }
            if (trs.RatingFrequency != 50 && trs.RatingFrequency != 60)
            {
                return Errorsymbol.TransformerRatingFrequencyNull;
            }
            if (trs.PhaseNum == 3)
            {
                if (trs.WindingConfig.HV != TransformerWindingConfigName.Y &&
               trs.WindingConfig.HV != TransformerWindingConfigName.D &&
               trs.WindingConfig.HV != TransformerWindingConfigName.Yn &&
               trs.WindingConfig.HV != TransformerWindingConfigName.Zn &&
               trs.WindingConfig.MV != TransformerWindingConfigName.Y &&
               trs.WindingConfig.MV != TransformerWindingConfigName.D &&
               trs.WindingConfig.MV != TransformerWindingConfigName.Yn &&
               trs.WindingConfig.MV != TransformerWindingConfigName.Zn &&
                trs.WindingConfig.LV != TransformerWindingConfigName.Y &&
               trs.WindingConfig.LV != TransformerWindingConfigName.D &&
               trs.WindingConfig.LV != TransformerWindingConfigName.Yn &&
               trs.WindingConfig.LV != TransformerWindingConfigName.Zn &&
               trs.WindingConfig.MVLabel < 0 && trs.WindingConfig.MVLabel > 11 &&
                trs.WindingConfig.LVLabel < 0 && trs.WindingConfig.LVLabel > 11)
                {
                    return Errorsymbol.TransformerWindingConfigFalse;

                }
            }
            else if (trs.PhaseNum == 1)
            {
            }
            else
            {
                return Errorsymbol.TransformerPhaseNumFalse;

            }
            if (trs.Bushing.HVContained != true && trs.Bushing.HVContained != false &&
                trs.Bushing.MVContained != true && trs.Bushing.MVContained != false)
            {
                return Errorsymbol.TransformerBushingFalse;

            }
            if (trs.OLTC.Contained)
            {
                if (trs.OLTC.WindingPositions == 0)
                {
                    trs.OLTC.WindingPosition = WindingType.HV;
                }
                else if (trs.OLTC.WindingPositions == 1)
                {
                    trs.OLTC.WindingPosition = WindingType.MV;
                }
                else if (trs.OLTC.WindingPositions == 2)
                {
                    trs.OLTC.WindingPosition = WindingType.LV;
                }
                else
                {
                    return Errorsymbol.TransformerOLTCWindingPositionsFalse;
                }
                if (trs.OLTC.SerialNo == null)
                {
                    return Errorsymbol.TransformerOLTCSerialNoNull;
                }
                if (trs.OLTC.ModelType == null)
                {
                    return Errorsymbol.TransformerOLTCModelTypeNull;
                }
                if (trs.OLTC.Manufacturer == null)
                {
                    return Errorsymbol.TransformerOLTCManufacturerNull;
                }
                if (trs.OLTC.ProductionYear == null)
                {
                    return Errorsymbol.TransformerOLTCProductionYearNull;
                }
                if (trs.OLTC.Step != 0.005 && trs.OLTC.Step != 0.01 && trs.OLTC.Step != 0.0125 &&
                    trs.OLTC.Step != 0.02 && trs.OLTC.Step != 0.025 && trs.OLTC.Step != 0.05 && trs.OLTC.Step != 0.1)
                {
                    return Errorsymbol.TransformerOLTCStepNull;
                }
                if (trs.OLTC.TapMainNum != 1 && trs.OLTC.TapMainNum != 3)
                {
                    return Errorsymbol.TransformerOLTCTapMainNumNull;

                }
                if (trs.OLTC.TapNum != -1 && trs.OLTC.TapNum != 1 && trs.OLTC.TapNum != 2 &&
                    trs.OLTC.TapNum != 3 && trs.OLTC.TapNum != 4 && trs.OLTC.TapNum != 5 && trs.OLTC.TapNum != 0)
                {
                    return Errorsymbol.TransformerOLTCTapNumNull;
                }
            }
            else
            {
                trs.OLTC.TapMainNum = 0;
                trs.OLTC.WindingPositions = 0;
                trs.OLTC.TapNum = 0;
                trs.OLTC.SerialNo = null;
                trs.OLTC.ModelType = null;
                trs.OLTC.Manufacturer = null;
                trs.OLTC.ProductionYear = null;
                trs.OLTC.Step = 0;
            }
            // trs.OLTC.WindingPosition = WindingType.HV;


            return Errorsymbol.True;
        }
        public static Errorsymbol JoblistCheck(ref JobList job, Errorsymbol err = Errorsymbol.True)
        {
            if (job.Name == null)
                return Errorsymbol.JobNameNull;
            if (Errorsymbol.True != TransformerCheck(ref job.Transformer))
            {
                return TransformerCheck(ref job.Transformer);
            }
            if (job.DCInsulation.HVEnabled != true && job.DCInsulation.HVEnabled != false &&
                job.DCInsulation.MVEnabled != true && job.DCInsulation.MVEnabled != false &&
                job.DCInsulation.LVEnabled != true && job.DCInsulation.LVEnabled != false &&
                job.DCInsulation.ZcEnable != true && job.DCInsulation.ZcEnable != false &&
                job.DCInsulation.Enabled != true && job.DCInsulation.Enabled != false)
            {
                return Errorsymbol.JobDCInsulationNull;
            }
            if (job.Capacitance.HVEnabled != true && job.Capacitance.HVEnabled != false &&
               job.Capacitance.MVEnabled != true && job.Capacitance.MVEnabled != false &&
               job.Capacitance.LVEnabled != true && job.Capacitance.LVEnabled != false &&
               job.Capacitance.ZcEnable != true && job.Capacitance.ZcEnable != false &&
               job.Capacitance.Enabled != true && job.Capacitance.Enabled != false)
            {
                return Errorsymbol.JobCapacitanceNull;
            }
            if (job.DCResistance.HVEnabled != true && job.DCResistance.HVEnabled != false &&
               job.DCResistance.MVEnabled != true && job.DCResistance.MVEnabled != false &&
               job.DCResistance.LVEnabled != true && job.DCResistance.LVEnabled != false &&
               job.DCResistance.ZcEnable != true && job.DCResistance.ZcEnable != false &&
               job.DCResistance.Enabled != true && job.DCResistance.Enabled != false)
            {
                return Errorsymbol.JobDCResistanceNull;
            }

            if (job.Bushing.DCInsulation != true && job.Bushing.DCInsulation != false &&
               job.Bushing.Capacitance != true && job.Bushing.Capacitance != false
               )
            {
                return Errorsymbol.JobBushingNull;
            }

            if (job.OLTC.DCResistance != true && job.OLTC.DCResistance != false &&
              job.OLTC.SwitchingCharacter != true && job.OLTC.SwitchingCharacter != false &&
              job.OLTC.Enabled != true && job.OLTC.Enabled != false && job.OLTC.Range < 1
              )
            {
                return Errorsymbol.JobBushingNull;
            }
            if (VolateIsOk(job.Parameter.DCInsulationVoltage, 0) != -1)
                job.Parameter.DCInsulationVoltage = VolateIsOk(job.Parameter.DCInsulationVoltage, 0);
            else
                return Errorsymbol.JobParameterDCInsulationVoltageNull;
            if (job.Parameter.DCInsulationResistance < 0)
            {
                return Errorsymbol.JobParameterDCInsulationResistanceNull;
            }
            if (job.Parameter.DCInsulationAbsorptionRatio < 0)
                return Errorsymbol.JobParameterDCInsulationAbsorptionRatioNull;


            if (VolateIsOk(job.Parameter.CapacitanceVoltage, 2) != -1)
                job.Parameter.CapacitanceVoltage = VolateIsOk(job.Parameter.CapacitanceVoltage, 2);
            else
                return Errorsymbol.JobParameterCapacitanceVoltageNull;
            if (job.Parameter.DCResistanceCurrent != 5 &&
                job.Parameter.DCResistanceCurrent != 10 &&
                job.Parameter.DCResistanceCurrent != 15 &&

                job.Parameter.DCHvResistanceCurrent != 5 &&
                job.Parameter.DCHvResistanceCurrent != 10 &&
                job.Parameter.DCHvResistanceCurrent != 15 &&

                job.Parameter.DCMvResistanceCurrent != 5 &&
                job.Parameter.DCMvResistanceCurrent != 10 &&
                job.Parameter.DCMvResistanceCurrent != 15 &&

                 job.Parameter.DCLvResistanceCurrent != 5 &&
                job.Parameter.DCLvResistanceCurrent != 10 &&
                job.Parameter.DCLvResistanceCurrent != 15)
            {
                return Errorsymbol.JobParameterDCResistanceCurrentNull;
            }
            if (VolateIsOk(job.Parameter.BushingDCInsulationVoltage, 0) != -1)
                job.Parameter.BushingDCInsulationVoltage = VolateIsOk(job.Parameter.BushingDCInsulationVoltage, 0);
            else
                return Errorsymbol.JobParameterBushingDCInsulationVoltageNull;
            if (VolateIsOk(job.Parameter.BushingCapacitanceVoltage, 2) != -1)
                job.Parameter.BushingCapacitanceVoltage = VolateIsOk(job.Parameter.BushingCapacitanceVoltage, 2);
            else
                return Errorsymbol.JobParameterBushingCapacitanceVoltageNull;

            if (job.Information.testingTime == null)
            {
                return Errorsymbol.JobInformationtestingTimeNull;
            }
            if (job.Information.testingName == null)
            {
                return Errorsymbol.JobInformationtestingNameNull;
            }
            if (job.Information.tester == null)
            {
                return Errorsymbol.JobInformationtesterNull;
            }
            if (job.Information.testingAgency == null)
            {
                return Errorsymbol.JobInformationtestingAgencyNull;
            }
            if (job.Information.auditor == null)
            {
                return Errorsymbol.JobInformationauditorNull;
            }
            if (job.Information.approver == null)
            {
                return Errorsymbol.JobInformationapproverNull;
            }
            if (job.Information.weather == null)
            {
                return Errorsymbol.JobInformationweatherNull;
            }
            if (job.Information.temperature == null)
            {
                return Errorsymbol.JobInformationtemperatureNull;
            }
            if (job.Information.humidity == null)
            {
                return Errorsymbol.JobInformationhumidityNull;
            }
            if (job.Information.principal == null)
            {
                return Errorsymbol.JobInformationprincipalNull;
            }

            return Errorsymbol.True;
        }
        public static int VolateIsOk(double data, int kind)
        {
            if (kind == 0)
            {
                if (data > 150 && data < 400)
                    return 250;
                else if (data >= 400 && data < 700)
                    return 500;
                else if (data >= 700 && data < 1300)
                    return 1000;
                else if (data >= 1300 && data < 1700)
                    return 1500;
                else if (data >= 1700 && data < 2200)
                    return 2000;
                else if (data >= 2200 && data < 2700)
                    return 2500;
                else if (data >= 2700 && data < 3200)
                    return 3000;
                else if (data >= 3200 && data < 3700)
                    return 3500;
                else if (data >= 3700 && data < 4500)
                    return 4000;
                else if (data >= 4500 && data < 6300)
                    return 5000;
                else if (data >= 6300 && data < 8000)
                    return 7500;
                else if (data >= 8000 && data <= 15000)
                    return 10000;
                else
                    return -1;
            }
            else
            {
                if (data > 350 && data < 750)
                    return 500;
                else if (data >= 750 && data < 1250)
                    return 1000;
                else if (data >= 1250 && data < 1750)
                    return 1500;
                else if (data >= 1750 && data < 2250)
                    return 2000;
                else if (data >= 2250 && data < 2750)
                    return 2500;
                else if (data >= 2750 && data < 3250)
                    return 3000;
                else if (data >= 3250 && data < 3750)
                    return 3500;
                else if (data >= 3750 && data < 4250)
                    return 4000;
                else if (data >= 4250 && data < 4750)
                    return 4500;
                else if (data >= 4750 && data < 5250)
                    return 5000;
                else if (data >= 5250 && data < 5750)
                    return 5500;
                else if (data >= 5750 && data < 6250)
                    return 6000;
                else if (data >= 6250 && data < 6750)
                    return 6500;
                else if (data >= 6750 && data < 7250)
                    return 7000;
                else if (data >= 7250 && data < 7750)
                    return 7500;
                else if (data >= 7750 && data < 8250)
                    return 8000;
                else if (data >= 8250 && data < 8750)
                    return 8500;
                else if (data >= 8750 && data < 9250)
                    return 9000;
                else if (data >= 9250 && data < 9750)
                    return 9500;
                else if (data >= 9750 && data < 13000)
                    return 10000;
                else
                    return -1;
            }


        }
        public static void AddLocation(Location lc)
        {
            WorkingSets.local.deleteLocation(lc.name);
            WorkingSets.local.addLocation(name: lc.name, company: lc.company, address: lc.address, operatorName: lc.operatorName, id: lc.id);
        }
        public static void AddTransformer(Transformer trs)
        {
            DataRow[] rows = WorkingSets.local.Transformers.Select("serialno = '" + trs.SerialNo.Trim() + "'");
            DataRow r = WorkingSets.local.Transformers.NewRow();
            if (rows.Length > 0)
                r = rows[0];
            else
                r = WorkingSets.local.Transformers.NewRow();
            bool previewSave = WorkingSets.local.saveTransformer();
            r["serialno"] = trs.SerialNo;
            r["location"] = trs.Location;
            r["apparatusid"] = trs.ApparatusID;
            r["manufacturer"] = trs.Manufacturer;
            r["productionyear"] = trs.ProductionYear;
            r["assetsystemcode"] = trs.AssetSystemCode;
            r["phases"] = trs.PhaseNum;
            r["windings"] = trs.WindingNum;
            r["ratedfrequency"] = trs.RatingFrequency;
            r["windingconfig_hv"] = (int)trs.WindingConfig.HV;
            r["windingconfig_mv"] = (int)trs.WindingConfig.MV;
            r["windingconfig_mv_label"] = (int)trs.WindingConfig.MVLabel;
            r["windingconfig_lv"] = (int)trs.WindingConfig.LV;
            r["windingconfig_lv_label"] = (int)trs.WindingConfig.LVLabel;
            r["voltageratinghv"] = (int)trs.VoltageRating.HV;
            r["voltageratingmv"] = (int)trs.VoltageRating.MV;
            r["voltageratinglv"] = (int)trs.VoltageRating.LV;
            r["powerratinghv"] = (int)trs.PowerRating.HV;
            r["powerratingmv"] = (int)trs.PowerRating.MV;
            r["powerratinglv"] = (int)trs.PowerRating.LV;
            r["bushing_hv_enabled"] = trs.Bushing.HVContained;
            r["bushing_mv_enabled"] = trs.Bushing.MVContained;
            r["coupling"] = trs.Coupling;
            if (trs.OLTC.Contained)
            {
                r["oltc_winding"] = (int)trs.OLTC.WindingPosition;
                r["oltc_tapmainnum"] = trs.OLTC.TapMainNum;
                r["oltc_tapnum"] = trs.OLTC.TapNum;
                r["oltc_step"] = getstep(trs.OLTC.Step);
                r["oltc_serialno"] = trs.OLTC.SerialNo;
                r["oltc_modeltype"] = trs.OLTC.ModelType;
                r["oltc_manufacturer"] = trs.OLTC.Manufacturer;
                r["oltcproductionyear"] = trs.OLTC.ProductionYear;
            }
            else
            {
                r["oltc_winding"] = 0;
                r["oltc_tapmainnum"] = 0;
                r["oltc_tapnum"] = -1;
                r["oltc_serialno"] = string.Empty;
                r["oltc_modeltype"] = string.Empty;
                r["oltc_manufacturer"] = string.Empty;
                r["oltcproductionyear"] = string.Empty;
            }
            r["id"] = 1;
            if (rows.Length > 0) r.EndEdit();
            else WorkingSets.local.Transformers.Rows.Add(r);
            WorkingSets.local.saveTransformer();
        }
        private static bool saveJob(JobList job)
        {
            DataRow[] rows = WorkingSets.local.Jobs.Select("TransformerSerialNo = '" + job.Transformer.SerialNo + "' and JobName = '" + job.Name + "'");
            DataRow r;
            if (rows.Length > 0)
                r = rows[0];
            else
                r = WorkingSets.local.Jobs.NewRow();
            WorkingSets.local.saveJobs();
            r["TransformerSerialNo"] = job.Transformer.SerialNo;
            r["JobName"] = job.Name;
            r["WindingDCInsulation"] = job.DCInsulation.Enabled;
            r["HVWindingDCInsulation"] = job.DCInsulation.HVEnabled;
            r["MVWindingDCInsulation"] = job.DCInsulation.MVEnabled;
            r["LVWindingDCInsulation"] = job.DCInsulation.LVEnabled;
            r["WindingCapacitance"] = job.Capacitance.Enabled;
            r["HVWindingCapacitance"] = job.Capacitance.HVEnabled;
            r["MVWindingCapacitance"] = job.Capacitance.MVEnabled;
            r["LVWindingCapacitance"] = job.Capacitance.LVEnabled;
            r["WindingDCResistance"] = job.DCResistance.Enabled;
            r["HVWindingDCResistance"] = job.DCResistance.HVEnabled;
            r["MVWindingDCResistance"] = job.DCResistance.MVEnabled;
            r["LVWindingDCResistance"] = job.DCResistance.LVEnabled;
            r["zcenable"] = job.DCResistance.ZcEnable;
            r["BushingDCInsulation"] = job.Bushing.DCInsulation;
            r["BushingCapacitance"] = job.Bushing.Capacitance;
            r["OLTC"] = job.OLTC.Enabled;
            r["OLTCRangeTextBox"] = (int)(job.OLTC.Range);
            r["OLTCDCResistance"] = job.OLTC.DCResistance;
            r["OLTCSwitching"] = job.OLTC.SwitchingCharacter;
            r["dci_voltage"] = job.Parameter.DCInsulationVoltage;
            r["dci_resistance"] = job.Parameter.DCInsulationResistance;
            r["dci_ar"] = job.Parameter.DCInsulationAbsorptionRatio;
            r["cap_voltage"] = job.Parameter.CapacitanceVoltage;
            r["dcr_current"] = job.Parameter.DCResistanceCurrent;
            r["dchvcurrent"] = job.Parameter.DCHvResistanceCurrent;
            r["dcmvcurrent"] = job.Parameter.DCMvResistanceCurrent;
            r["dclvcurrent"] = job.Parameter.DCLvResistanceCurrent;
            r["bushing_cap_voltage"] = job.Parameter.BushingCapacitanceVoltage;
            r["bushing_dci_voltage"] = job.Parameter.BushingDCInsulationVoltage;
            if (rows.Length > 0) r.EndEdit();
            else WorkingSets.local.Jobs.Rows.Add(r);
            return WorkingSets.local.saveJobs();
        }
        public static int getstep(double num)
        {
            //trs.OLTC.Step != 0.005 && trs.OLTC.Step != 0.01 && trs.OLTC.Step != 0.0125 &&
            //        trs.OLTC.Step != 0.02 && trs.OLTC.Step != 0.025 && trs.OLTC.Step != 0.05 && trs.OLTC.Step != 0.1)
            if (num == 0.005)
                return 0;
            else if (num == 0.01)
                return 1;
            else if (num == 0.0125)
                return 2;
            else if (num == 0.02)
                return 3;
            else if (num == 0.025)
                return 4;
            else if (num == 0.05)
                return 5;
            else if (num == 0.1)
                return 6;
            else
                return -1;

        }
        public static string GetJsonByclass<T>(T t)
        {
            JsonSerializerSettings jsetting = new JsonSerializerSettings()
            {
                NullValueHandling = NullValueHandling.Include,
                StringEscapeHandling = StringEscapeHandling.EscapeNonAscii,
            };
            return JsonConvert.SerializeObject(t, Formatting.Indented, jsetting);
        }
    }
    public enum Errorsymbol
    {
        True = 0,
        TransformerSerialNoNull = 1,
        TransformerLocationNull = 2,
        TransformerApparatusIDNull = 3,
        TransformerManufacturerNull = 4,
        TransformerProductionYearNull = 5,
        TransformerAssetSystemCodeNull = 6,
        TransformerWindingNumFalse = 7,
        TransformerWindingConfigFalse = 8,
        TransformerRatingFrequencyNull = 9,
        TransformerVoltageRatingNull = 10,
        TransformerPhaseNumFalse = 11,
        TransformerPowerRatingNull = 12,
        TransformerBushingFalse = 13,
        TransformerOLTCWindingPositionsFalse = 14,
        TransformerOLTCSerialNoNull = 15,
        TransformerOLTCManufacturerNull = 16,
        TransformerOLTCModelTypeNull = 17,
        TransformerOLTCProductionYearNull = 18,
        TransformerOLTCStepNull = 19,
        TransformerOLTCTapMainNumNull = 20,
        TransformerOLTCTapNumNull = 21,
        JobNameNull = 22,
        JobDCInsulationNull = 23,
        JobCapacitanceNull = 24,
        JobDCResistanceNull = 25,
        JobBushingNull = 26,
        JobParameterDCInsulationVoltageNull = 27,
        JobParameterDCInsulationResistanceNull = 28,
        JobParameterDCInsulationAbsorptionRatioNull = 29,
        JobParameterCapacitanceVoltageNull = 30,
        JobParameterDCResistanceCurrentNull = 31,
        JobParameterBushingDCInsulationVoltageNull = 32,
        JobParameterBushingCapacitanceVoltageNull = 33,
        JobInformationtestingTimeNull = 34,
        JobInformationtestingNameNull = 35,
        JobInformationtesterNull = 36,
        JobInformationtestingAgencyNull = 37,
        JobInformationauditorNull = 38,
        JobInformationapproverNull = 39,
        JobInformationweatherNull = 40,
        JobInformationtemperatureNull = 41,
        JobInformationhumidityNull = 42,
        JobInformationprincipalNull = 43,
        AddLocationOk = 44,
        AddLocationOkChangeId = 45,
        AddLocationFalse = 46,
        AddTransformerOk = 47,
        AddTransformerFalse = 48,
        AddJobListOk = 49,
        AddJobListFalse = 50,
        StartTestFalse=51,

    }
}
