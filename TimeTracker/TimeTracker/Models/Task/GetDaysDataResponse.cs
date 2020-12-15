using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using TimeTracker.Helper.Attributes;

namespace TimeTracker.Models.Task
{
    public class GetDaysDataResponse
    {
        public Guid Guid { get; set; }

        [JsonConverter(typeof(DateFormatConverter))]
        public DateTime Date { get; set; }

        public bool IsLeave { get; set; }

        public List<DAL.DBModels.Task.Task> FormData { get; set; }
    }
}
