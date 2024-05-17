using SportzHunter.Common;
using SportzHunter.Model;
using System;
using System.Threading.Tasks;

namespace SportzHunter.Service.Common
{
    public interface IAttendService
    {
        Task<int> PostAttendTournamentAsync(Attend attend);
        Task<bool> IsTeamAlreadyAttendingAsync(Guid tournamentId, Guid teamId);
        Task<PagedList<Attend>> GetAttendListAsync();
    }
}
