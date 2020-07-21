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
using System.Windows.Shapes;
using SCEEC.MI.TZ3310;
using System.Data;
using System.Net.NetworkInformation;

namespace SCEEC.TTM
{
    /// <summary>
    /// TransformerSettingWindow.xaml 的交互逻辑
    /// </summary>
    public partial class TransformerSettingWindow : Window
    {

        bool changed = false;
        bool closeConfirmed = false;
        string originSerialNo = string.Empty;
        public string serialno = string.Empty;




        public void TransformerInfoInitial(string serialNo)
        {
            if (serialNo == string.Empty)
            {
                Title = "新变压器";
                return;
            }
            serialNo = serialNo.Trim();
            SerialNoTextBox.Text = serialNo;
            originSerialNo = serialNo;
            DataRow[] rows = WorkingSets.local.Transformers.Select("serialno = '" + serialNo + "'");
            DataRow[] rowm = WorkingSets.local.Transformermassage.Select("serialno = '" + serialNo + "'");
            if (rows.Length < 1)
            {
                Title = "新变压器";
                return;
            }
            DataRow r = rows[0];
            if (rowm.Length < 1)
            {
                Title = "新变压器";
                return;
            }
            DataRow rm = rowm[0];
            locationComboBox.SelectedIndex = locationComboBox.Items.IndexOf((string)r["location"]);
            ApparatusIDTextBox.Text = (string)r["apparatusid"];
            ManufacturerTextBox.Text = (string)r["manufacturer"];
            ProductionYearTextBox.Text = (string)r["productionyear"];
            AssetSystemCodeTextBox.Text = (string)r["assetsystemcode"];
            PhaseComboBox.SelectedIndex = ((int)r["phases"] == 3) ? 1 : 0;
            WindingNumComboBox.SelectedIndex = ((int)r["windings"] == 3) ? 1 : 0;
            RatingFrequencyComboBox.SelectedIndex = ((int)r["ratedfrequency"] == 50) ? 0 : 1;
            HvWindingConfigComboBox.SelectedIndex = (int)r["windingconfig_hv"];
            MvWindingConfigComboBox.SelectedIndex = (int)r["windingconfig_mv"];
            MvWindingLabelComboBox.SelectedIndex = (int)r["windingconfig_mv_label"];
            LvWindingConfigComboBox.SelectedIndex = (int)r["windingconfig_lv"];
            LvWindingLabelComboBox.SelectedIndex = (int)r["windingconfig_lv_label"];
            HvVoltageRatingTextBox.Text = ((double)r["voltageratinghv"]).ToString();
            MvVoltageRatingTextBox.Text = ((double)r["voltageratingmv"]).ToString();
            LvVoltageRatingTextBox.Text = ((double)r["voltageratinglv"]).ToString();
            HvPowerRatingTextBox.Text = ((double)r["powerratinghv"]).ToString();
            MvPowerRatingTextBox.Text = ((double)r["powerratingmv"]).ToString();
            LvPowerRatingTextBox.Text = ((double)r["powerratinglv"]).ToString();
            HvBushingCheckBox.IsChecked = (bool)r["bushing_hv_enabled"];
            MvBushingCheckBox.IsChecked = (bool)r["bushing_mv_enabled"];
            LvBushingCheckBox.IsChecked = (bool)r["bushing_lv_enabled"];
            if (HvBushingCheckBox.IsChecked == true)
            {
                if (r["hv_a_outfactory"] != null)
                    Hv_A_OutFactoryId.Text = (string)r["hv_a_outfactory"];
                if (r["hv_b_outfactory"] != null)
                    Hv_B_OutFactoryId.Text = (string)r["hv_b_outfactory"];
                if (r["hv_c_outfactory"] != null)
                    Hv_C_OutFactoryId.Text = (string)r["hv_c_outfactory"];
                if (r["hv_o_outfactory"] != null)
                    Hv_O_OutFactoryId.Text = (string)r["hv_o_outfactory"];
                if (r["hv_o_mp_enabled"] != null)
                    Hv_o_mp.IsChecked = (bool)r["hv_o_mp_enabled"];
            }
            if (MvBushingCheckBox.IsChecked == true)
            {
                if (r["mv_a_outfactory"] != null)
                    Mv_A_OutFactoryId.Text = (string)r["mv_a_outfactory"];
                if (r["mv_b_outfactory"] != null)
                    Mv_B_OutFactoryId.Text = (string)r["mv_b_outfactory"];
                if (r["mv_c_outfactory"] != null)
                    Mv_C_OutFactoryId.Text = (string)r["mv_c_outfactory"];
                if (r["mv_o_outfactory"] != null)
                    Mv_O_OutFactoryId.Text = (string)r["mv_o_outfactory"];
                if (r["mv_o_mp_enabled"] != null)
                Mv_O_HaveMp.IsChecked = (bool)r["mv_o_mp_enabled"];
            }
            if (LvBushingCheckBox.IsChecked == true)
            {
                if (r["lv_a_outfactory"] != null)
                Lv_A_OutFactoryId.Text = (string)r["lv_a_outfactory"];
                if (r["lv_b_outfactory"] != null)
                Lv_B_OutFactoryId.Text = (string)r["lv_b_outfactory"];
                if (r["lv_c_outfactory"] != null)
                Lv_C_OutFactoryId.Text = (string)r["lv_c_outfactory"];
                if (r["lv_o_outfactory"] != null)
                Lv_O_OutFactoryId.Text = (string)r["lv_o_outfactory"];
                if (r["lv_o_mp_enabled"] != null)
                Lv_o_mp.IsChecked = (bool)r["lv_o_mp_enabled"];
            }
            coulpcombobox.SelectedIndex = (int)r["coulplingindex"];

            if (r["coupling"] != null)
                coupling.IsChecked = (bool)r["coupling"];
            OLTCCheckBox.IsChecked = (((int)r["oltc_tapnum"]) > -1);
            if (OLTCCheckBox.IsChecked == true)
            {
                OLTCWindingComboBox.SelectedIndex = (int)r["oltc_winding"];
                OLTCTapNumComboBox.SelectedIndex = (int)r["oltc_tapnum"];
                OLTCMulTapNumComboBox.SelectedIndex = (int)r["oltc_multapnum"];
                OLTCStepComboBox.SelectedIndex = (int)r["oltc_step"];
                OLTCTapMainNumTextBox.Text = ((int)r["oltc_tapmainnum"]).ToString();
                OLTCSerialNoTextBox.Text = (string)r["oltc_serialno"];
                OLTCModelTypeTextBox.Text = (string)r["oltc_modeltype"];
                OLTCManufacturerTextBox.Text = (string)r["oltc_manufacturer"];
                OLTCProductionYearTextBox.Text = (string)r["oltcproductionyear"];
                OLTCTapMainNumLocationTextBox.Text = r["oltc_taplocation"].ToString();
            }

            HMImpVol.Text = Convertdata(rm["impedancevoltagehv"]);
            HLImpVol.Text = Convertdata(rm["impedancevoltagemv"]);
            MLImpVol.Text = Convertdata(rm["impedancevoltagelv"]);
            HMLoadLoss.Text = Convertdata(rm["theloadlosshv"]);
            HLLoadLoss.Text = Convertdata(rm["theloadlossmv"]);
            MLLoadLoss.Text = Convertdata(rm["theloadlosslv"]);
            NoLoadLoss.Text = Convertdata(rm["noloadloss"]);
            NoLoadCur.Text = Convertdata(rm["Noloadcurrent"]);

        }
        private string Convertdata(object data)
        {
            if (data.ToString() == "")
                return string.Empty;
            else
                return (string)data;
        }

