using System;

namespace SportzHunter.Model.Common
{
    public interface ITeamLeader
    {
        Guid Id { get; set; }
        Guid UserId { get; set; }
        Guid SportCategoryId { get; set; }
        string Description { get; set; }
        bool IsAccepted { get; set; }
        Guid CreatedBy { get; set; }
        Guid UpdatedBy { get; set; }
        DateTime DateCreated { get; set; }
        DateTime DateUpdated { get; set; }
        bool IsActive { get; set; }
    }
}
