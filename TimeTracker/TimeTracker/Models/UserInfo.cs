using System;
using System.Collections.Generic;
using TimeTracker.DAL.DBModels.Auth;
using TimeTracker.DAL.Models;

namespace TimeTracker.Models
{
    public class UserInfo
    {
        public Guid Guid { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }

        public AccountStatus AccountStatus { get; set; }

        public ICollection<UserRole> UserRoles { get; set; }

        public UserInfo(User user)
        {
            this.Guid = user.Guid;
            this.Name = user.Name;
            this.Email = user.Email;
            this.AccountStatus = user.AccountStatus;
            this.UserRoles = user.UserRoles;
        }
    }
}
