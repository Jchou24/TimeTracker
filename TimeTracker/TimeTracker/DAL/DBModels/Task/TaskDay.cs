using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TimeTracker.DAL.DBModels.Auth;
using TimeTracker.DAL.DBModelsBase;

namespace TimeTracker.DAL.DBModels.Task
{
    public class TaskDay: EntityTaskBase
    {
        /// <summary>
        /// 是否請假
        /// </summary>
        [Required]
        public bool IsLeave { get; set; }

        public User User { get; set; }

        public ICollection<Task> Task { get; set; }

        public ICollection<TaskTimeRange> TaskTimeRange { get; set; }
    }
}
