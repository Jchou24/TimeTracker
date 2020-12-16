using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using TimeTracker.Helper.Attributes;

namespace TimeTracker.Models.Task
{
    public class QueryTasks
    {
        public Guid OwnerGuid { get; set; }

        [JsonConverter(typeof(DateFormatConverter))]
        public DateTime StartDate { get; set; }

        [JsonConverter(typeof(DateFormatConverter))]
        public DateTime EndDate { get; set; }
    }
}
