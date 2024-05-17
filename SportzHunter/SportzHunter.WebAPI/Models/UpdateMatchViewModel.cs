using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SportzHunter.WebAPI.Models
{
    public class UpdateMatchViewModel
    {
        public Guid TournamentId { get; set; }
        public Guid TeamHomeId { get; set; }
        public Guid TeamAwayId { get; set; }
        public string Result { get; set; }
        public DateTime DateUpdated { get; set; }
    }
}