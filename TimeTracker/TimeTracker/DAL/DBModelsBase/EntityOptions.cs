using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TimeTracker.DAL.Attributes;

namespace TimeTracker.DAL.DBModelsBase
{
    public class EntityOptions: EntityDateInfo
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Guid { get; set; }

        /// <summary>
        /// for matching program code
        /// </summary>
        [Required]
        //[Column(TypeName = "NVARCHAR")]
        [StringLength(256)]
        [Unique]
        public string CodeName { get; set; }

        //[Column(TypeName = "NVARCHAR")]
        [StringLength(256)]
        public string DisplayName { get; set; }

        public bool IsActive { get; set; } = true;
    }
}
