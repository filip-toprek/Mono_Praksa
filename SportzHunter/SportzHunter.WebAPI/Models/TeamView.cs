using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SportzHunter.WebAPI.Models
{
    public class TeamView
    {

        public Guid Id { get; set; }
        public string TeamLeaderUsername { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public DateTime DateCreated { get; set; }
       
        public TeamView() { }
        public TeamView(Guid id, string teamleaderusername, string name, string description, DateTime datecreated)
        {
            Id = id;
            TeamLeaderUsername = teamleaderusername;
            Name = name;
            Description = description;
            DateCreated = datecreated;
           
        }
    }
}