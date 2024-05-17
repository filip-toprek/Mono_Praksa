using SportzHunter.Model.Common;
using System;

namespace SportzHunter.Model
{
    public class Match : IMatch
    {
        Guid id;
        Guid tournamentid;
        Guid teamhomeid;
        Guid teamawayid;
        string result;
        DateTime datecreated;
        DateTime dateupdated;
        bool isactive;
        string tournamentname;
        string teamhomename;
        string teamawayname;
        public Match() { }
        public Match(Guid id, Guid tournamentid, Guid teamhomeid, Guid teamawayid, string result, DateTime datecreated, DateTime dateupdated, bool isactive, string tournamentname, string teamhomename, string teamawayname)
        {
            this.id = id;
            this.tournamentid = tournamentid;
            this.teamhomeid = teamhomeid;
            this.teamawayid = teamawayid;
            this.result = result;
            this.datecreated = datecreated;
            this.dateupdated = dateupdated;
            this.isactive = isactive;
            this.tournamentname = tournamentname;
            this.teamhomename = teamhomename;
            this.teamawayname = teamawayname;
        }

        public Guid Id { get { return id; } set { id = value; } }
        public Guid TournamentId { get { return tournamentid; } set { tournamentid = value; } }
        public Guid TeamHomeId { get { return teamhomeid; } set { teamhomeid = value; } }
        public Guid TeamAwayId { get { return teamawayid; } set { teamawayid = value; } }
        public string Result { get { return result; } set { result = value; } }
        public DateTime DateCreated { get { return datecreated; } set { datecreated = value; } }
        public DateTime DateUpdated { get { return dateupdated; } set { dateupdated = value; } }
        public bool IsActive { get { return isactive; } set { isactive = value; } }

        public string TournamentName { get { return tournamentname; } set { tournamentname = value; } }
        public string TeamHomeName { get { return teamhomename; } set { teamhomename = value; } }
        public string TeamAwayName { get { return teamawayname; } set { teamawayname = value; } }
    }
}
