using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportzHunter.Model
{
    public class CommentView
    {
        public Guid Id { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
        public string Text { get; set; }
        public string Username { get; set; }

        public CommentView(Comment comment)
        {
            this.DateCreated = comment.DateCreated;
            this.DateUpdated = comment.DateUpdated;
            this.Text = comment.Text;
            this.Username = comment.Player.User.Username;
        }

        public CommentView()
        {
        }
    }
}
