using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SportzHunter.WebAPI.Models
{
    public class PostInviteModel
    {
        [Required]
        public Guid UserId { get; set; }
        [Required]
        public Guid TeamId { get; set; }
    }
}