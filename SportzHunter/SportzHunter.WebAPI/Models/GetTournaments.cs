using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;

namespace SportzHunter.WebAPI.Models
{
    public class GetTournaments
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int? NumberOfTeams { get; set; }
        public string Location { get; set; }
        public DateTime? Date { get; set; }
        public DateTime DateCreated { get; set; }

        public GetTournaments(Guid id, string name, string description, int? numberOfTeams, string location, DateTime? date, DateTime dateCreated)
        {
            Id = id;
            Name = name;
            Description = description;
            NumberOfTeams = numberOfTeams;
            Location = location;
            Date = date;
            DateCreated = dateCreated;
        }
    }
}