using AutoMapper;
using SportzHunter.Model;
using SportzHunter.WebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SportzHunter.WebAPI.AutoMapper
{
    public class TeamAutoMapperProfile : Profile
    {
        public TeamAutoMapperProfile() {
            CreateMap<Team, TeamView>()
                .ForMember(dest => dest.TeamLeaderUsername, opt => opt.MapFrom(src => src.TeamLeaderName));
            CreateMap<CreateTeamViewModel, Team>();
            CreateMap<Team, GetTeamByIdViewModel>();
            CreateMap<UpdateTeamViewModel, Team>();
            CreateMap<Team, TeamViewById>()
                .ForMember(dest => dest.TeamLeaderUsername, opt => opt.MapFrom(src => src.TeamLeaderName));
            CreateMap<Player, GetTeamByIdViewModel>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.Username));
        }

    }
}