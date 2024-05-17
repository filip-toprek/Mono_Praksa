using AutoMapper;
using SportzHunter.Common;
using SportzHunter.Model;
using SportzHunter.Service.Common;
using SportzHunter.WebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;

namespace SportzHunter.WebAPI.Controllers
{
    public class TeamLeaderController : ApiController
    {
        protected ITeamLeaderService TeamLeaderService { get; set; }
        protected IMapper Mapper { get; set; }
        public TeamLeaderController(ITeamLeaderService teamLeaderService, IMapper mapper)
        {
            TeamLeaderService = teamLeaderService;
            Mapper = mapper;
        }
        [Authorize(Roles = "Admin, Teamleader")]
        [HttpPost]
        public async Task<HttpResponseMessage> PostKickPlayerFromTeamAsync(PostKickPlayerViewModel playerToKick)
        {
            if (playerToKick == null)
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Please provide a player to kick.");

            switch(await TeamLeaderService.KickPlayerFromTeamAsync(playerToKick.PlayerId, playerToKick.TeamId))
            {
                case 0:
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, "There was an error kicking the player.");
                case 1:
                    return Request.CreateResponse(HttpStatusCode.OK, "Player was kicked from the team.");
                case -1:
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "You do not have premissino to do that.");
                default:
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, "Error in the request.");
            }
        }
        [Authorize(Roles = "Admin, Teamleader")]
        // GET api/teamleader/
        public async Task<HttpResponseMessage> GetPlayerListAsync(Guid sportCategoryId, Guid? positionId = null, Guid? countyId = null, int ageCategory = 0, string sortBy = "Height", string sortOrder = "ASC", int pageNumber = 1, int pageSize = 3)
        {
            PlayerFiltering playerFilter = new PlayerFiltering(positionId, countyId, ageCategory, sportCategoryId);
            Sorting sorting = new Sorting(sortBy, sortOrder);
            Paging paging = new Paging(pageNumber, pageSize);
            PagedList<Player> playerList = await TeamLeaderService.GetPlayersAsync(playerFilter, sorting, paging);
            if(playerList == null)
                return Request.CreateResponse(HttpStatusCode.NotFound, "No players found for that sport.");
            PagedList<GetPlayerViewModel> playerViewModel = new PagedList<GetPlayerViewModel>();
            InitializePagedList(playerList, playerViewModel);
            return Request.CreateResponse(HttpStatusCode.OK, playerViewModel);
        }

        public async Task<HttpResponseMessage> GetTeamLeaderListAsync()
        {
            List<GetTeamLeader> result = new List<GetTeamLeader>();

            try
            {
                List<TeamLeader> teamLeaders = await TeamLeaderService.GetTeamLeaderListAsync();

                if (teamLeaders != null)
                {
                    result = teamLeaders.Select(tl => new GetTeamLeader
                    {
                        TeamLeaderId = tl.Id,
                        TeamLeaderName = tl.Username
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }

            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Authorize(Roles = "Admin, Teamleader")]
        [Route("Api/Teamleader/profile")]
        [HttpGet]
        public async Task<HttpResponseMessage> GetTeamLeaderAsync()
        {
            return Request.CreateResponse(HttpStatusCode.OK, await TeamLeaderService.GetTeamLeaderAsync());
        }


        private void InitializePagedList(PagedList<Player> playerList, PagedList<GetPlayerViewModel> playerViewModel)
        {
            playerViewModel.TotalCount = playerList.TotalCount;
            playerViewModel.PageCount = playerList.PageCount;
            playerViewModel.PageSize = playerList.PageSize;
            playerViewModel.List = playerList.List.Select(x => Mapper.Map<GetPlayerViewModel>(x)).ToList();
        }
    }
}
