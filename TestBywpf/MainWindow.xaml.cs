using Newtonsoft.Json;
using SCEEC.MI.TZ3310;
using System;
using System.IO;
using System.Windows;

namespace TestBywpf
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            ReceiveTransformer(new Transformer());
            JobInformation jf = new JobInformation
            {
                approver = "approver",
                auditor = "auditor",
                humidity = "humidity",
                oilTemperature = 20.6,
                principal = "principal",
                temperature = "temperature",
                tester = "tester",
                testingAgency = "testingAgency",
                testingName = "testingName",
                testingTime = DateTime.Now,
                weather = "Good"

            };
            ReceiveTransformer(new JobList() { Information = jf });
            ReceiveTransformer(new Location());
            JobList js = JsonConvert.DeserializeObject<JobList>(File.ReadAllText("JobList.json"));
            Transformer ts = JsonConvert.DeserializeObject<Transformer>(File.ReadAllText("Transformer.json"));
            Location lc = JsonConvert.DeserializeObject<Location>(File.ReadAllText("Location.json"));

        }

        public void ReceiveTransformer(Transformer trs)
        {
            JsonSerializerSettings jsetting = new JsonSerializerSettings()
            {
                NullValueHandling = NullValueHandling.Include,
                StringEscapeHandling = StringEscapeHandling.EscapeNonAscii,
            };
            var jsonp = JsonConvert.SerializeObject(trs, Formatting.Indented, jsetting);
            File.WriteAllText("Transformer.json", jsonp);
           // Transformer ts = JsonConvert.DeserializeObject<Transformer>(jsonp);
        }

        public void ReceiveTransformer(JobList trs)
        {
            JsonSerializerSettings jsetting = new JsonSerializerSettings()
            {
                NullValueHandling = NullValueHandling.Include,
                StringEscapeHandling = StringEscapeHandling.EscapeNonAscii,
            };
            var jsonp = JsonConvert.SerializeObject(trs, Formatting.Indented, jsetting);
            byte[] byteArray = System.Text.Encoding.Default.GetBytes(jsonp);
            var jsbp = System.Text.Encoding.Default.GetString(byteArray);
            File.WriteAllText("JobList.json", jsonp);
            JobList ts = JsonConvert.DeserializeObject<JobList>(jsbp);


        }

        public void ReceiveTransformer(Location trs)
        {
            JsonSerializerSettings jsetting = new JsonSerializerSettings()
            {
                NullValueHandling = NullValueHandling.Include,
                StringEscapeHandling = StringEscapeHandling.EscapeNonAscii,
            };
            var jsonp = JsonConvert.SerializeObject(trs, Formatting.Indented, jsetting);
            byte[] byteArray = System.Text.Encoding.Default.GetBytes(jsonp);
            var jsbp= System.Text.Encoding.Default.GetString(byteArray);
            byte[] data = System.Text.Encoding.Default.GetBytes("Location" + jsbp);
            File.WriteAllText("Location.json", jsonp);

            //  JobList ts = JsonConvert.DeserializeObject<JobList>(File.ReadAllText("Location.json"));
        }

    }
}
