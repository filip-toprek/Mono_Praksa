using SportzHunter.Model.Common;
using System;

namespace SportzHunter.Model
{
    public class InviteStatus : IInviteStatus
    {
        public Guid Id { get; set; }
        public string StatusName { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
        public bool IsActive { get; set; }
    }
}
