using Microsoft.AspNet.Identity;
using SportzHunter.Common;
using SportzHunter.Model;
using SportzHunter.Repository.Common;
using SportzHunter.Service.Common;
using System;
using System.Collections.Generic;
using System.Security.Policy;
using System.Threading.Tasks;
using System.Web;

namespace SportzHunter.Service
{
    public class TeamLeaderService : ITeamLeaderService
    {
        protected IPlayerRepository PlayerRepository { get; set; }
        protected ITeamRepository TeamRepository { get; set; }
        protected ITeamLeaderRepository TeamLeaderRepository { get; set; }
        public TeamLeaderService(IPlayerRepository playerRepository, ITeamRepository teamRepository, ITeamLeaderRepository teamLeaderRepository)
        {
            this.PlayerRepository = playerRepository;
            this.TeamRepository = teamRepository;
            TeamLeaderRepository = teamLeaderRepository;
        }
        public async Task<int> KickPlayerFromTeamAsync(Guid playerId, Guid teamId)
        {
            Player playerToKick = await PlayerRepository.GetPlayerByIdAsync(playerId);
            Team teamToKickFrom = await TeamRepository.GetTeamByIdAsync(teamId);
            if (playerToKick == null || teamToKickFrom == null || playerToKick.TeamId == Guid.Empty)
            {
                return 0;
            }

            if (teamToKickFrom.TeamLeaderName != HttpContext.Current.User.Identity.Name & HttpContext.Current.User.Identity.Name != "Admin")
                return -1;

            Guid kickerGuid = Guid.Parse(HttpContext.Current.User.Identity.GetUserId());

            switch (await TeamLeaderRepository.KickPlayerFromTeamAsync(playerId, kickerGuid))
            {
                case 1:
                    return 1;
                default:
                    return 0;
            }
        }

        public async Task<PagedList<Player>> GetPlayersAsync(PlayerFiltering filtering, Sorting sorting, Paging paging)
        {
            return await PlayerRepository.GetPlayersAsync(filtering, sorting, paging);
        }

        public async Task<List<TeamLeader>> GetTeamLeaderListAsync()
        {
            return await TeamLeaderRepository.GetTeamLeaderListAsync();
        }

        public async Task<TeamLeader> GetTeamLeaderAsync()
        {
            Guid userId = Guid.Parse(HttpContext.Current.User.Identity.GetUserId());
            return await TeamLeaderRepository.GetTeamLeaderByUserId(userId);
        }
    }
}