        private bool reviewTable()
        {
            bool passed;
            double outValue;
            SerialNoTextBox.Text = SerialNoTextBox.Text.Trim();
            if (SerialNoTextBox.Text == string.Empty)
            {
                MessageBox.Show("请输入变压器出厂序号!", "变压器管理器", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                SerialNoTextBox.Focus();
                return false;
            }
            if (locationComboBox.SelectedIndex < 0)
            {
                MessageBox.Show("请选择变压器位置!", "变压器管理器", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                locationComboBox.Focus();
                return false;
            }
            passed = double.TryParse(HvVoltageRatingTextBox.Text, out outValue);
            if ((!passed) || (outValue <= 0.0))
            {
                MessageBox.Show("额定电压需要是正数!", "数据错误", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                HvVoltageRatingTextBox.Focus();
                return false;
            }
            passed = double.TryParse(MvVoltageRatingTextBox.Text, out outValue);
            if ((!passed) || (outValue <= 0.0))
            {
                MessageBox.Show("额定电压需要是正数!", "数据错误", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                MvVoltageRatingTextBox.Focus();
                return false;
            }
            if (WindingNumComboBox.SelectedIndex > 0)
            {
                passed = double.TryParse(LvVoltageRatingTextBox.Text, out outValue);
                if ((!passed) || (outValue <= 0.0))
                {
                    MessageBox.Show("额定电压需要是正数!", "数据错误", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    LvVoltageRatingTextBox.Focus();
                    return false;
                }
            }
            passed = double.TryParse(HvPowerRatingTextBox.Text, out outValue);
            if ((!passed) || (outValue <= 0.0))
            {
                MessageBox.Show("额定容量需要是正数!", "数据错误", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                HvPowerRatingTextBox.Focus();
                return false;
            }
            passed = double.TryParse(MvPowerRatingTextBox.Text, out outValue);
            if ((!passed) || (outValue <= 0.0))
            {
                MessageBox.Show("额定容量需要是正数!", "数据错误", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                MvPowerRatingTextBox.Focus();
                return false;
            }
            passed = double.TryParse(HvPowerRatingTextBox.Text, out outValue);
            if ((!passed) || (outValue <= 0.0))
            {
                MessageBox.Show("额定容量需要是正数!", "数据错误", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                LvPowerRatingTextBox.Focus();
                return false;
            }
            return true;
        }

        private bool saveTable()
        {
            if (!reviewTable()) return false;
            DataRow[] rows = WorkingSets.local.Transformers.Select("serialno = '" + SerialNoTextBox.Text.Trim() + "'");
            DataRow[] rowm = WorkingSets.local.Transformermassage.Select("serialno = '" + SerialNoTextBox.Text.Trim() + "'");
            DataRow r = WorkingSets.local.Transformers.NewRow();
            DataRow rm = WorkingSets.local.Transformermassage.NewRow();
            if (rows.Length > 0)
            {
                r = rows[0];
            }
            else
            {
                r = WorkingSets.local.Transformers.NewRow();
            }
            if (rowm.Length > 0)
            {
                rm = rowm[0];
            }
            else
            {
                rm = WorkingSets.local.Transformermassage.NewRow();
            }
            bool previewSave = WorkingSets.local.saveTransformer();
            WorkingSets.local.saveTransformermessage();
            r["serialno"] = SerialNoTextBox.Text;
            r["location"] = (string)locationComboBox.SelectedItem;
            r["apparatusid"] = ApparatusIDTextBox.Text;
            r["manufacturer"] = ManufacturerTextBox.Text;
            r["productionyear"] = ProductionYearTextBox.Text;
            r["assetsystemcode"] = AssetSystemCodeTextBox.Text;
            r["phases"] = (PhaseComboBox.SelectedIndex == 1) ? 3 : 2;
            r["windings"] = (WindingNumComboBox.SelectedIndex == 1) ? 3 : 2;
            r["ratedfrequency"] = (RatingFrequencyComboBox.SelectedIndex == 0) ? 50 : 60;
            r["windingconfig_hv"] = HvWindingConfigComboBox.SelectedIndex;
            r["windingconfig_mv"] = MvWindingConfigComboBox.SelectedIndex;
            r["windingconfig_mv_label"] = MvWindingLabelComboBox.SelectedIndex;
            r["windingconfig_lv"] = LvWindingConfigComboBox.SelectedIndex;
            r["windingconfig_lv_label"] = LvWindingLabelComboBox.SelectedIndex;
            r["voltageratinghv"] = double.Parse(HvVoltageRatingTextBox.Text);
            r["voltageratingmv"] = MvVoltageRatingTextBox.Text;
            r["voltageratinglv"] = LvVoltageRatingTextBox.Text;
            r["powerratinghv"] = HvPowerRatingTextBox.Text;
            r["powerratingmv"] = MvPowerRatingTextBox.Text;
            r["powerratinglv"] = LvPowerRatingTextBox.Text;
            r["bushing_hv_enabled"] = HvBushingCheckBox.IsChecked;
            r["bushing_mv_enabled"] = MvBushingCheckBox.IsChecked;
            r["bushing_lv_enabled"] = LvBushingCheckBox.IsChecked;
            r["coupling"] = coupling.IsChecked;
            if (OLTCCheckBox.IsChecked == true)
            {
                r["oltc_winding"] = OLTCWindingComboBox.SelectedIndex;
                r["oltc_tapmainnum"] = int.Parse(OLTCTapMainNumTextBox.Text);

                r["oltc_taplocation"] = int.Parse(OLTCTapMainNumLocationTextBox.Text);
                r["oltc_tapnum"] = OLTCTapNumComboBox.SelectedIndex;
                r["oltc_multapnum"] = OLTCMulTapNumComboBox.SelectedIndex;
                r["oltc_step"] = OLTCStepComboBox.SelectedIndex;
                r["oltc_serialno"] = OLTCSerialNoTextBox.Text;
                r["oltc_modeltype"] = OLTCModelTypeTextBox.Text;
                r["oltc_manufacturer"] = OLTCManufacturerTextBox.Text;
                r["oltcproductionyear"] = OLTCProductionYearTextBox.Text;
            }
            else
            {
                r["oltc_winding"] = 0;
                r["oltc_tapmainnum"] = 0;
                r["oltc_tapnum"] = -1;
                r["oltc_multapnum"] = -1;
                r["oltc_serialno"] = string.Empty;
                r["oltc_modeltype"] = string.Empty;
                r["oltc_manufacturer"] = string.Empty;
                r["oltcproductionyear"] = string.Empty;
            }
            if (HvBushingCheckBox.IsChecked == true)
            {
                r["hv_a_outfactory"] = Hv_A_OutFactoryId.Text;
                r["hv_b_outfactory"] = Hv_B_OutFactoryId.Text;
                r["hv_c_outfactory"] = Hv_B_OutFactoryId.Text;
                r["hv_o_outfactory"] = Hv_O_OutFactoryId.Text;
                r["hv_o_mp_enabled"] = Hv_o_mp.IsChecked;
            }
            if (MvBushingCheckBox.IsChecked == true)
            {
                r["mv_a_outfactory"] = Mv_A_OutFactoryId.Text;
                r["mv_b_outfactory"] = Mv_B_OutFactoryId.Text;
                r["mv_c_outfactory"] = Mv_C_OutFactoryId.Text;
                r["mv_o_outfactory"] = Mv_O_OutFactoryId.Text;
                r["mv_o_mp_enabled"] = Mv_O_HaveMp.IsChecked;
            }
            if (LvBushingCheckBox.IsChecked == true)
            {
                r["lv_a_outfactory"] = Lv_A_OutFactoryId.Text;
                r["lv_b_outfactory"] = Lv_B_OutFactoryId.Text;
                r["lv_c_outfactory"] = Lv_C_OutFactoryId.Text;
                r["lv_o_outfactory"] = Lv_O_OutFactoryId.Text;
                r["lv_o_mp_enabled"] = Lv_o_mp.IsChecked;
            }
            r["coulplingindex"] = coulpcombobox.SelectedIndex;

            if (rows.Length > 0) r.EndEdit();
            else WorkingSets.local.Transformers.Rows.Add(r);
            WorkingSets.local.saveTransformer();

            rm["transformerid"] = WorkingSets.local.Transformers.Select("serialno = '" + SerialNoTextBox.Text.Trim() + "'")[0]["id"];
            rm["serialno"] = SerialNoTextBox.Text.Trim();
            rm["noloadloss"] = NoLoadLoss.Text;
            rm["impedancevoltagehv"] = HMImpVol.Text;
            rm["impedancevoltagemv"] = HLImpVol.Text;
            rm["impedancevoltagelv"] = MLImpVol.Text;
            rm["theloadlosshv"] = HMLoadLoss.Text;
            rm["theloadlossmv"] = HLLoadLoss.Text;
            rm["theloadlosslv"] = MLLoadLoss.Text;
            rm["Noloadcurrent"] = NoLoadCur.Text;
            if (rowm.Length > 0) r.EndEdit();
            else WorkingSets.local.Transformermassage.Rows.Add(rm);
            return WorkingSets.local.saveTransformermessage();
        }

        public TransformerSettingWindow(string serialNo = "")
        {
            InitializeComponent();

            DataContext = this;

            locationComboBox.ItemsSource = WorkingSets.local.getLocationName();
            locationComboBox.SelectedIndex = 0;
            TransformerInfoInitial(serialNo);
            changed = false;
            this.serialno = serialNo;


        }

        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
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

        private void WindingNumComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (LvPowerRatingLabel == null) return;
            if (LvPowerRatingTextBox == null) return;
            if (LvVoltageRatingLabel == null) return;
            if (LvVoltageRatingTextBox == null) return;
            if (LvWindingConfigComboBox == null) return;
            if (LvWindingLabelComboBox == null) return;
            if (MvBushingCheckBox == null) return;
            if (LvBushingCheckBox == null) return;

            if (WindingNumComboBox.SelectedIndex == 1)
            {
                LvPowerRatingLabel.Visibility = Visibility.Visible;
                LvPowerRatingTextBox.Text = "1";
                LvPowerRatingTextBox.Visibility = Visibility.Visible;
                LvVoltageRatingLabel.Visibility = Visibility.Visible;
                LvVoltageRatingTextBox.Text = "35";
                LvVoltageRatingTextBox.Visibility = Visibility.Visible;
                if (LvWindingConfigComboBox.IsEnabled) LvWindingConfigComboBox.SelectedIndex = 2;
                LvWindingConfigComboBox.Visibility = Visibility.Visible;
                if (LvWindingLabelComboBox.IsEnabled) LvWindingLabelComboBox.SelectedIndex = 11;
                LvWindingLabelComboBox.Visibility = Visibility.Visible;
                LVOLTCComboBoxItem.Visibility = Visibility.Visible;

                twodockpanel1.Visibility = Visibility.Visible;
                twodockpanel2.Visibility = Visibility.Visible;
                twodockpanel3.Visibility = Visibility.Visible;
                twodockpanel4.Visibility = Visibility.Visible;


                controlcombobox();
                LoadCombobox();

                MvBushingCheckBox.Visibility = Visibility.Visible;
                LvBushingCheckBox.Visibility = Visibility.Collapsed;
                if (coulpcombobox != null)
                {
                    hv_lv_wind.Visibility = Visibility.Visible;
                    hv_mv_wind.Visibility = Visibility.Visible;
                    mv_lv_wind.Visibility = Visibility.Visible;
                    coulpcombobox.SelectedIndex = 0;
                }
                ControlCoulpCombobox();
            }
            else
            {
                LvPowerRatingLabel.Visibility = Visibility.Collapsed;
                LvPowerRatingTextBox.Text = "0";
                LvPowerRatingTextBox.Visibility = Visibility.Collapsed;
                LvVoltageRatingLabel.Visibility = Visibility.Collapsed;
                LvVoltageRatingTextBox.Text = "0";
                LvVoltageRatingTextBox.Visibility = Visibility.Collapsed;
                LvWindingConfigComboBox.SelectedIndex = -1;
                LvWindingConfigComboBox.Visibility = Visibility.Collapsed;
                LvWindingLabelComboBox.SelectedIndex = -1;
                LvWindingLabelComboBox.Visibility = Visibility.Collapsed;
                LVOLTCComboBoxItem.Visibility = Visibility.Collapsed;
                twodockpanel1.Visibility = Visibility.Collapsed;
                twodockpanel2.Visibility = Visibility.Collapsed;
                twodockpanel3.Visibility = Visibility.Collapsed;
                twodockpanel4.Visibility = Visibility.Collapsed;

                MvBushingCheckBox.Visibility = Visibility.Collapsed;
                LvBushingCheckBox.Visibility = Visibility.Visible;

                if (coulpcombobox != null)
                {
                    hv_lv_wind.Visibility = Visibility.Collapsed;
                    hv_mv_wind.Visibility = Visibility.Collapsed;
                    mv_lv_wind.Visibility = Visibility.Collapsed;
                    coulpcombobox.SelectedIndex = 0;
                }

            }

            changed = true;
        }

        private void LoadCombobox()
        {
            if (MvWindingLabelComboBox == null) return;
            if (LvWindingLabelComboBox == null) return;
            var data = MvWindingLabelComboBox.Items[0] as ComboBoxItem;
            if (data.Visibility == Visibility.Visible)
            {
                MvWindingLabelComboBox.SelectedIndex = 0;
            }
            else
            {
                MvWindingLabelComboBox.SelectedIndex = 1;
            }

            var data1 = LvWindingLabelComboBox.Items[0] as ComboBoxItem;
            if (data1.Visibility == Visibility.Visible)
            {
                LvWindingLabelComboBox.SelectedIndex = 0;
            }
            else
            {
                LvWindingLabelComboBox.SelectedIndex = 1;
            }
        }

        private void controlcombobox(int flag = 0)
        {
            if (HvWindingConfigComboBox == null) return;
            if (MvWindingConfigComboBox == null) return;
            if (LvWindingConfigComboBox == null) return;
            if (MvWindingLabelComboBox == null) return;
            if (LvWindingLabelComboBox == null) return;
            if (WindingNumComboBox != null && WindingNumComboBox.SelectedIndex == 0)
            {
                foreach (var item in MvWindingLabelComboBox.Items)
                {
                    var da = item as ComboBoxItem;
                    da.Visibility = Visibility.Visible;
                }
                return;
            }
            if (flag == 0 || flag == 1)
            {
                if (HvWindingConfigComboBox.SelectedValue != null && HvWindingConfigComboBox.SelectedValue.ToString().Split(':')[1].Trim().ToCharArray()[0].ToString().ToUpper() ==
         MvWindingConfigComboBox.SelectedValue.ToString().Split(':')[1].Trim().ToCharArray()[0].ToString().ToUpper())
                {
                    int i = 0;
                    foreach (var item in MvWindingLabelComboBox.Items)
                    {
                        var data = item as ComboBoxItem;
                        if (i % 2 == 0)
                        {
                            data.Visibility = Visibility.Visible;

                        }
                        else
                        {
                            data.Visibility = Visibility.Collapsed;
                        }
                        i++;
                    }
                }
                else
                {
                    int i = 0;
                    foreach (var item in MvWindingLabelComboBox.Items)
                    {
                        var data = item as ComboBoxItem;
                        if (i % 2 == 0)
                        {
                            data.Visibility = Visibility.Collapsed;
                        }
                        else
                        {
                            data.Visibility = Visibility.Visible;
                        }
                        i++;
                    }

                }
            }
            if (flag == 0 || flag == 2)
            {
                string[] lowdata = { "yn", "y", "d" };
                if (HvWindingConfigComboBox.SelectedValue.ToString().Split(':')[1].Trim().ToCharArray()[0].ToString().ToUpper() ==
             lowdata[LvWindingConfigComboBox.SelectedIndex].ToUpper().ToCharArray()[0].ToString())
                {
                    int i = 0;
                    foreach (var item in LvWindingLabelComboBox.Items)
                    {
                        var data = item as ComboBoxItem;
                        if (i % 2 == 0)
                        {
                            data.Visibility = Visibility.Visible;
                        }
                        else
                        {
                            data.Visibility = Visibility.Collapsed;
                        }
                        i++;
                    }
                }
                else
                {
                    int i = 0;
                    foreach (var item in LvWindingLabelComboBox.Items)
                    {
                        var data = item as ComboBoxItem;
                        if (i % 2 == 0)
                        {
                            data.Visibility = Visibility.Collapsed;
                        }
                        else
                        {
                            data.Visibility = Visibility.Visible;
                        }
                        i++;
                    }
                }
            }
            LoadCombobox();
            changed = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }



        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!changed) return;
            if (closeConfirmed) return;
            switch (MessageBox.Show("变压器信息已发生更改，是否进行保存?", "位置管理器", MessageBoxButton.YesNoCancel, MessageBoxImage.Question))
            {
                case MessageBoxResult.Yes:
                    closeWithConfirm();
                    return;
                case MessageBoxResult.No:
                    return;
                case MessageBoxResult.Cancel:
                    e.Cancel = true;
                    return;
            }
        }

        private void closeWithConfirm()
        {
            if (saveTable())
            {
                serialno = SerialNoTextBox.Text;
                closeConfirmed = true;
            }
        }

        private void TextChanged(object sender, object e)
        {
            changed = true;
        }

        private void PhaseComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (HvWindingConfigComboBox == null) return;
            if (MvWindingConfigComboBox == null) return;
            if (MvWindingConfigComboBox == null) return;
            if (LvWindingConfigComboBox == null) return;
            if (LvWindingLabelComboBox == null) return;
            changed = true;
            if (PhaseComboBox.SelectedIndex == 1)
            {
                HvWindingConfigComboBox.SelectedIndex = 0;
                MvWindingConfigComboBox.SelectedIndex = 0;
                MvWindingLabelComboBox.SelectedIndex = 0;
                LvWindingConfigComboBox.SelectedIndex = 2;
                LvWindingLabelComboBox.SelectedIndex = 11;
                HvWindingConfigComboBox.IsEnabled = true;
                MvWindingConfigComboBox.IsEnabled = true;
                MvWindingLabelComboBox.IsEnabled = true;
                LvWindingConfigComboBox.IsEnabled = true;
                LvWindingLabelComboBox.IsEnabled = true;
            }
            else
            {
                HvWindingConfigComboBox.SelectedIndex = -1;
                MvWindingConfigComboBox.SelectedIndex = -1;
                MvWindingLabelComboBox.SelectedIndex = -1;
                LvWindingConfigComboBox.SelectedIndex = -1;
                LvWindingLabelComboBox.SelectedIndex = -1;
                HvWindingConfigComboBox.IsEnabled = false;
                MvWindingConfigComboBox.IsEnabled = false;
                MvWindingLabelComboBox.IsEnabled = false;
                LvWindingConfigComboBox.IsEnabled = false;
                LvWindingLabelComboBox.IsEnabled = false;
            }
        }

        private void OLTCCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            OLTCMainTapWrapPanel.Visibility = Visibility.Visible;
            OLTCGrid.Visibility = Visibility.Visible;
            changed = true;
        }

        private void OLTCCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            OLTCMainTapWrapPanel.Visibility = Visibility.Collapsed;
            OLTCGrid.Visibility = Visibility.Collapsed;
            changed = true;
        }

        private void SerialNoTextBox_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            SerialNoTextBox.Text = SerialNoTextBox.Text.Trim();
            string SerialNo = SerialNoTextBox.Text;
            DataRow[] rows = WorkingSets.local.Transformers.Select("serialno = '" + SerialNo + "'");
            if (rows.Length > 0)
            {
                if ((originSerialNo != string.Empty) && (originSerialNo != SerialNo))
                {
                    MessageBox.Show("该变压器出厂序号已存在，请修改出厂序号!", "变压器出厂序号重复", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    SerialNoTextBox.Focus();
                    return;
                }
                if (originSerialNo != SerialNo)
                    switch (MessageBox.Show("该变压器出厂序号已存在，是否对该变压器信息进行修改", "变压器出厂序号重复", MessageBoxButton.YesNo, MessageBoxImage.Exclamation))
                    {
                        case MessageBoxResult.Yes:
                            SerialNoTextBox.Focus();
                            return;
                        default:
                            SerialNoTextBox.Focus();
                            return;
                    }
            }
        }

        private void OLTCTapNumComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            changed = true;
            if (OLTCTapNumComboBox == null) return;
            if (OLTCStepComboBox == null) return;
            switch (((ComboBoxItem)(OLTCTapNumComboBox.SelectedItem)).Content.ToString())
            {
                case "1":
                    OLTCStepComboBox.SelectedIndex = 6;
                    break;
                case "2":
                    OLTCStepComboBox.SelectedIndex = 5;
                    break;
                case "4":
                    OLTCStepComboBox.SelectedIndex = 4;
                    break;
                case "5":
                    OLTCStepComboBox.SelectedIndex = 3;
                    break;
                case "8":
                    OLTCStepComboBox.SelectedIndex = 2;
                    break;
                case "10":
                    OLTCStepComboBox.SelectedIndex = 1;
                    break;
            }
        }

        private void LossPanelButton_Click(object sender, RoutedEventArgs e)
        {
            changed = true;
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            closeWithConfirm();
            this.Close();
        }

        private void OLTCTapMainNumTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!Microsoft.VisualBasic.Information.IsNumeric(e.Text))
                e.Handled = true;
            else
                e.Handled = false;
        }

