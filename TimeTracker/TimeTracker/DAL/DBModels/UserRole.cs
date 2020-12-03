using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TimeTracker.DAL.DBModels
{
    public class UserRole
    {
        [Key]
        public int Id { get; set; }
        /// <summary>
        /// for matching program code
        /// </summary>
        public string CodeName { get; set; }
        public string DisplayName { get; set; }
        [JsonIgnore]
        public ICollection<User> Users { get; set; }
        [JsonIgnore]
        public List<MapUserRole> MapUserRoles { get; set; }
    }
}
