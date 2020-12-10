using System.ComponentModel.DataAnnotations.Schema;
using TimeTracker.DAL.DBModels.Auth;
using TimeTracker.DAL.DBModelsBase;

namespace TimeTracker.DAL.DBModels.Auth
{
    public class MapUserRole: EntityDateInfo
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }


        public int UserRolesId { get; set; }
        public UserRole UserRoles { get; set; }
    }
}
