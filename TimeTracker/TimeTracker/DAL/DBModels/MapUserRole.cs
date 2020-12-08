using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using TimeTracker.DAL.Models;

namespace TimeTracker.DAL.DBModels
{
    public class MapUserRole: BaseEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }


        public int UserRolesId { get; set; }
        public UserRole UserRoles { get; set; }
    }
}
