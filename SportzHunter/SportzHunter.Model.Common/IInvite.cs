using System;

namespace SportzHunter.Model.Common
{
    public interface IInvite
    {
        Guid Id { get; set; }
        Guid PlayerId { get; set; }
        Guid TeamId { get; set; }
        Guid InviteStatus { get; set; }
        Guid CreatedBy { get; set; }
        Guid UpdatedBy { get; set; }
        DateTime DateCreated { get; set; }
        DateTime DateUpdated { get; set; }
        bool IsActive { get; set; }
    }
}