        private void OLTCTapMainNumTextBox_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (!Microsoft.VisualBasic.Information.IsNumeric(OLTCTapMainNumTextBox.Text))
            {
                MessageBox.Show("请输入变压器主分接数，应为1或3。", "变压器管理器", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                OLTCTapMainNumTextBox.Focus();
                return;
            }
            if (OLTCTapMainNumTextBox.Text.IndexOf('.') > -1)
            {
                MessageBox.Show("请输入变压器主分接数，应为1或3。", "变压器管理器", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                OLTCTapMainNumTextBox.Focus();
                return;
            }
            int OLTCTapMainNum = int.Parse(OLTCTapMainNumTextBox.Text);
            if ((OLTCTapMainNum != 1) && (OLTCTapMainNum != 3))
            {
                MessageBox.Show("请输入变压器主分接数，应为1或3。", "变压器管理器", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                OLTCTapMainNumTextBox.Focus();
                return;
            }
        }

        private void TextChanged(object sender, RoutedEventArgs e)
        {
            changed = true;
        }



        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void OLTCTapMainNumLocationTextBox_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            OLTCTapMainNumLocationTextBox.Focus();
        }

        private void OLTCMulTapNumComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            changed = true;
            if (OLTCMulTapNumComboBox == null)
                return;

        }
        private void TextChanged(object sender, SelectionChangedEventArgs e)
        {
            controlcombobox();


            changed = true;
        }

