﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SportzHunter.WebAPI.Models
{
    public class PostUserViewModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string VerifyPassword { get; set; }
        public string Email { get; set; }
        public Guid SportCategory { get; set; }
    }
}