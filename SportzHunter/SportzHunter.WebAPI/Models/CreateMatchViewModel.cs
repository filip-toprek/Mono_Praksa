using SportzHunter.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Results;

namespace SportzHunter.WebAPI.Models
{
    public class CreateMatchViewModel
    {
        public Guid TournamentId { get; set; }
        public Guid TeamHomeId { get; set; }
        public Guid TeamAwayId { get; set; }
        public string Result { get; set; }
        public DateTime DateCreated { get; set; }
    }
}