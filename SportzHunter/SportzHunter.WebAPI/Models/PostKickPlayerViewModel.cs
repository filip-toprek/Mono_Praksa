using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SportzHunter.WebAPI.Models
{
    public class PostKickPlayerViewModel
    {
        public Guid TeamId { get; set; }
        public Guid PlayerId { get; set; }
    }
}