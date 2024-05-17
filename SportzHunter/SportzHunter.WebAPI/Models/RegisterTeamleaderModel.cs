using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SportzHunter.WebAPI.Models
{
    public class RegisterTeamleaderModel
    {
        public Guid PlayerId { get; set; }
        public string LeaderDescription { get; set; }
        public string TeamName { get; set; }
        public string TeamDescription { get; set; }
        public Guid SportCategoryId { get; set; }
    }
}