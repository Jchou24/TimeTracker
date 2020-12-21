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

        public bool IsLeave { get; set; } = false;

        public SimpleSummary(){}

        public SimpleSummary(MultiSimpleSummary multiSimpleSummary)
        {
            this.Date = multiSimpleSummary.Date;
            this.ConsumeTime = multiSimpleSummary.TotalConsumeTime;
            this.Overtime = multiSimpleSummary.TotalOvertime;
        }

        public SimpleSummary(PersonSummary personSummary)
        {
            this.Date = personSummary.Date;
            this.ConsumeTime = personSummary.ConsumeTime;
            this.Overtime = personSummary.Overtime;
            this.IsLeave = personSummary.IsLeave;
        }
    }
}
