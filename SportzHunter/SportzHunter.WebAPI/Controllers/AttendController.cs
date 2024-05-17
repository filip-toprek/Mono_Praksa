using SportzHunter.Model;
using SportzHunter.Service.Common;
using SportzHunter.WebAPI.Models;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace SportzHunter.WebAPI.Controllers
{
    public class AttendController : ApiController
    {
        protected IAttendService _attendService;
        protected ITeamService _teamService;
        public AttendController(IAttendService attendService, ITeamService teamService)
        {
            _attendService = attendService;
            _teamService = teamService;
        }

        [HttpPost]
        [Authorize(Roles = "Admin, Teamleader")]
        public async Task<HttpResponseMessage> AddAttendTournamentAsync(PostAttendTournament attend)
        {
            attend.TeamId = await _teamService.GetTeamIdByUserIdAsync();
            if (attend == null)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            try
            {
                Attend attendTournament = new Attend(Guid.Empty, attend.TournamentId, attend.TeamId);
                int rowsAffected = await _attendService.PostAttendTournamentAsync(attendTournament);
                if (rowsAffected != 0)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, "Successfully attended.");
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, "Failed to attend.");
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, $"Internal server error: {ex.Message}");
            }
        }

        [Authorize(Roles = "Admin")]
        public async Task<HttpResponseMessage> GetAttendListAsync()
        {
            return Request.CreateResponse(HttpStatusCode.OK, await _attendService.GetAttendListAsync());
        }
    }
}