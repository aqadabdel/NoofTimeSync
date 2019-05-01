using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeSynchro
{
    class JsonServer
    {
        public class WorldClockJson
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
    }
}