        //private void coulpchanged()
        //{
        //    Hv_Mv.Visibility = Visibility.Collapsed;
        //    Hv_Lv.Visibility = Visibility.Collapsed;
        //    Mv_Lv.Visibility = Visibility.Collapsed;
        //    AllWind.Visibility = Visibility.Collapsed;
        //    bool zeroOrSixIsOk = false;
        //    if (MvWindingLabelComboBox != null && WindingNumComboBox.SelectedIndex == 1)
        //    {
        //        if (MvWindingLabelComboBox.SelectedIndex.ToString().Trim() == "0" || MvWindingLabelComboBox.SelectedIndex.ToString().Trim() == "6")
        //        {
        //            zeroOrSixIsOk = true;
        //        }
        //        //高中自耦
        //        if (zeroOrSixIsOk)
        //            Hv_Mv.Visibility = Visibility.Visible;
        //        else
        //        {
        //            Hv_Mv.Visibility = Visibility.Collapsed;
        //            Hv_Lv.Visibility = Visibility.Collapsed;
        //            Mv_Lv.Visibility = Visibility.Collapsed;
        //            AllWind.Visibility = Visibility.Collapsed;
        //        }
        //    }

        //    bool isexit = false;
        //    if (LvWindingLabelComboBox != null && WindingNumComboBox.SelectedIndex == 1)
        //    {

