using SportzHunter.Common;
using SportzHunter.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SportzHunter.Service.Common
{
    public interface ITeamService
    {
        Task<PagedList<Team>> GetAllTeamsAsync(TeamFiltering filtering, Sorting sorting, Paging paging);
        Task<Team> GetTeamByIdAsync(Guid id);
        Task<bool> CreateTeamAsync(Team team);
        Task<bool> UpdateTeamAsync(Guid id, Team team);
        Task<bool> DeleteTeamAsync(Guid id);
        Task<Guid> GetTeamIdByUserIdAsync();
    }
}
