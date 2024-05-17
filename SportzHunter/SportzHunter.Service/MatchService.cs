using Microsoft.AspNet.Identity;
using SportzHunter.Common;
using SportzHunter.Model;
using SportzHunter.Repository.Common;
using SportzHunter.Service.Common;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;

namespace SportzHunter.Service
{
    public class MatchService : IMatchService
    {
        IMatchRepository MatchRepository { get; set; }
        public MatchService(IMatchRepository matchRepository)
        {
            MatchRepository = matchRepository;
        }

        public async Task<PagedList<Match>> GetAllMatchesAsync(Paging paging)
        {
            try
            {
                return await MatchRepository.GetAllMatchesAsync(paging);
            }
            catch
            {
                return null;
            }
        }

        public async Task<Match> GetMatchByIdAsync(Guid id)
        {
            try
            {
                return await MatchRepository.GetMatchByIdAsync(id);
            }
            catch
            {
                return null;
            }
        }

        public async Task<bool> CreateMatchAsync(Match match)
        {
            try
            {
                match.Id = Guid.NewGuid();
                match.DateCreated = DateTime.UtcNow;
                return await MatchRepository.CreateMatchAsync(match);
            }
            catch
            {
                return false;
            }
        }
        public async Task<bool> UpdateMatchAsync(Guid id, Match match)
        {
            try
            {
                match.DateUpdated = DateTime.UtcNow;
                return await MatchRepository.UpdateMatchAsync(id, match);
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteMatchAsync(Guid id)
        {
            try
            {
                return await MatchRepository.DeleteMatchAsync(id);
            }
            catch
            {
                return false;
            }
        }
        public async Task<PagedList<Match>> GetAllMatchesByTournamentId(Guid tournamentId, Paging paging)
        {
            try
            {
                return await MatchRepository.GetAllMatchesByTournamentId(tournamentId, paging);
            }
            catch { return null; }
        }
    }
}
