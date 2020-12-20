using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using TimeTracker.Helper.Attributes;

namespace TimeTracker.Models.Task
{
    public class QueryTasks
    {
        [GuidNotEmpty]
        public Guid OwnerGuid { get; set; }

        [Required]
        [JsonConverter(typeof(DateFormatConverter))]
        public DateTime StartDate { get; set; }

        [Required]
        [JsonConverter(typeof(DateFormatConverter))]
        public DateTime EndDate { get; set; }
    }
}
