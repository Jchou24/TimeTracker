using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TimeTracker.DAL.Attributes;
using TimeTracker.DAL.DBModelsBase;

namespace TimeTracker.DAL.DBModels.Task
{
    /// <summary>
    /// 工作時間上限
    /// </summary>
    public class DayWorkLimitTime: EntityDateInfo
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Guid { get; set; }

        /// <summary>
        /// 工作時間上限
        /// </summary>
        [Required]
        [Range(0.0, 24.0)]
        public double LimitWorkTime { get; set; } = 7.5;
    }
}
