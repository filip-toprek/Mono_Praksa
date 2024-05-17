using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SportzHunter.WebAPI.Models
{
    public class PostNewTournament
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int NumberOfTeams { get; set; }
        public string Location { get; set; }
        public DateTime Date { get; set; } 
    }
}