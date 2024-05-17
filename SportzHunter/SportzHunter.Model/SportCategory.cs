using SportzHunter.Model.Common;
using System;

namespace SportzHunter.Model
{
    public class SportCategory : ISportCategory
    {
        private Guid id;
        private string categoryname;
        private DateTime datecreated;
        private DateTime dateupdated;
        private bool isactive;

        public SportCategory() { }
        public SportCategory(Guid id, string categoryname, DateTime datecreated, DateTime dateupdated, bool isactive)
        {
            this.id = id;
            this.categoryname = categoryname;
            this.datecreated = datecreated;
            this.dateupdated = dateupdated;
            this.isactive = isactive;
        }

        public Guid Id { get { return id; } set { id = value; } }
        public string CategoryName { get { return categoryname; } set { categoryname = value; } }
        public DateTime DateCreated { get { return datecreated; } set { datecreated = value; } }
        public DateTime DateUpdated { get { return dateupdated; } set { dateupdated = value; } }
        public bool IsActive { get { return isactive; } set { isactive = value; } }
    }
}
