using System;

namespace SportzHunter.Model.Common
{
    public interface IInviteStatus
    {
        Guid Id { get; set; }
        string StatusName { get; set; }
        DateTime DateCreated { get; set; }
        DateTime DateUpdated { get; set; }
        bool IsActive { get; set; }
    }
}
