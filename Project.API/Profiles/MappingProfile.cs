using AutoMapper;
using Project.Data.DTO;
using Project.Data.Models;

namespace Project.API.Profiles
{
    public class MappingProfile : Profile
    {

        public MappingProfile()
        {
            CreateMap<UserProfile, UserProfileDTO>().ReverseMap();
        }
    }
}
