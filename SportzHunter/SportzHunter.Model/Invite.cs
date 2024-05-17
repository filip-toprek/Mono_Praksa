using SportzHunter.Model.Common;
using System;

namespace SportzHunter.Model
{
    public class Invite : IInvite
    {
        public Guid Id { get; set; }
        public Guid PlayerId { get; set; }
        public Guid TeamId { get; set; }
        public Team Team { get; set; }
        public Player Player { get; set; }
        public Guid InviteStatus { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid UpdatedBy { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
        public bool IsActive { get; set; }
    }
}
