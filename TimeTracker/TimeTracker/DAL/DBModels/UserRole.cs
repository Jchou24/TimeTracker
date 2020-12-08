using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using TimeTracker.DAL.Models;

namespace TimeTracker.DAL.DBModels
{
    public class UserRole: BaseEntity
    {
        [Key]
        public int Id { get; set; }
        /// <summary>
        /// for matching program code
        /// </summary>
        [Required]
        [Column(TypeName = "NVARCHAR")]
        [StringLength(256)]
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
