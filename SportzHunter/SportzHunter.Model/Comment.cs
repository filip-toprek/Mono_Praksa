using SportzHunter.Model.Common;
using System;

namespace SportzHunter.Model
{
    public class Comment : IComment
    {
        public Guid Id { get; set; }
        public Guid PlayerId { get; set; }
        public Guid TournamentId { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid UpdatedBy { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
        public string Text { get; set; }
        public Player Player { get; set; }
        public bool IsActive { get; set; }
    }
}
