using System;

namespace SportzHunter.Common
{
    public class TeamFiltering
    {
        private string name;
        private string description;
        private Guid? teamleaderid;
        private DateTime? datecreated;
        private Guid sportcategoryid;
        public TeamFiltering(string name, string description, Guid? teamleaderid, DateTime? datecreated)
        {
            this.name = name;
            this.description = description;
            this.teamleaderid = teamleaderid;
            this.datecreated = datecreated;
            
        }

        public string Name { get { return name; } set { name = value; } }
        public string Description { get { return description; } set { description = value; } }
        public Guid? TeamleaderId { get { return teamleaderid; } set { teamleaderid = value; } }

        public DateTime? DateCreated { get { return datecreated; } set { datecreated = value; } }
       public Guid SportCategoryId {  get { return sportcategoryid; } set {  sportcategoryid = value; } }
    }
}
