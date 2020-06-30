using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HNReport
{
    public class Infomation
    {

        public struct JobInformation
        {


            private string _testingName;
            private string _tester;
            private string _testingAgency;
            private string _auditor;
            private string _approver;
            private string _weather;
            private string _temperature;
            private string _humidity;
            private string _principal;

            public DateTime testingTime;
            public string testingName
            {
                get
                {
                    if (this._testingName != null)
                    {

                        if (this._testingName.Length > 0)
                            return this._testingName;
                        else
                            return "--未指定--";
                    }
                    else
                    {
                        return "--未指定--";

                    }
                }
                set
                {
                    this._testingName = value;
                }
            }
            public string tester
            {
                get
                {
                    if (this._tester.Length > 0)
                        return this._tester;
                    else
                        return "--未指定--";
                }
                set
                {
                    this._tester = value;
                }
            }
            public string testingAgency
            {
                get
                {
                    if (this._testingAgency.Length > 0)
                        return this._testingAgency;
                    else
                        return "--未指定--";
                }
                set
                {
                    this._testingAgency = value;
                }
            }
            public string auditor
            {
                get
                {
                    if (this._auditor.Length > 0)
                        return this._auditor;
                    else
                        return "--未指定--";
                }
                set
                {
                    this._auditor = value;
                }
            }
            public string approver
            {
                get
                {
                    if (this._approver.Length > 0)
                        return this._approver;
                    else
                        return "--未指定--";
                }
                set
                {
                    this._approver = value;
                }
            }
            public string weather
            {
                get
                {
                    if (this._weather.Length > 0)
                        return this._weather;
                    else
                        return "--未指定--";
                }
                set
                {
                    this._weather = value;
                }
            }
            public string temperature
            {
                get
                {
                    if (this._temperature.Length > 0)
                        return this._temperature;
                    else
                        return "--未指定--";
                }
                set
                {
                    this._temperature = value;
                }
            }
            public string humidity
            {
                get
                {
                    if (this._humidity.Length > 0)
                        return this._humidity;
                    else
                        return "--未指定--";
                }
                set
                {
                    this._humidity = value;
                }
            }
            public string principal
            {
                get
                {
                    if (this._principal.Length > 0)
                        return this._principal;
                    else
                        return "--未指定--";
                }
                set
                {
                    this._principal = value;
                }
            }
            public double oilTemperature;

            public override int GetHashCode()
            {
                return this.ToString().GetHashCode();
            }

            public override string ToString()
            {
                var sb = new System.IO.StringWriter();
                sb.WriteLine(Convert.ToBase64String(BitConverter.GetBytes(testingTime.ToBinary())));
                sb.WriteLine(Convert.ToBase64String(Encoding.UTF32.GetBytes(testingName)));
                sb.WriteLine(Convert.ToBase64String(Encoding.UTF32.GetBytes(tester)));
                sb.WriteLine(Convert.ToBase64String(Encoding.UTF32.GetBytes(testingAgency)));
                sb.WriteLine(Convert.ToBase64String(Encoding.UTF32.GetBytes(auditor)));
                sb.WriteLine(Convert.ToBase64String(Encoding.UTF32.GetBytes(approver)));
                sb.WriteLine(Convert.ToBase64String(Encoding.UTF32.GetBytes(weather)));
                sb.WriteLine(Convert.ToBase64String(Encoding.UTF32.GetBytes(temperature)));
                sb.WriteLine(Convert.ToBase64String(Encoding.UTF32.GetBytes(humidity)));
                sb.WriteLine(Convert.ToBase64String(Encoding.UTF32.GetBytes(principal)));
                sb.WriteLine(Convert.ToBase64String(BitConverter.GetBytes(oilTemperature)));
                return sb.ToString();
            }

            public static JobInformation FromString(string s)
            {
                JobInformation info = new JobInformation();
                var sr = new System.IO.StringReader(s);
                string base64;

                base64 = sr.ReadLine();
                if (base64 == string.Empty) throw new ArgumentException("JobInformation字段信息缺失");
                info.testingTime = DateTime.FromBinary(BitConverter.ToInt64(Convert.FromBase64String(base64), 0));

                base64 = sr.ReadLine();
                if (base64 == string.Empty) throw new ArgumentException("JobInformation字段信息缺失");
                info.testingName = Encoding.UTF32.GetString(Convert.FromBase64String(base64));

                base64 = sr.ReadLine();
                if (base64 == string.Empty) throw new ArgumentException("JobInformation字段信息缺失");
                info.tester = Encoding.UTF32.GetString(Convert.FromBase64String(base64));

                base64 = sr.ReadLine();
                if (base64 == string.Empty) throw new ArgumentException("JobInformation字段信息缺失");
                info.testingAgency = Encoding.UTF32.GetString(Convert.FromBase64String(base64));

                base64 = sr.ReadLine();
                if (base64 == string.Empty) throw new ArgumentException("JobInformation字段信息缺失");
                info.auditor = Encoding.UTF32.GetString(Convert.FromBase64String(base64));

                base64 = sr.ReadLine();
                if (base64 == string.Empty) throw new ArgumentException("JobInformation字段信息缺失");
                info.approver = Encoding.UTF32.GetString(Convert.FromBase64String(base64));

                base64 = sr.ReadLine();
                if (base64 == string.Empty) throw new ArgumentException("JobInformation字段信息缺失");
                info.weather = Encoding.UTF32.GetString(Convert.FromBase64String(base64));

                base64 = sr.ReadLine();
                if (base64 == string.Empty) throw new ArgumentException("JobInformation字段信息缺失");
                info.temperature = Encoding.UTF32.GetString(Convert.FromBase64String(base64));

                base64 = sr.ReadLine();
                if (base64 == string.Empty) throw new ArgumentException("JobInformation字段信息缺失");
                info.humidity = Encoding.UTF32.GetString(Convert.FromBase64String(base64));

                base64 = sr.ReadLine();
                if (base64 == string.Empty) throw new ArgumentException("JobInformation字段信息缺失");
                info.principal = Encoding.UTF32.GetString(Convert.FromBase64String(base64));

                base64 = sr.ReadLine();
                if (base64 == string.Empty) throw new ArgumentException("JobInformation字段信息缺失");
                info.oilTemperature = BitConverter.ToDouble(Convert.FromBase64String(base64), 0);

                return info;
            }


        }

    }


  
}
