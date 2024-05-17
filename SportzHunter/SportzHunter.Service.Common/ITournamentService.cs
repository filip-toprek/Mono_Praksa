using SportzHunter.Common;
using SportzHunter.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SportzHunter.Service.Common
{
    public interface ITournamentService
    {
        Task<PagedList<Tournament>> GetTournamentListAsync(TournamentFiltering filtering, Sorting sorting, Paging paging);
        Task<int> AddNewTournamentAsync(Tournament tournament);

        Task<int> UpdateTournamentAsync(Guid id, Tournament tournament);

        Task<Tournament> GetTournamentByIdAsync(Guid id);

        Task<bool> DeleteByIdTournamentAsync(Guid id);

        Task<List<Tournament>> GetTournamentByTeamIdAsync(Guid teamId, Sorting sorting);
    }
}
