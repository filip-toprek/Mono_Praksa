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
    public class TournamentService : ITournamentService
    {
        private ITournamentRepository _tournamentRepository;

        public TournamentService(ITournamentRepository tournamentRepository)
        {
            _tournamentRepository = tournamentRepository;
        }

        public async Task<PagedList<Tournament>> GetTournamentListAsync(TournamentFiltering filtering, Sorting sorting, Paging paging)
        {
            return await _tournamentRepository.GetTournamentListAsync(filtering, sorting, paging);
        }

        public async Task<int> AddNewTournamentAsync(Tournament tournament)
        {
            InitializeTournament(tournament);
            return await _tournamentRepository.AddNewTournamentAsync(tournament);
        }
        public async Task<Tournament> GetTournamentByIdAsync(Guid id)
        {
            return await _tournamentRepository.GetTournamentByIdAsync(id);
        }
        public async Task<int> UpdateTournamentAsync(Guid id, Tournament tournament)
        {
            InitializeUpdate(tournament);
            return await _tournamentRepository.UpdateTournamentAsync(id,tournament);
        }
        public async Task<bool> DeleteByIdTournamentAsync(Guid id)
        {
            return await _tournamentRepository.DeleteByIdTournamentAsync(id);
        }

        public async Task<List<Tournament>> GetTournamentByTeamIdAsync(Guid teamId, Sorting sorting)
        {
            return await _tournamentRepository.GetTournamentByTeamAsync(teamId, sorting);
        }

        private void InitializeTournament(Tournament tournament)
        {
            Guid userId = Guid.Parse(HttpContext.Current.User.Identity.GetUserId());
            Guid id = Guid.NewGuid();
            tournament.Id = id;
            tournament.CreatedBy = userId;
            tournament.UpdatedBy = userId;
            tournament.DateCreated = DateTime.UtcNow;
            tournament.DateUpdated = DateTime.UtcNow;
        }

        private void InitializeUpdate(Tournament tournament)
        {
            Guid userId = Guid.Parse(HttpContext.Current.User.Identity.GetUserId());
            tournament.UpdatedBy = userId;
            tournament.DateUpdated = DateTime.UtcNow;
        }
    }
}
