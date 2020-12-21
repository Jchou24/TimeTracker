using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using TimeTracker.Helper.Attributes;

namespace TimeTracker.Models.Task
{
    public class QueryPeopleTasks
    {
        [ListNotEmpty(ErrorMessage ="At least a person is required")]
        public List<Guid> OwnerGuids { get; set; }

        [Required]
        [JsonConverter(typeof(DateFormatConverter))]
        public DateTime StartDate { get; set; }

        [Required]
        [JsonConverter(typeof(DateFormatConverter))]
        public DateTime EndDate { get; set; }

        public QueryPeopleTasks(){}

        public QueryPeopleTasks(QueryTasks queryTasks)
        {
            this.OwnerGuids = (new[] { queryTasks.OwnerGuid }).ToList();
            this.StartDate = queryTasks.StartDate;
            this.EndDate = queryTasks.EndDate;
        }
    }
}
