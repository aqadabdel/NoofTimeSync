using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using Newtonsoft.Json;
using System.Net;

namespace TimeSynchro
{
   class TimeSync
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        public extern static uint SetSystemTime(ref SYSTEMTIME systemtime);

        DateTime serverTime;

        public struct SYSTEMTIME
        {
            public ushort wYear, wMonth,
                          wDayOfWeek, wDay,
                          wHour, wMinute,
                          wSecond, wMilliseconds;
        }

        public TimeSync(DateTime dt)
        {
            this.serverTime = dt.ToUniversalTime();
            SetWindowsTime();
            
        }
            
        private void SetWindowsTime()
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

            if (SetSystemTime(ref st) == 0)
            {
                Console.WriteLine("DATE & TIME: SYNCHRONIZATION DONE.");
            }
            else
            {
                Console.WriteLine("DATE & TIME: CAN'T SET DATETIME.");
            }
        }
    }


}
