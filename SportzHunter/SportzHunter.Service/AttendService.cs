    using Microsoft.AspNet.Identity;
using SportzHunter.Common;
using SportzHunter.Model;
using SportzHunter.Repository.Common;
using SportzHunter.Service.Common;
using System;
using System.Threading.Tasks;
using System.Web;

namespace SportzHunter.Service
{
    public class AttendService : IAttendService
    {
        private IAttendRepository _attendRepository;
        public AttendService(IAttendRepository attendRepository)
        {
            _attendRepository = attendRepository;
        }
        public async Task<int> PostAttendTournamentAsync(Attend attend)
        {
            bool isTeamAlreadyAttending = await IsTeamAlreadyAttendingAsync(attend.TournamentId, attend.TeamId);
            if (isTeamAlreadyAttending)
            {
                throw new InvalidOperationException("Team is already attending the tournament.");
            }
            InitializeAttend(attend);
            return await _attendRepository.AddAttendTournamentAsync(attend);
        }
        public async Task<bool> IsTeamAlreadyAttendingAsync(Guid tournamentId, Guid teamId)
        {
            return await _attendRepository.IsTeamAlreadyAttendingAsync(tournamentId, teamId);
        }

        public async Task<PagedList<Attend>> GetAttendListAsync()
        {
            return await _attendRepository.GetAttendListAsync();
        }

        private void InitializeAttend(Attend attend)
        {
            Guid id = Guid.NewGuid();
            Guid userId = Guid.Parse(HttpContext.Current.User.Identity.GetUserId());
            attend.Id = id;
            attend.CreatedBy = userId;
            attend.UpdatedBy = userId;
            attend.DateCreated = DateTime.UtcNow;
            attend.DateUpdated = DateTime.UtcNow;
        }
    }
}
