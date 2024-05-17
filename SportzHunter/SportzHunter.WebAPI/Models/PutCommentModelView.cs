using SportzHunter.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SportzHunter.WebAPI.Models
{
    public class PutCommentModelView
    {
        [Required]
        public string Text { get; set; }
        [Required]
        public bool isActive { get; set; }
    }
}