using SportzHunter.Model.Common;
using System;

namespace SportzHunter.Model
{
    public class User : IUser
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public Role UserRole { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid UpdatedBy { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
        public bool IsActive { get; set; }

        public User(Guid id, string firstName, string lastName, string username, string passwordHash, Guid createdBy, Guid updatedBy, DateTime dateCreated, DateTime dateUpdated, bool isActive)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            Username = username;
            PasswordHash = passwordHash;
            CreatedBy = createdBy;
            UpdatedBy = updatedBy;
            DateCreated = dateCreated;
            DateUpdated = dateUpdated;
            IsActive = isActive;
        }
        public User(string username, string passwordHash)
        {
            Username = username;
            PasswordHash = passwordHash;
        }

        public User(Guid id, string firstName, string lastName)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
        }
        public User(Guid id, string firstName, string lastName, string username)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            Username = username;
        }
        public User() { }
    }
}
