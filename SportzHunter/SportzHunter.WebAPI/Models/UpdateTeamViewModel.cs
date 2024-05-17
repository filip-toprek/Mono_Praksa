using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SportzHunter.WebAPI.Models
{
    public class UpdateTeamViewModel
    {
        public Guid TeamLeaderId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

       
        public DateTime DateUpdated { get; set; }
    }
}