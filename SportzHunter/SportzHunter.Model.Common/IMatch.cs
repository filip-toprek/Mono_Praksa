using System;

namespace SportzHunter.Model.Common
{
    public interface IMatch
    {
        Guid Id { get; set; }
        Guid TournamentId { get; set; }
        Guid TeamHomeId { get; set; }
        Guid TeamAwayId { get; set; }
        string Result { get; set; }
        DateTime DateCreated { get; set; }
        DateTime DateUpdated { get; set; }
        bool IsActive { get; set; }

    }
}
