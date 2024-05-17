using SportzHunter.Model.Common;
using System;

namespace SportzHunter.Model
{
    public class Attend : IAttend
    {
        public Guid Id { get; set; }
        public Guid TournamentId { get; set; }
        public Guid TeamId { get; set; }
        public bool IsApproved { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid UpdatedBy { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
        public Team Team { get; set; }
        public Tournament Tournament { get; set; }
        public bool IsActive { get; set; }

        public Attend(Guid id, Guid tournamentId, Guid teamId, bool isApproved, Guid createdBy, Guid updatedBy, DateTime dateCreated, DateTime dateUpdated, bool isActive)
        {
            Id = id;
            TournamentId = tournamentId;
            TeamId = teamId;
            IsApproved = isApproved;
            CreatedBy = createdBy;
            UpdatedBy = updatedBy;
            DateCreated = dateCreated;
            DateUpdated = dateUpdated;
            IsActive = isActive;
        }

        public Attend(Guid id, Guid tournamentId, Guid teamId)
        {
            Id = id;
            TournamentId = tournamentId;
            TeamId = teamId;
        }

        public Attend()
        {
        }
    }
}
