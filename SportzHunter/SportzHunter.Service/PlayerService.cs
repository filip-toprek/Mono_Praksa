using Microsoft.AspNet.Identity;
using SportzHunter.Model;
using SportzHunter.Repository.Common;
using SportzHunter.Service.Common;
using System;
using System.Threading.Tasks;
using System.Web;

namespace SportzHunter.Service
{
    public class PlayerService : IPlayerService
    {
        protected IPlayerRepository PlayerRepository { get; set; }

        public PlayerService(IPlayerRepository playerRepository)
        {
            PlayerRepository = playerRepository;
        }

        public async Task<Player> GetPlayerByIdAsync(Guid id)
        {
            return await PlayerRepository.GetPlayerByIdAsync(id);
        }

        public async Task<int> PostCreateTeamleaderAsync(TeamLeader newTeamLeader, Team newTeam)
        {
            InitializeNewTeam(newTeamLeader, newTeam);
            return await PlayerRepository.PostCreateTeamleaderAsync(newTeamLeader, newTeam);
        }

        public async Task<int> UpdatePlayerProfileAsync(Guid id, Player player)
        {
            Guid userId = Guid.Parse(HttpContext.Current.User.Identity.GetUserId());
            try
            {
                return await PlayerRepository.UpdatePlayerProfileAsync(id, player, userId);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred while updating player profile.", ex);
            }
        }

        public async Task<int> LeaveTeamAsync()
        {
            Guid id = Guid.Parse(HttpContext.Current.User.Identity.GetUserId());
            return await PlayerRepository.LeaveTeamAsync(id);
        }

        private void InitializeNewTeam(TeamLeader newTeamLeader, Team newTeam)
        {
            Guid userId = Guid.Parse(HttpContext.Current.User.Identity.GetUserId());
            newTeamLeader.UserId = userId;
            newTeam.TeamLeaderId = newTeamLeader.Id = Guid.NewGuid();
            newTeam.Id = Guid.NewGuid();
            newTeamLeader.DateUpdated = newTeamLeader.DateCreated = DateTime.UtcNow;
            newTeamLeader.CreatedBy = newTeamLeader.UpdatedBy = userId;
            newTeam.DateUpdated = newTeam.DateCreated = DateTime.UtcNow;
            newTeam.CreatedBy = newTeam.UpdatedBy = userId;

        }

        public async Task<Player> GetUserInfoAsync(Guid id)
        {
            try
            {
                return await PlayerRepository.GetUserInfoAsync(id);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred while retrieving user info.", ex);
            }
        }

    }
}
