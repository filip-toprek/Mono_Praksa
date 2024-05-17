using System;

namespace SportzHunter.Model.Common
{
    public interface ISportCategory
    {
        Guid Id { get; set; }
        string CategoryName { get; set; }
        DateTime DateCreated { get; set; }
        DateTime DateUpdated { get; set; }
        bool IsActive { get; set; }

    }
}
