using SportzHunter.Common;
using SportzHunter.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SportzHunter.Service.Common
{
    public interface IAdminService
    {
        Task<List<TeamLeader>> GetPendingApplicationsAsync();
        Task<int> ApproveApplicationAsync(Guid id);
        Task<int> DisapproveApplicationAsync(Guid id);
        Task<int> DeleteUserByIdAsync(Guid id);
        Task<PagedList<User>> GetUsersAsync(Paging paging, Sorting sorting);
        Task<int> ApproveAttendAsync(Guid id);
        Task<int> DisapproveAttendAsync(Guid id);

    }
}
