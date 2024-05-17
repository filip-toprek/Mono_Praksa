using SportzHunter.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SportzHunter.WebAPI.Models
{
    public class PutPlayerDetails
    {

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public decimal? Height { get; set; }
        public decimal? Weight { get; set; }
        public Guid? PreferredPositionId { get; set; }
        public string Description { get; set; }
    }
}