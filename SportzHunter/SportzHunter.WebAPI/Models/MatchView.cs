using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SportzHunter.WebAPI.Models
{
    public class MatchView
    {
        public Guid Id { get; set; }
        public string TournamentName { get; set; }
        public string TeamHome { get; set; }
        public string TeamAway { get; set; }
        public Guid TeamHomeId {  get; set; }
        public Guid TeamAwayId {  get; set; }
        public DateTime DateCreated { get; set; }

        public string Result {  get; set; }

        public MatchView() { }
        public MatchView(Guid id, string tournamentName, string teamHome, string teamAway, string result)
        {
            Id = id;
            TournamentName = tournamentName;
            TeamHome = teamHome;
            TeamAway = teamAway;
            Result = result;
        }
    }
}