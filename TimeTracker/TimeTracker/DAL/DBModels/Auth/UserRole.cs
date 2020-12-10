using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using TimeTracker.DAL.Attributes;
using TimeTracker.DAL.DBModelsBase;

namespace TimeTracker.DAL.DBModels.Auth
{
    public class UserRole: EntityDateInfo
    {
        [Key]
        public int Id { get; set; }
        /// <summary>
        /// for matching program code
        /// </summary>
        [Required]
        [Column(TypeName = "NVARCHAR")]
        [StringLength(256)]
        [Unique]
        public string CodeName { get; set; }
        [Column(TypeName = "NVARCHAR")]
        [StringLength(256)]
        public string DisplayName { get; set; }
        [JsonIgnore]
        public ICollection<User> Users { get; set; }
        [JsonIgnore]
        public List<MapUserRole> MapUserRoles { get; set; }
    }
}
