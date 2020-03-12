using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCEEC.NET.TCPSERVER;
using SCEEC.MI.TZ3310;
using System.Runtime.Serialization.Formatters.Binary;
using Newtonsoft.Json;
using System.IO;
using System.Net.Sockets;

namespace GetTcpJob
{
    public class GetLocationCode
    {
        public bool TcpIsRunning { get; set; } = false;
        public bool StartTcp()
        {
            if(!TcpIsRunning)
            {
                TcpTask.TcpServer.CloseAllClient();//待测试
                TcpTask.TcpServer.DataReceived += TcpServer_DataReceived;
                TcpTask.TcpServer.ClientConnected += TcpServer_ClientConnected;
                TcpTask.TcpServer.ClientDisconnected += TcpServer_ClientDisconnected;
                TcpTask.TcpServer.CompletedSend += TcpServer_CompletedSend;
            }
            return true;
         
        }
        public bool CloseTcp()
        {
            if (!TcpIsRunning)
            {
                TcpTask.TcpServer.CloseAllClient();//待测试
                TcpTask.TcpServer.DataReceived -= TcpServer_DataReceived;
                TcpTask.TcpServer.ClientConnected -= TcpServer_ClientConnected;
                TcpTask.TcpServer.ClientDisconnected -= TcpServer_ClientDisconnected;
                TcpTask.TcpServer.CompletedSend -= TcpServer_CompletedSend;
            }
            return true;

        }

        private void TcpServer_CompletedSend(object sender, AsyncEventArgs e)
        {
        }

        private void TcpServer_ClientDisconnected(object sender, AsyncEventArgs e)
        {
        }

        private void TcpServer_ClientConnected(object sender, AsyncEventArgs e)
        {
        }

        private void TcpServer_DataReceived(object sender, AsyncEventArgs e)
        {


        }

        public void ReceiveLocation(Location lcs)
        {
           
        }

        public void ReceiveTransformer(Transformer trs)
        {
            JsonSerializerSettings jsetting = new JsonSerializerSettings()
            {
                NullValueHandling = NullValueHandling.Include,
                StringEscapeHandling = StringEscapeHandling.EscapeNonAscii,
            };
            var jsonp = JsonConvert.SerializeObject(trs, Formatting.Indented, jsetting);

            File.WriteAllText("Location.json", jsonp);
            Transformer ts = JsonConvert.DeserializeObject<Transformer>(jsonp);
        }

        public void ReceiveJob(JobList job)
        {

        }


        public void SendData(byte[] senddata, AsyncTCPServer tCPServer)
        {
            try
            {
                // state = (TCPClientState)tCPServer._clients.ToArray()[0];
                var temp = tCPServer._clients.ToArray();
                foreach (var a in temp)
                {
                    tCPServer.Send((TCPClientState)a, senddata);
                }
                // tCPServer.Send(state.TcpClient, senddata);
            }
            catch (SocketException ex)
            {
                throw ex;
            }
        }


    }
}
