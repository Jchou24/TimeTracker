using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using TimeTracker.Helper.Attributes;

namespace TimeTracker.Models.Task
{
    public class SimpleSummary
    {
        [JsonConverter(typeof(DateFormatConverter))]
        public DateTime Date { get; set; }

        public double ConsumeTime { get; set; }

        public double Overtime { get; set; }

        public SimpleSummary(){}

        public SimpleSummary(DateTime date, double consumeTime, double limitWorkTime)
        {
            this.Date = date;
            this.ConsumeTime = consumeTime;
            this.Overtime = consumeTime - limitWorkTime > 0 ? consumeTime - limitWorkTime : 0;
        }
    }
}
