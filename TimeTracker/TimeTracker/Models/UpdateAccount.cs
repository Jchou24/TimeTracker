using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TimeTracker.DAL.DBModels;
using TimeTracker.DAL.Models;

namespace TimeTracker.Models
{
    public class UpdateAccount
    {
        [Required]
        public int Id { get; set; }

        public string Name { get; set; }
        public bool IsUpdateName { get; set; }

        public AccountStatus AccountStatus { get; set; }
        public bool IsUpdateAccountStatus { get; set; }

        public ICollection<int> UserRoles { get; set; }
        public bool IsUpdateUserRoles { get; set; }

        public void DecryptId()
        { 
        
        }
    }
}
