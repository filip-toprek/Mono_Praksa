using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SportzHunter.WebAPI.Models
{
    public class TournamentBasicInfo
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int NumberOfTeams { get; set; }
        public int Attendees { get; set; }
        public string Location { get; set; }
        public DateTime Date { get; set; }

        public TournamentBasicInfo(string name, string description, int numberOfTeams, string location, DateTime date, int attendees)
        {
            Name = name;
            Description = description;
            NumberOfTeams = numberOfTeams;
            Location = location;
            Date = date;
            Attendees = attendees;
        }
    }
}