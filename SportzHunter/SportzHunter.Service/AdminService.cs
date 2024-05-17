using SportzHunter.Model;
using SportzHunter.Repository.Common;
using SportzHunter.Service.Common;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity;
using SportzHunter.Common;

namespace SportzHunter.Service
{
    public class AdminService : IAdminService
    {
        protected IAdminRepository AdminRepository { get; set; }
        protected ITeamLeaderRepository TeamLeaderRepository { get; set; }
        protected IUserRepository UserRepository { get; set; }
        protected IAttendRepository AttendRepository { get; set; }

        public AdminService(IAdminRepository adminRepository, ITeamLeaderRepository teamleaderRepository, IUserRepository userRepository, IAttendRepository attendRepository)
        {
            AdminRepository = adminRepository;
            TeamLeaderRepository = teamleaderRepository;
            UserRepository = userRepository;
            AttendRepository = attendRepository;
        }
        public async Task<List<TeamLeader>> GetPendingApplicationsAsync()
        {
            return await AdminRepository.GetPendingApplicationsAsync();
        }
        public async Task<int> ApproveApplicationAsync(Guid id)
        {
            Guid adminId = Guid.Parse(HttpContext.Current.User.Identity.GetUserId());
            return await AdminRepository.ApproveApplicationAsync(id, adminId);
        }

        public async Task<int> DisapproveApplicationAsync(Guid id)
        {
            return await AdminRepository.DisapproveApplicationAsync(id);
        }        
        
        public async Task<int> DeleteUserByIdAsync(Guid id)
        {
            Guid adminId = Guid.Parse(HttpContext.Current.User.Identity.GetUserId());
            bool isPlayer = (await TeamLeaderRepository.GetTeamLeaderByUserId(id)) == null;
            return await AdminRepository.DeleteUserByIdAsync(id, adminId, isPlayer);
        }
        public async Task<PagedList<User>> GetUsersAsync(Paging paging, Sorting sorting)
        {
            return await UserRepository.GetUsersAsync(paging, sorting);
        }

        public async Task<int> ApproveAttendAsync(Guid id)
        {
            return await AttendRepository.ApproveAttendAsync(id);
        }

        public async Task<int> DisapproveAttendAsync(Guid id)
        {
            return await AttendRepository.DisapproveAttendAsync(id);
        }
    }
}
