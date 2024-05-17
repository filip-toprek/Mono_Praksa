using SportzHunter.Model.Common;
using System;

namespace SportzHunter.Model
{
    public class TeamLeader : ITeamLeader
    {
        private Guid id;
        private Guid userid;
        private Guid sportcategoryid;
        private string description;
        private bool isaccepted;
        private Guid createdby;
        private Guid updatedby;
        private DateTime datecreated;
        private DateTime dateupdated;
        private bool isactive;
        private Team team;

        public TeamLeader() { }
        public TeamLeader(Guid id, Guid userid, Guid sportcategoryid, string description, bool isaccepted, Guid createdby, Guid updatedby, DateTime datecreated, DateTime dateupdated, bool isactive)
        {
            this.id = id;
            this.userid = userid;
            this.sportcategoryid = sportcategoryid;
            this.description = description;
            this.isaccepted = isaccepted;
            this.createdby = createdby;
            this.updatedby = updatedby;
            this.datecreated = datecreated;
            this.dateupdated = dateupdated;
            this.isactive = isactive;
        }

        public Guid Id { get { return id; } set { id = value; } }
        public Guid UserId { get { return userid; } set { userid = value; } }
        public Guid SportCategoryId { get { return sportcategoryid; } set { sportcategoryid = value; } }
        public string Description { get { return description; } set { description = value; } }
        public bool IsAccepted { get { return isaccepted; } set { isaccepted = value; } }
        public Guid CreatedBy { get { return createdby; } set { createdby = value; } }
        public Guid UpdatedBy { get { return updatedby; } set { updatedby = value; } }
        public DateTime DateCreated { get { return datecreated; } set { datecreated = value; } }
        public DateTime DateUpdated { get { return dateupdated; } set { dateupdated = value; } }
        public bool IsActive { get { return isactive; } set { isactive = value; } }
        public Team Team { get { return team; }  set { team = value; } }
        public string Username { get; set; }
    }
}
