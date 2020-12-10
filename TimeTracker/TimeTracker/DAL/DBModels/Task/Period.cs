using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TimeTracker.DAL.DBModelsBase;

namespace TimeTracker.DAL.DBModels.Task
{
    /// <summary>
    /// 週期
    /// </summary>
    public class Period: EntityDateInfo
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Guid { get; set; }

        /// <summary>
        /// 年月日 year-month-day
        /// </summary>
        [DataType(DataType.Date)]
        [Column(TypeName = "Date")]
        [Required]
        public DateTime StartDate { get; set; }

        /// <summary>
        /// 年月日 year-month-day
        /// </summary>
        [DataType(DataType.Date)]
        [Column(TypeName = "Date")]
        [Required]
        public DateTime EndDate { get; set; }

        [Column(TypeName = "NVARCHAR")]
        [StringLength(256)]
        public string Name { get; set; }
    }
}
