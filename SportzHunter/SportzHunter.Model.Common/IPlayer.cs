using System;

namespace SportzHunter.Model.Common
{
    public interface IPlayer
    {
        Guid Id { get; set; }
        IUser User { get; set; }
        Guid? TeamId { get; set; }
        decimal? Height { get; set; }
        decimal? Weight { get; set; }
        Guid? PreferredPositionId { get; set; }
        DateTime DateOfBirth { get; set; }
        Guid CountyId { get; set; }
        Guid SportCategory { get; set; }
        string Description { get; set; }
        Guid CreatedBy { get; set; }
        Guid UpdatedBy { get; set; }
        DateTime DateCreated { get; set; }
        DateTime DateUpdated { get; set; }
        bool IsActive { get; set; }
    }
}
