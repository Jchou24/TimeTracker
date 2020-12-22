using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using TimeTracker.Helper.Attributes;

namespace TimeTracker.Models.Period
{
    public class QueryPeriod
    {
        [GuidNotEmpty]
        public Guid Guid { get; set; }

        [JsonConverter(typeof(DateFormatConverter))]
        public DateTime StartDate { get; set; }

        [JsonConverter(typeof(DateFormatConverter))]
        public DateTime EndDate { get; set; }

        [StringLength(256)]
        public string Name { get; set; }
    }
}
