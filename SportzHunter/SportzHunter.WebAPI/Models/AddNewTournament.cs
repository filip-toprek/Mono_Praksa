using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SportzHunter.WebAPI.Models
{
    public class AddNewTournament
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int NumberOfTeams { get; set; }
        public string Location { get; set; }
        public DateTime Date { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid UpdatedBy { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdateded { get; set; }
        public bool IsActive { get; set; }

        public AddNewTournament(Guid id, string name, string description, int numberOfTeams, string location, DateTime date, Guid createdBy, Guid updatedBy, DateTime dateCreated, DateTime dateUpdateded, bool isActive)
        {
            Id = id;
            Name = name;
            Description = description;
            NumberOfTeams = numberOfTeams;
            Location = location;
            Date = date;
            CreatedBy = createdBy;
            UpdatedBy = updatedBy;
            DateCreated = dateCreated;
            DateUpdateded = dateUpdateded;
            IsActive = isActive;
        }
    }
}