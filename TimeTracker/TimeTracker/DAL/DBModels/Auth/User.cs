using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using TimeTracker.DAL.Attributes;
using TimeTracker.DAL.DBModels.Task;
using TimeTracker.DAL.DBModelsBase;
using TimeTracker.DAL.Models;
using TimeTracker.Helper.Auth;

namespace TimeTracker.DAL.DBModels.Auth
{
    public class User: EntityDateInfo
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Unique]
        public Guid Guid { get; set; }

        [Column(TypeName = "NVARCHAR")]
        [StringLength(256)]
        public string Name { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [Column(TypeName = "NVARCHAR")]
        [StringLength(256)]
        [Unique]
        public string Email { get; set; }
        [Required]
        public string PasswordHash { get; set; }
        [Required]
        public AccountStatus AccountStatus { get; set; }

        public ICollection<UserRole> UserRoles { get; set; }
        [JsonIgnore]
        public List<MapUserRole> MapUserRoles { get; set; }

        public ICollection<TaskDay> TaskDay { get; set; }
        //public ICollection<Task> Task { get; set; }
        //public ICollection<TaskTimeRange> TaskTimeRange { get; set; }

        public User()
        {

        }

        public User(string email, string password)
        {
            this.Email = email;
            this.PasswordHash = SecurePasswordHasher.Hash(password);            
            this.AccountStatus = AccountStatus.Uncheck;
        }
    }
}
