using System;

namespace SportzHunter.Model.Common
{
    public interface ITeam
    {
        Guid Id { get; set; }
        Guid TeamLeaderId { get; set; }
        string Name { get; set; }
        string Description { get; set; }
        Guid CreatedBy { get; set; }
        Guid UpdatedBy { get; set; }
        DateTime DateCreated { get; set; }
        DateTime DateUpdated { get; set; }
        bool IsActive { get; set; }


    }
}
