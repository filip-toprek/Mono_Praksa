using SportzHunter.Common;
using SportzHunter.Model;
using System;
using System.Threading.Tasks;

namespace SportzHunter.Repository.Common
{
    public interface IAttendRepository
    {
        Task<int> AddAttendTournamentAsync(Attend attend);
        Task<bool> IsTeamAlreadyAttendingAsync(Guid tournamentId, Guid teamId);
        Task<PagedList<Attend>> GetAttendListAsync();
        Task<int> ApproveAttendAsync(Guid id);
        Task<int> DisapproveAttendAsync(Guid id);
    }
}
