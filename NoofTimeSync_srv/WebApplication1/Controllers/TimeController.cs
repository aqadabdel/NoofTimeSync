using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using Newtonsoft.Json;


namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TimeController : ControllerBase
    {
        public struct TimeData
        {
            public string date, time, datetime, day_of_week, day_of_year,
                   timezone;
        }
        // GET api/time
        [HttpGet]
        public ActionResult<string> Get()
        {
            DateTime dt = DateTime.Now;
            TimeZoneInfo localZone = TimeZoneInfo.Local;
            TimeData td;

            td.date = dt.ToShortDateString();
            td.time = dt.ToLongTimeString();
            td.day_of_week = dt.DayOfWeek.ToString();
            td.day_of_year = dt.DayOfYear.ToString();
            td.datetime = dt.ToString("yyyy-MM-ddTHH:mm:ss.fffffffK");
            td.timezone = localZone.StandardName;

            string json = JsonConvert.SerializeObject(td);


            return json;
            /*new string[] { "time", time,
                                  "timestamp", timestamp,
                                  "date", date,
                                  "Day of Week", day_of_week,
                                  "Day of Year", day_of_year,
                                  "DST",dst,
                                  "timezone", localZone.StandardName
            };*/
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
