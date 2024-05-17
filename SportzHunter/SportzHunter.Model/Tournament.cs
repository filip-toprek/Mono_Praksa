using SportzHunter.Model.Common;
using System;

namespace SportzHunter.Model
{
    public class Tournament : ITournament
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
        public DateTime DateUpdated { get; set; }
        public bool IsActive { get; set; }
        public int Attendees { get; set; }

        public Tournament(Guid id, string name, string description, int numberOfTeams, string location, DateTime date, Guid createdBy, Guid updatedBy, DateTime dateCreated, DateTime dateUpdated, bool isActive, int attendees)
        {
            Id = id;
            Name = name;
            Description = description;
            NumberOfTeams = numberOfTeams;
            Location = location;
            Date = date;
            Attendees = attendees;
            CreatedBy = createdBy;
            UpdatedBy = updatedBy;
            DateCreated = dateCreated;
            DateUpdated = dateUpdated;
            IsActive = isActive;
        }

        public Tournament() { }

        public Tournament(string name, string description, int numberOfTeams, string location, DateTime date)
        {
            Name = name;
            Description = description;
            NumberOfTeams = numberOfTeams;
            Location = location;
            Date = date;
        }

        public Tournament(string name, string description, int numberOfTeams, string location, DateTime date, DateTime dateCreated)
        {
            Name = name;
            Description = description;
            NumberOfTeams = numberOfTeams;
            Location = location;
            Date = date;
            DateCreated = dateCreated;
        }

    }
}
