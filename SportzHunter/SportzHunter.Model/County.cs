using System;

namespace SportzHunter.Model
{
    public class County
    {
        public County()
        {
        }

        public County(Guid id, string countyName, DateTime dateCreated, DateTime dateUpdated, bool isActive)
        {
            Id = id;
            CountyName = countyName;
            DateCreated = dateCreated;
            DateUpdated = dateUpdated;
            IsActive = isActive;
        }

        public Guid Id { get; set; }
        public string CountyName { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
        public bool IsActive { get; set; }
    }
}
