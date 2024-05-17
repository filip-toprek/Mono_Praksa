using System;

namespace SportzHunter.Common
{
    public class TournamentFiltering
    {

        public enum TournamentStatus
        {
            Past,
            Upcoming,
            Active
        }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public DateTime? Date { get; set; }
        public TournamentStatus? Status { get; set; }
        public TournamentFiltering(string name, string description, string location, DateTime? date, TournamentStatus? status)
        {
            Name = name;
            Description = description;
            Location = location;
            Date = date;
            Status = status;
        }
    }
}
