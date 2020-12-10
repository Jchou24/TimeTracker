using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TimeTracker.DAL.Attributes;
using TimeTracker.DAL.DBModelsBase;

namespace TimeTracker.DAL.DBModels.Task
{
    /// <summary>
    /// 非工作日
    /// </summary>
    public class NonWorkDays: EntityDateInfo
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Guid { get; set; }

        /// <summary>
        /// 非工作日 年月日 year-month-day
        /// </summary>
        [DataType(DataType.Date)]
        [Column(TypeName = "Date")]
        [Required]
        public DateTime NonWorkDate { get; set; }
    }
}
