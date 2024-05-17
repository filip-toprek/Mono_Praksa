using System;

namespace SportzHunter.Model
{
    public class Role
    {
        public Role(Guid id, string roleName)
        {
            Id = id;
            RoleName = roleName;
        }

        public Guid Id { get; set; }
        public string RoleName { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
        public bool IsActive { get; set; }
    }
}
