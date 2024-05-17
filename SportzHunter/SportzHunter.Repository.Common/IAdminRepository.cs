using SportzHunter.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SportzHunter.Repository.Common
{
    public interface IAdminRepository
    {
        Task<List<TeamLeader>> GetPendingApplicationsAsync();
        Task<int> ApproveApplicationAsync(Guid id, Guid adminId);
        Task<int> DisapproveApplicationAsync(Guid id);
        Task<int> DeleteUserByIdAsync(Guid id, Guid adminId, bool isPlayer);
    }
}
