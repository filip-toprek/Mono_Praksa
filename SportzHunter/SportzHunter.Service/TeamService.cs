using SportzHunter.Common;
using SportzHunter.Model;
using SportzHunter.Repository.Common;
using SportzHunter.Service.Common;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity;
namespace SportzHunter.Service
{
    public class TeamService : ITeamService
    {
        ITeamRepository TeamRepository { get; set; }
        public TeamService(ITeamRepository teamRepository)
        {
            TeamRepository = teamRepository;
            
        }

        public async Task<PagedList<Team>> GetAllTeamsAsync(TeamFiltering filtering, Sorting sorting, Paging paging)
        {
            try
            {
                return await TeamRepository.GetAllTeamsAsync(filtering, sorting, paging);
            }
            catch
            {
                return null;
            }
        }


        public async Task<Team> GetTeamByIdAsync(Guid id)
        {
            try
            {
                return await TeamRepository.GetTeamByIdAsync(id);
            }
            catch
            {
                return null;
            }
        }
        public async Task<bool> CreateTeamAsync(Team team)
        {
            try
            {
                
                Guid UserId = Guid.Parse(HttpContext.Current.User.Identity.GetUserId());
                team.Id = Guid.NewGuid();
                team.DateCreated = team.DateUpdated = DateTime.UtcNow;
                team.UpdatedBy = team.CreatedBy = UserId;
                return await TeamRepository.CreateTeamAsync(team,UserId);
            }
            catch
            {
                return false;
            }
        }
        public async Task<bool> UpdateTeamAsync(Guid id, Team team)
        {
            try
            {

                Guid UserId = Guid.Parse(HttpContext.Current.User.Identity.GetUserId());
                team.Id = id;
                team.DateUpdated = DateTime.UtcNow;
                team.UpdatedBy = UserId;
                return await TeamRepository.UpdateTeamAsync(id, team, UserId);
            }
            catch
            {
                return false;
            }
        }
        public async Task<bool> DeleteTeamAsync(Guid id)
        {
            try
            {
                return await TeamRepository.DeleteTeamAsync(id);
            }
            catch
            {
                return false;
            }
        }

        public async Task<Guid> GetTeamIdByUserIdAsync()
        {
            Guid userId = Guid.Parse(HttpContext.Current.User.Identity.GetUserId());
            return await TeamRepository.GetTeamIdByUserIdAsync(userId);
        }
    }
}
