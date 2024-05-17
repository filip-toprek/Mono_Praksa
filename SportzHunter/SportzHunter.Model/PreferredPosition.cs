using System;

namespace SportzHunter.Model
{
    public class PreferredPosition
    {
        public PreferredPosition()
        {
        }

        public PreferredPosition(Guid id, string positionName, DateTime dateCreated, DateTime dateUpdated, bool isActive)
        {
            Id = id;
            PositionName = positionName;
            DateCreated = dateCreated;
            DateUpdated = dateUpdated;
            IsActive = isActive;
        }

        public Guid Id { get; set; }
        public string PositionName { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
        public bool IsActive { get; set; }
    }
}