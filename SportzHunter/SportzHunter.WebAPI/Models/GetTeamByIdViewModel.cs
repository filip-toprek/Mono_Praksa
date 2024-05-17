using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SportzHunter.WebAPI.Models
{
    public class GetTeamByIdViewModel
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
    }
}