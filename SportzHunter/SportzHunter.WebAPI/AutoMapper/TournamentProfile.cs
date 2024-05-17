using AutoMapper;
using SportzHunter.Model;
using SportzHunter.WebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SportzHunter.WebAPI.AutoMapper
{
    public class TournamentProfile : Profile
    {
        public TournamentProfile()
        {
            CreateMap<Tournament, GetTournaments>();
            CreateMap<Tournament, GetTournamentByTeamId>();
            CreateMap<PutTournamentDetails, Tournament>();
            CreateMap<PostNewTournament, Tournament>();
            CreateMap<Tournament, TournamentBasicInfo>();
        }
    }
}