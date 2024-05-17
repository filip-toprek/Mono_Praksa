using AutoMapper;
using SportzHunter.Model;
using SportzHunter.Service;
using SportzHunter.Service.Common;
using SportzHunter.WebAPI.Models;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using HttpPutAttribute = System.Web.Http.HttpPutAttribute;

namespace SportzHunter.WebAPI.Controllers
{
    public class PlayerController : ApiController
    {
        protected IPlayerService PlayerService {  get; set; }
        protected IMapper Mapper {  get; set; }

        public PlayerController(IPlayerService playerService, IMapper mapper)
        {
           PlayerService = playerService;
           Mapper = mapper;
        }

        [Authorize(Roles = "Admin, Player, Teamleader")]
        [HttpGet]
        public async Task<HttpResponseMessage> GetPlayerByIdAsync(Guid id)
        {
            Player player = await PlayerService.GetPlayerByIdAsync(id);
            if(player == null)
                return Request.CreateResponse(HttpStatusCode.NotFound, "Player not found!");
            return Request.CreateResponse(HttpStatusCode.OK, player);
        }

        [Authorize(Roles = "Admin, Player, Teamleader")]
        [HttpPut]
        public async Task<HttpResponseMessage> UpdatePlayerProfileAsync(Guid id, [FromBody] PutPlayerDetails player)
        {
            try
            {
                if (player == null)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "Player object is null.");
                }
                
                Player playerToUpdate = Mapper.Map<Player>(player);
                playerToUpdate.User = new User(id, player.FirstName, player.LastName);

                int rowsAffected = await PlayerService.UpdatePlayerProfileAsync(id, playerToUpdate);
                if (rowsAffected > 0)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, "Player profile successfully updated.");
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Player not found or profile not updated.");
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, $"Internal server error: {ex.Message}");
            }
        }


        // POST api/Player/
        [Authorize(Roles = "Admin, Player")]
        [HttpPost]
        public async Task<HttpResponseMessage> PostCreateTeamleaderAsync([FromBody] RegisterTeamleaderModel registerTeamleaderModel)
        {
            if(registerTeamleaderModel == null)
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Please provide information!");

            TeamLeader newTeamLeader = Mapper.Map<TeamLeader>(registerTeamleaderModel);

            Team newTeam = Mapper.Map<Team>(registerTeamleaderModel);

            switch(await PlayerService.PostCreateTeamleaderAsync(newTeamLeader, newTeam))
            {
                case 0:
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, "Failed to sent teamleader application!");
                case 1:
                    return Request.CreateResponse(HttpStatusCode.OK, "Your application has been sent!");
                default:
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, "Server error!");
            }
        }

        [Authorize(Roles = "Admin, Player")]
        [HttpPut]
        public async Task<HttpResponseMessage> LeaveTeamAsync()
        {
            switch(await PlayerService.LeaveTeamAsync())
            {
                case 1:
                    return Request.CreateResponse(HttpStatusCode.OK, "Team left successfully!");
                default:
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, "Unable to do that right now!");
            }
        }

        [Authorize(Roles = "Admin, Player, Teamleader")]
        [HttpGet]
        [Route("Api/Player/profile/{id}")]
        public async Task<HttpResponseMessage> GetUserInfo(Guid id)
        {
            try
            {
                Player userInfo = await PlayerService.GetUserInfoAsync(id);
                if (userInfo == null)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "Unable to get user info");
                }

                return Request.CreateResponse(HttpStatusCode.OK, userInfo);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, $"Internal server error: {ex.Message}");
            }
        }
    }
}
