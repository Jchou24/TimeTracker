using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using TimeTracker.DAL.Models;
using TimeTracker.Helper.Auth;

namespace TimeTracker.DAL.DBModels
{
    public class User
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public AccountStatus AccountStatus { get; set; }


        public ICollection<UserRole> UserRoles { get; set; }
        [JsonIgnore]
        public List<MapUserRole> MapUserRoles { get; set; }

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
