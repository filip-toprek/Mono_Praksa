using System;

namespace SportzHunter.Common
{
    public class PlayerFiltering
    {
        public PlayerFiltering(Guid? positionId, Guid? countyId, int? ageCategory, Guid? sportCategoryId)
        {
            PositionId = positionId;
            CountyId = countyId;
            AgeCategory = ageCategory;
            SportCategoryId = sportCategoryId;
        }

        public Guid? PositionId { get; set; }
        public Guid? CountyId { get; set; }
        public int? AgeCategory { get; set; }
        public Guid? SportCategoryId { get; set; }
    }
}
