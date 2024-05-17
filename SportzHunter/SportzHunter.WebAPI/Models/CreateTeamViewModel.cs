using SportzHunter.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace SportzHunter.WebAPI.Models
{
    public class CreateTeamViewModel
    {
        public Guid Id { get; set; }
        public Guid TeamLeaderId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
       
       public DateTime DateCreated { get; set; }
    }
}