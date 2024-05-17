using System;

namespace SportzHunter.Model.Common
{
    public interface IComment
    {
        Guid Id { get; set; }
        Guid PlayerId { get; set; }
        Guid TournamentId { get; set; }
        Guid CreatedBy { get; set; }
        Guid UpdatedBy { get; set; }
        DateTime DateCreated { get; set; }
        DateTime DateUpdated { get; set; }
        string Text { get; set; }
        bool IsActive { get; set; }

    }
}
