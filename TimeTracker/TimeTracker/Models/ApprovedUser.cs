using AutoMapper;
using System;
using TimeTracker.DAL.DBModels.Auth;

namespace TimeTracker.Models
{
    public class ApprovedUser
    {
        public Guid Guid { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public ApprovedUser(User user)
        {
            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<User, ApprovedUser>();
            });
            IMapper mapper = mapperConfig.CreateMapper();
            mapper.Map(user, this);
        }
    }
}
