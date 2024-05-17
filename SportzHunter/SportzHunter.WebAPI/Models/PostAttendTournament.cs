using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SportzHunter.WebAPI.Models
{
    public class PostAttendTournament
    {
        public Guid TournamentId { get; set; }
        public Guid TeamId { get; set; }

        public PostAttendTournament(Guid tournamentId, Guid teamId)
        {
            TournamentId = tournamentId;
            TeamId = teamId;
        }
    }
}