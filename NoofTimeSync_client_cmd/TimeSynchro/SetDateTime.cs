using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using Newtonsoft.Json;

namespace TimeSynchro
{
    class SetDateTime
    {
      
        public static bool SetDateTimeFromWorldClockApi()
        {
            using (WebClient wc = new WebClient())
            {
                //string url = "https://localhost:44353/api/time";
                string url = "http://worldtimeapi.org/api/ip";
                var json = "";
                try
                {
                    json = wc.DownloadString(url);
                    var obj = JsonConvert.DeserializeObject<JsonServer.WorldClockJson>(json);
                    
                    DateTime server_time = new DateTime();
                    server_time = Convert.ToDateTime(obj.datetime);

                    Console.WriteLine("DATE & SYSTEM TIME: {0}", DateTime.Now);
                    Console.WriteLine("YOUR GEOGRAPHIC ZONE: {0} ", obj.timezone);
                    Console.WriteLine("TIMESTAMP: {0} ", obj.datetime);
                    Console.WriteLine("UTC OFFSET: {0} ", obj.utc_offset);
                    Console.WriteLine("DATE & TIME ON REMOTE SERVER: " + server_time.ToString("dd/MM/yyyy HH:mm:ss"));

                    new TimeSync(server_time);
                    return true;

                }
                catch (Exception ex)
                {
                    PrintErrorCon(url);
                    Console.WriteLine("Error Message: {0}", ex.Message);
                    return false;
                }
            }

            
        }


        public  static bool SetDateTimeFromNTP(string ntpAdress)
        {
            DateTime dt = GetDateTimeFromNTP(ntpAdress);
            PrintDateTime(dt);
            new TimeSync(TimeZoneInfo.ConvertTimeFromUtc(dt, TimeZoneInfo.Local));
            return true;
        }




        private static DateTime GetDateTimeFromNTP(string address)
        {
            string ntpServer = address;
            var ntpData = new byte[48];
            ntpData[0] = 0x1B; //LeapIndicator = 0 (no warning), VersionNum = 3 (IPv4 only), Mode = 3 (Client Mode)

            var addresses = Dns.GetHostEntry(ntpServer).AddressList;
            var ipEndPoint = new IPEndPoint(addresses[0], 123);
            var socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

            socket.Connect(ipEndPoint);
            socket.Send(ntpData);
            socket.Receive(ntpData);
            socket.Close();

            ulong intPart = (ulong)ntpData[40] << 24 | (ulong)ntpData[41] << 16 | (ulong)ntpData[42] << 8 | (ulong)ntpData[43];
            ulong fractPart = (ulong)ntpData[44] << 24 | (ulong)ntpData[45] << 16 | (ulong)ntpData[46] << 8 | (ulong)ntpData[47];

            var milliseconds = (intPart * 1000) + ((fractPart * 1000) / 0x100000000L);
            var networkDateTime = (new DateTime(1900, 1, 1)).AddMilliseconds((long)milliseconds);

            return networkDateTime;
        }

        private static void PrintDateTime(DateTime dt)
        {
            //dt.ToUniversalTime();
            Console.WriteLine("UTC DATETIME ON REMOTE SERVER: " + dt.ToString("dd/MM/yyyy HH:mm:ss"));

            Console.WriteLine("LOCAL DATE & TIME FROM REMOTE SERVER: " + TimeZoneInfo.ConvertTimeFromUtc(dt, TimeZoneInfo.Local).ToString("dd/MM/yyyy HH:mm:ss"));
            
            //Console.WriteLine(TimeZoneInfo.Local.GetUtcOffset(dt));
            //Console.WriteLine( );
        }

        private static void PrintErrorCon(string url)
        {
            Console.WriteLine("Error: Can't download data from {0}", url);
            Console.WriteLine("Please verify your network configuration");
        }


    }
}
