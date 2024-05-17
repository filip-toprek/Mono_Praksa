using SportzHunter.Common;
using SportzHunter.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SportzHunter.Service.Common
{
    public interface ITeamLeaderService
    {
        Task<PagedList<Player>> GetPlayersAsync(PlayerFiltering filtering, Sorting sorting, Paging paging);
        Task<int> KickPlayerFromTeamAsync(Guid playerId, Guid teamId);
        Task<List<TeamLeader>> GetTeamLeaderListAsync();
        Task<TeamLeader> GetTeamLeaderAsync();
    }
}
