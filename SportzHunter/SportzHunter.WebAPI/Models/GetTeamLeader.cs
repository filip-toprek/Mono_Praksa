using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SportzHunter.WebAPI.Models
{
    public class GetTeamLeader
    {
        public Guid TeamLeaderId { get; set; }
        public string TeamLeaderName { get; set; }
    }
}