using AutoMapper;
using SportzHunter.Model;
using SportzHunter.WebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SportzHunter.WebAPI.AutoMapper
{
    public class TeamleaderProfile : Profile
    {
        public TeamleaderProfile() 
        { 
            CreateMap<RegisterTeamleaderModel, TeamLeader>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.PlayerId))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.LeaderDescription));
            CreateMap<RegisterTeamleaderModel, Team>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.TeamName))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.TeamDescription));
        }

    }
}