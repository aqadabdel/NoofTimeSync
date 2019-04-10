using System;
using System.IO;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace TimeSynchro
{
    class Program
    {

        [DllImport("kernel32.dll", SetLastError = true)]
        public extern static uint SetSystemTime(ref SYSTEMTIME systemtime);

        static void Main(string[] args)
        {
            if (args.Length == 1 && (args[0] == "--help" || args[0] == "-h" || args[0] == "/?"))
            {
                ShowUsage();
            }
            else
            {

                using (WebClient wc = new WebClient())
                {
                    try
                    {
                        var json = "";
                        json = wc.DownloadString("https://localhost:44353/api/time");
                        var obj = JsonConvert.DeserializeObject<jsontime>(json);

                        DateTime server_time = new DateTime();
                        server_time = Convert.ToDateTime(obj.datetime);
                        //Console.Beep();

                        Console.WriteLine("DATE & SYSTEM TIME: {0}", DateTime.Now);
                        Console.WriteLine("YOUR GEOGRAPHIC ZONE: {0} ", obj.timezone);
                        Console.WriteLine("TIMESTAMP: {0} ", obj.datetime);
                        Console.WriteLine("UTC OFFSET: {0} ", obj.utc_offset);
                        Console.WriteLine("DATE & TIME ON REMOTE SERVER: " + server_time.ToString("dd/MM/yyyy HH:mm:ss"));


                        server_time = server_time.ToUniversalTime();

                        /* CALL kernel32.dll to set system time, only works with admin privileges */
                        SetWindowsTime(server_time);
                        Console.WriteLine("DATE & HEURE : Synchronisation effectuée");
                        json = null;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(@"Error: Can't download data from http://worldtimeapi.org/api/ip");
                        Console.WriteLine("Please verify your network configuration");
                        Console.WriteLine("Error Message: {0}", ex.Message);
                    }
                }
                /* PAUSE */
                Console.ReadKey();
            }

        }

        public struct SYSTEMTIME
        {
            public ushort wYear,      wMonth,
                          wDayOfWeek, wDay,
                          wHour,      wMinute,
                          wSecond,    wMilliseconds;
        }

        public static void SetWindowsTime(DateTime serverTime)
        {
            SYSTEMTIME st = new SYSTEMTIME();
            st.wYear = (ushort)serverTime.Year;
            st.wMonth = (ushort)serverTime.Month;
            st.wDayOfWeek = (ushort)serverTime.DayOfWeek;
            st.wDay = (ushort)serverTime.Day;
            st.wHour = (ushort)serverTime.Hour;
            st.wMinute = (ushort)serverTime.Minute;
            st.wSecond = (ushort)serverTime.Second;
            st.wMilliseconds = (ushort)serverTime.Millisecond;
            SetSystemTime(ref st);
            
        }
        
        public class jsontime
        {
            public string week_number { get; set; }
            public string utc_offset { get; set; }
            public string unixtime { get; set; }
            public string timezone { get; set; }
            public string dst_until { get; set; }
            public string dst_from { get; set; }
            public string dst { get; set; }
            public string day_of_year { get; set; }
            public string day_of_week { get; set; }
            public string datetime { get; set; }
            public string abbreviation { get; set; }
        }

        private static void ShowUsage()
        {
            Console.WriteLine("____________________________________________________________");
            Console.WriteLine("  MiniTimeSync v0.1  released: April 09, 2019");
            Console.WriteLine("  This tool synchronize time directly from worldtimeapi.org");
            Console.WriteLine("  It requires Administratives right to do so.");
            Console.WriteLine("  Under Free usage 2019 AQAD Abdel");
            Console.WriteLine("  https://github.com/aqadabdel/");
            Console.WriteLine("____________________________________________________________");
        }
    }
}
