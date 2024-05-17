using SportzHunter.Model.Common;
using System;
using System.Collections.Generic;

namespace SportzHunter.Model
{
    public class Team : ITeam
    {
        private Guid id;
        private Guid teamleaderid;
        private string name;
        private string description;
        private Guid createdby;
        private Guid updatedby;
        private DateTime datecreated;
        private DateTime dateupdated;
        private bool isactive;
        private string teamleadername;
        public List<Player> PlayerList { get; set; }
        public Team() { }
        public Team(Guid id, Guid teamleaderid, string name, string description, Guid createdby, Guid updatedby, DateTime datecreated, DateTime dateupdated, bool isactive, string teamleadername)
        {
            this.id = id;
            this.teamleaderid = teamleaderid;
            this.name = name;
            this.description = description;
            this.createdby = createdby;
            this.updatedby = updatedby;
            this.datecreated = datecreated;
            this.dateupdated = dateupdated;
            this.isactive = isactive;
            this.teamleadername = teamleadername;
        }

        public Guid Id { get { return id; } set { id = value; } }
        public Guid TeamLeaderId { get { return teamleaderid; } set { teamleaderid = value; } }
        public string Name { get { return name; } set { name = value; } }
        public string Description { get { return description; } set { description = value; } }
        public Guid CreatedBy { get { return createdby; } set { createdby = value; } }
        public Guid UpdatedBy { get { return updatedby; } set { updatedby = value; } }
        public DateTime DateCreated { get { return datecreated; } set { datecreated = value; } }
        public DateTime DateUpdated { get { return dateupdated; } set { dateupdated = value; } }
        public bool IsActive { get { return isactive; } set { isactive = value; } }
        public string TeamLeaderName { get { return teamleadername; } set { teamleadername = value; } }

    }
}
