using SportzHunter.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SportzHunter.WebAPI.Models
{
    public class TeamViewById
    {
        public Guid Id { get; set; }
        public string TeamLeaderUsername { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public DateTime DateCreated { get; set; }

        public List<GetTeamByIdViewModel> PlayerList { get; set; }
        public TeamViewById() { }
        public TeamViewById(Guid id, string teamleaderusername, string name, string description, DateTime datecreated)
        {
            Id = id;
            TeamLeaderUsername = teamleaderusername;
            Name = name;
            Description = description;
            DateCreated = datecreated;

        }
    }
}