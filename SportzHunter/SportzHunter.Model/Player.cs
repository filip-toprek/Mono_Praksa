using SportzHunter.Model.Common;
using System;

namespace SportzHunter.Model
{
    public class Player : IUser
    {
        public Player()
        {

        }
        public Player(User user, decimal? height, decimal? weight, Guid? preferredPositionId, string description)
        {
            User = user;
            Height = height;
            Weight = weight;
            PreferredPositionId = preferredPositionId;
            Description = description;
        }

        public Player(Guid id, User user, Guid? team, decimal? height, decimal? weight, Guid? preferredPositionId, DateTime dateOfBirth, Guid countyId, Guid sportCategoryId, string description)
        {
            Id = id;
            User = user;
            TeamId = team;
            Height = height;
            Weight = weight;
            PreferredPositionId = preferredPositionId;
            DateOfBirth = dateOfBirth;
            CountyId = countyId;
            SportCategoryId = sportCategoryId;
            Description = description;
            CreatedBy = CreatedBy;
            UpdatedBy = UpdatedBy;
            DateCreated = DateCreated;
            DateUpdated = DateUpdated;
            IsActive = IsActive;
        }

        public Player(Guid id, Guid userid, string username)
        {
            Id = id;
            Username = username;
            UserId = userid;
        }

        public Guid Id { get; set; }
        public User User { get; set; }
        public Guid? TeamId { get; set; }
        public Team Team { get; set; }
        public decimal? Height { get; set; }
        public decimal? Weight { get; set; }
        public Guid? PreferredPositionId { get; set; }
        public PreferredPosition PreferredPosition { get; set; }
        public DateTime DateOfBirth { get; set; }
        public Guid CountyId { get; set; }
        public County County { get; set; }
        public Guid SportCategoryId { get; set; }
        public string Description { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid UpdatedBy { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
        public bool IsActive { get; set; }

        public string Username {  get; set; }
        public Guid UserId { get; set; }

    }
}