        //        if (LvWindingLabelComboBox.SelectedIndex.ToString().Trim() == "0" || LvWindingLabelComboBox.SelectedIndex.ToString().Trim() == "6")
        //        {
        //            isexit = true;
        //        }
        //        //高低自耦
        //        if (isexit)
        //            Hv_Lv.Visibility = Visibility.Visible;
        //        else
        //        {
        //            Hv_Lv.Visibility = Visibility.Collapsed;
        //            Mv_Lv.Visibility = Visibility.Collapsed;
        //            AllWind.Visibility = Visibility.Collapsed;
        //        }
        //        if (isexit && zeroOrSixIsOk)
        //        {
        //            Hv_Mv.Visibility = Visibility.Visible;
        //            Hv_Lv.Visibility = Visibility.Visible;
        //            Mv_Lv.Visibility = Visibility.Visible;
        //            AllWind.Visibility = Visibility.Visible;
        //        }
        //    }
        //}

        private void Mv_selectchange(object sender, SelectionChangedEventArgs e)
        {
            controlcombobox(1);
        }

        private void Lv_change(object sender, SelectionChangedEventArgs e)
        {
            controlcombobox(2);
        }

        private void ControlCoulpCombobox()
        {
            bool MvIsContain = false;
            bool LvIsContain = false;
            bool Mulcontain = false;
            if (hv_mv_wind != null && MvWindingLabelComboBox != null)
            {
                hv_mv_wind.Visibility = Visibility.Collapsed;
                foreach (var item in MvWindingLabelComboBox.Items)
                {
                    var data = item as ComboBoxItem;
                    if (data.Visibility == Visibility.Visible && (data.Content.ToString().Contains("0") || data.Content.ToString().Contains("6")))
                    {
                        MvIsContain = true;
                        hv_mv_wind.Visibility = Visibility.Visible;
                        break;
                    }
                }
            }
            if (hv_lv_wind != null && LvWindingLabelComboBox != null && LvWindingLabelComboBox.Visibility == Visibility.Visible)
            {
                hv_lv_wind.Visibility = Visibility.Collapsed;
                foreach (var item in LvWindingLabelComboBox.Items)
                {
                    var data = item as ComboBoxItem;
                    if (data.Visibility == Visibility.Visible && (data.Content.ToString().Contains("0") || data.Content.ToString().Contains("6")))
                    {
                        LvIsContain = true;
                        hv_lv_wind.Visibility = Visibility.Visible;
                        break;
                    }
                }
            }
            if (mv_lv_wind != null && MvWindingLabelComboBox != null && LvWindingLabelComboBox != null && LvWindingLabelComboBox.Visibility == Visibility.Visible)
            {
                mv_lv_wind.Visibility = Visibility.Collapsed;
                var p = MvWindingLabelComboBox.SelectedIndex - LvWindingLabelComboBox.SelectedIndex;
                if (p == 0 || p == 6 || p == -6)
                {
                    mv_lv_wind.Visibility = Visibility.Visible;
                    Mulcontain = true;
                }
            }
            if (hv_mv_wind != null && hv_lv_wind != null && mv_lv_wind != null && All_wind != null && MvIsContain && LvIsContain && Mulcontain)
            {
                hv_mv_wind.Visibility = Visibility.Visible;
                hv_lv_wind.Visibility = Visibility.Visible;
                mv_lv_wind.Visibility = Visibility.Visible;
                All_wind.Visibility = Visibility.Visible;
            }
            changed = true;
        }

