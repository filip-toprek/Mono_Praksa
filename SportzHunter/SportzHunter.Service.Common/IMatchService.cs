using SportzHunter.Common;
using SportzHunter.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SportzHunter.Service.Common
{
    public interface IMatchService
    {
        Task<PagedList<Match>> GetAllMatchesAsync(Paging paging);
        Task<Match> GetMatchByIdAsync(Guid id);
        Task<bool> CreateMatchAsync(Match match);
        Task<bool> UpdateMatchAsync(Guid id, Match match);
        Task<bool> DeleteMatchAsync(Guid id);
        Task<PagedList<Match>> GetAllMatchesByTournamentId(Guid tournamentId, Paging paging);
    }
}
