using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SportzHunter.WebAPI.Models
{
    public class PostCommentModelView
    {
        [Required]
        public Guid TournamentId { get; set; }
        [Required]
        public string Text { get; set; }
    }
}