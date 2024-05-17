using AutoMapper;
using SportzHunter.Model;
using SportzHunter.WebAPI.Models;

namespace SportzHunter.WebAPI.AutoMapper
{
    public class UserProfile : Profile
    {
        public UserProfile() 
        {
            CreateMap<RegisterUserModel, User>()
                .ForMember(dest => dest.PasswordHash, opt => opt.MapFrom(src => src.Password));
        }
    }
}