using AutoMapper;
using SportzHunter.Model;
using SportzHunter.WebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SportzHunter.WebAPI.AutoMapper
{
    public class MatchAutoMapper : Profile
    {
        public MatchAutoMapper() {

            CreateMap<CreateMatchViewModel, Match>();
            CreateMap<UpdateMatchViewModel, Match>();
            CreateMap<Match, MatchView>()
                 .ForMember(dest => dest.TeamAway, opt => opt.MapFrom(src => src.TeamAwayName))
                 .ForMember(dest => dest.TeamHome, opt => opt.MapFrom(src => src.TeamHomeName));
            CreateMap<Match, MatchDetailsViewByTournament>();



        }
    }
}