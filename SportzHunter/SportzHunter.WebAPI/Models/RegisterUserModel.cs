using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SportzHunter.WebAPI.Models
{
    public class RegisterUserModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string VerifyPassword { get; set; }
        public string Email { get; set; }
        public Guid SportCategory { get; set; }
        public Guid? TeamId { get; set;}
        public decimal? Height { get; set; }
        public decimal? Weight { get; set; }
        public Guid? PreferredPositionId { get; set; }
        public DateTime DateOfBirth { get; set; }
        public Guid CountyId { get; set; }
        public string Description { get; set; }
    }
}