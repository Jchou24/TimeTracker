using AutoMapper;
using TimeTracker.DAL.DBModels.Auth;
using TimeTracker.Models;

namespace TimeTracker
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Add as many of these lines as you need to map your objects
            CreateMap<User, ApprovedUser>();
        }
    }
}
