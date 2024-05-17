using AutoMapper;
using SportzHunter.Model;
using SportzHunter.WebAPI.Models;

namespace SportzHunter.WebAPI.AutoMapper
{
    public class PlayerProfile : Profile
    {
        public PlayerProfile() 
        {
            CreateMap<Player, GetPlayerViewModel>()
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.User.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.User.LastName))
                .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.User.Username));
            CreateMap<RegisterUserModel, Player>()
                .ForMember(dest => dest.SportCategoryId, opt => opt.MapFrom(src => src.SportCategory));
            CreateMap<PutPlayerDetails, Player>();

        }
    }
}