using System;
using System.ComponentModel.DataAnnotations;
using TimeTracker.DAL.DBModelsBase;

namespace TimeTracker.DAL.DBModels.Task
{
    /// <summary>
    /// 一個Task的資訊
    /// </summary>
    public class Task: EntityTask
    {
        /// <summary>
        /// 花費時數
        /// </summary>
        [Required]
        [Range(0.0, 24.0)]
        public double ConsumeTime { get; set; }

        public TaskDay TaskDay { get; set; }
    }
}
