using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SportzHunter.WebAPI.Models
{
    public class JoinTournament
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int NumberOfTeams { get; set; }
        public string Location { get; set; }
        public DateTime Date { get; set; }

        public JoinTournament(string name, string description, int numberOfTeams, string location, DateTime date)
        {
            Name = name;
            Description = description;
            NumberOfTeams = numberOfTeams;
            Location = location;
            Date = date;
        }
    }
}