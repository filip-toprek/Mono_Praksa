using System;

namespace SportzHunter.Model.Common
{
    public interface ITournament
    {
        Guid Id { get; set; }
        string Name { get; set; }
        string Description { get; set; }
        int NumberOfTeams { get; set; }
        string Location { get; set; }
        DateTime Date { get; set; }
        Guid CreatedBy { get; set; }
        Guid UpdatedBy { get; set; }
        DateTime DateCreated { get; set; }
        DateTime DateUpdated { get; set; }
        bool IsActive { get; set; }
    }
}
