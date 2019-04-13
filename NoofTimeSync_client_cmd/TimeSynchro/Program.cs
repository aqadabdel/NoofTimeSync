using System;
using System.IO;

using System.Collections.Generic;

using System.Diagnostics;

namespace TimeSynchro
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 1 && (args[0] == "--help" || args[0] == "-h" || args[0] == "/?"))
            {
                ShowUsage();
            }
            else if (args.Length == 2 && (args[0] == "--ntp" || args[0] == "-n"))
            {
                string ntpserver = args[1];
                Console.WriteLine("NTP SERVER: " + ntpserver);
                SetDateTime.SetDateTimeFromNTP(ntpserver);
                Console.ReadKey();
            }
            else if (args.Length == 2 && (args[0] == "--url" || args[0] == "-u"))
            {
                string json_address = args[1];
                Console.WriteLine("JSON TIME SERVER: " + json_address);
                //SetDateTime.SetDateTimeFromWorldClockApi();
                Console.ReadKey();
            }
            else
            {
                SetDateTime.SetDateTimeFromWorldClockApi();
                
                /* PAUSE */
                Console.ReadKey();
            }

        }

       

        private static void ShowUsage()
        {
            Console.WriteLine("____________________________________________________________");
            Console.WriteLine("By default this command will sync time directly from from worldtimeapi.org");
            Console.WriteLine(" -n or --ntp ntpserver_hostname to sync from a specified NTP server");
            Console.WriteLine(" -u or --url jsonapi_server_address to sync from a specified JSSON api server");
            Console.WriteLine("  MiniTimeSync v0.1  released: April 09, 2019");
            Console.WriteLine("  This tool requires aministratives privileges.");
            Console.WriteLine("  Under Free usage 2019 AQAD Abdel");
            Console.WriteLine("  https://github.com/aqadabdel/");
            Console.WriteLine("____________________________________________________________");
        }
    }
}
