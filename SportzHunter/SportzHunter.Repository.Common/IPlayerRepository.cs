using SportzHunter.Common;
using SportzHunter.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SportzHunter.Repository.Common
{
    public interface IPlayerRepository
    {
        Task<int> PostCreateTeamleaderAsync(TeamLeader newTeamleader, Team newTeam);
        Task<int> UpdatePlayerProfileAsync(Guid id, Player player, Guid userId);
        Task<PagedList<Player>> GetPlayersAsync(PlayerFiltering filtering, Sorting sorting, Paging paging);
        Task<Player> GetPlayerByIdAsync(Guid id);
        Task<Player> GetPlayerByUserIdAsync(Guid id);
        Task<int> LeaveTeamAsync(Guid id);
        Task<Guid> GetSportCategoryIdAsync(Guid id);

        Task<Player> GetUserInfoAsync(Guid id);
    }
}
