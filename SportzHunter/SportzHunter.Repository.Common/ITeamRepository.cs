using SportzHunter.Common;
using SportzHunter.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SportzHunter.Repository.Common
{
    public interface ITeamRepository
    {
        Task<PagedList<Team>> GetAllTeamsAsync(TeamFiltering filtering, Sorting sorting, Paging Paging);
        Task<Team> GetTeamByIdAsync(Guid id);
        Task<bool> CreateTeamAsync(Team team, Guid UserId);
        Task<bool> UpdateTeamAsync(Guid id, Team team, Guid UserId);
        Task<bool> DeleteTeamAsync(Guid id);
        Task<Team> GetTeamByTeamLeaderAsync(Guid id);
        Task<Guid> GetTeamIdByUserIdAsync(Guid userId);
    }
}
