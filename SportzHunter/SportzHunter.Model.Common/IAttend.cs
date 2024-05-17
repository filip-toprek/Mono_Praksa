using System;

namespace SportzHunter.Model.Common
{
    public interface IAttend
    {
        Guid Id { get; set; }
        Guid TournamentId { get; set; }
        Guid TeamId { get; set; }
        bool IsApproved { get; set; }
        Guid CreatedBy { get; set; }
        Guid UpdatedBy { get; set; }
        DateTime DateCreated { get; set; }
        DateTime DateUpdated { get; set; }
        bool IsActive { get; set; }
    }
}
