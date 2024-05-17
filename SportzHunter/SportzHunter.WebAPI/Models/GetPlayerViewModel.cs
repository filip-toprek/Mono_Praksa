using Microsoft.Win32;
using SportzHunter.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SportzHunter.WebAPI.Models
{
    public class GetPlayerViewModel
    {
        public GetPlayerViewModel()
        {
        }

        public GetPlayerViewModel(string firstName, string lastName, string username, Guid id, decimal? height, decimal? weight, PreferredPosition preferredPosition, DateTime dateOfBirth, County county, string description)
        {
            FirstName = firstName;
            LastName = lastName;
            Username = username;
            Id = id;
            Height = height;
            Weight = weight;
            PreferredPosition = preferredPosition;
            DateOfBirth = dateOfBirth;
            County = county;
            Description = description;
        }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public Guid Id { get; set; }
        public decimal? Height { get; set; }
        public decimal? Weight { get; set; }
        public PreferredPosition PreferredPosition { get; set; }
        public DateTime DateOfBirth { get; set; }
        public County County { get; set; }
        public string Description { get; set; }


    }
}