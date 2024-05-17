using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SportzHunter.WebAPI.Models
{
    public class MatchDetailsViewByTournament
    {
        public Guid Id { get; set; }
        public string TeamHomeName { get; set; }
        public string TeamAwayName { get; set; }
        public string Result { get; set; }
    }
}