using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TimeTracker.Helper.Attributes;

namespace TimeTracker.DAL.DBModelsBase
{
    public class EntityTaskBase: EntityDateInfo
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Guid { get; set; }

        /// <summary>
        /// 年月日 year-month-day
        /// </summary>
        [DataType(DataType.Date)]
        [Column(TypeName = "Date")]
        [Required]
        public DateTime Date { get; set; }
    }
}
