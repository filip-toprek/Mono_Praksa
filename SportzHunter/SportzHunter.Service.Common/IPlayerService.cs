using SportzHunter.Model;
using System;
using System.Threading.Tasks;
namespace SportzHunter.Service.Common
{
    public interface IPlayerService
    {
        Task<int> PostCreateTeamleaderAsync(TeamLeader newTeamLeader, Team newTeam);
        Task<int> UpdatePlayerProfileAsync(Guid id, Player player);
        Task<int> LeaveTeamAsync();
        Task<Player> GetPlayerByIdAsync(Guid id);

        Task<Player> GetUserInfoAsync(Guid id);
    }
}
