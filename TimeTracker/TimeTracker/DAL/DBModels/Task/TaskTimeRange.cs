using System;
using System.ComponentModel.DataAnnotations;
using TimeTracker.DAL.DBModelsBase;

namespace TimeTracker.DAL.DBModels.Task
{
    public class TaskTimeRange: EntityTask
    {
        /// <summary>
        /// 起始時間
        /// </summary>
        [Required]
        public DateTime StartTime { get; set; }

        /// <summary>
        /// 結束時間
        /// </summary>
        [Required]
        public DateTime EndTime { get; set; }

        public TaskDay TaskDay { get; set; }
    }
}
