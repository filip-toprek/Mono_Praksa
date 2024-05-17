using SportzHunter.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SportzHunter.Repository.Common
{
    public interface ITeamLeaderRepository
    {
        Task<int> KickPlayerFromTeamAsync(Guid playerId, Guid kickId);
        Task<TeamLeader> GetTeamLeaderByUserId(Guid id);
        Task<List<TeamLeader>> GetTeamLeaderListAsync();
    }
}
