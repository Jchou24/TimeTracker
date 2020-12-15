using System;
using System.Collections.Generic;

namespace TimeTracker.Models.Task
{
    public class CreateTask
    {
        public List<TimeTracker.DAL.DBModels.Task.Task> Tasks { get; set; }

        public Guid OwnerGuid { get; set; }
    }
}