        private void Mv_label_change(object sender, SelectionChangedEventArgs e)
        {
            //if (MvWindingLabelComboBox == null) return;
            //if (Hv_Mv == null) return;
            //if (Hv_Lv == null) return;
            //if (Mv_Lv == null) return;
            //if (AllWind == null) return;
            //coulpchanged();

            ControlCoulpCombobox();
        }

        private void MvBushingCheckBox_Loaded(object sender, RoutedEventArgs e)
        {
            if (WindingNumComboBox != null && WindingNumComboBox.SelectedIndex == 1)
            {
                MvBushingCheckBox.Visibility = Visibility.Visible;
            }
            if (WindingNumComboBox != null && WindingNumComboBox.SelectedIndex == 0)
            {
                MvBushingCheckBox.Visibility = Visibility.Collapsed;
            }
            if (MvBushingCheckBox != null && Mv_bushing_groupbox != null)
            {
                if ((bool)MvBushingCheckBox.IsChecked)
                    Mv_bushing_groupbox.Visibility = Visibility.Visible;
                else
                    Mv_bushing_groupbox.Visibility = Visibility.Collapsed;
            }
            changed = true;

        }

        private void LvBushingCheckBox_Loaded(object sender, RoutedEventArgs e)
        {
            if (WindingNumComboBox != null && WindingNumComboBox.SelectedIndex == 1)
            {
                LvBushingCheckBox.Visibility = Visibility.Collapsed;
            }
            if (WindingNumComboBox != null && WindingNumComboBox.SelectedIndex == 0)
            {
                LvBushingCheckBox.Visibility = Visibility.Visible;
            }

            if (LvBushingCheckBox != null && Lv_bushing_groupbox != null)
            {
                if ((bool)LvBushingCheckBox.IsChecked)
                    Lv_bushing_groupbox.Visibility = Visibility.Visible;
                else
                    Lv_bushing_groupbox.Visibility = Visibility.Collapsed;
            }
            changed = true;

        }

