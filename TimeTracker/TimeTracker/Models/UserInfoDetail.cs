using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TimeTracker.DAL.DBModels;

namespace TimeTracker.Models
{
    public class UserInfoDetail: UserInfo
    {
        public int Id { get; set; }
        public UserInfoDetail(User user): base(user)
        {
            this.Id = user.Id; // TODO need encrypt
        }
    }
}