        private void HvBushingCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (HvBushingCheckBox != null)
                Hv_bushing_groupbox.Visibility = Visibility.Visible;
            changed = true;
        }

        private void HvBushingCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            if (HvBushingCheckBox != null)
                Hv_bushing_groupbox.Visibility = Visibility.Collapsed;
            changed = true;

        }
        private void MvBushingCheckBox_Checked(object sender, RoutedEventArgs e)
        {

            if (MvBushingCheckBox != null)
                Mv_bushing_groupbox.Visibility = Visibility.Visible;
            changed = true;
        }

        private void MvBushingCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            if (MvBushingCheckBox != null)
                Mv_bushing_groupbox.Visibility = Visibility.Collapsed;
            changed = true;

        }
        private void LvBushingCheckBox_Checked(object sender, RoutedEventArgs e)
        {

            if (LvBushingCheckBox != null)
                Lv_bushing_groupbox.Visibility = Visibility.Visible;
            changed = true;
        }

        private void LvBushingCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            if (LvBushingCheckBox != null)
                Lv_bushing_groupbox.Visibility = Visibility.Collapsed;
            changed = true;

        }

        private void HvBushingCheckBox_Loaded(object sender, RoutedEventArgs e)
        {
            if (HvBushingCheckBox != null && Hv_bushing_groupbox != null)
            {
                if ((bool)HvBushingCheckBox.IsChecked)
                    Hv_bushing_groupbox.Visibility = Visibility.Visible;
                else
                    Hv_bushing_groupbox.Visibility = Visibility.Collapsed;
            }

            changed = true;
        }

        private void MvBushingCheckBox_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (MvBushingCheckBox != null && MvBushingCheckBox.Visibility == Visibility.Collapsed)
            {
                MvBushingCheckBox.IsChecked = false;
            }
        }

        private void LvBushingCheckBox_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (LvBushingCheckBox != null && LvBushingCheckBox.Visibility == Visibility.Collapsed)
            {
                LvBushingCheckBox.IsChecked = false;
            }
        }

        private void coulpcombobox_Loaded(object sender, RoutedEventArgs e)
        {
            ControlCoulpCombobox();
        }
    }
}
