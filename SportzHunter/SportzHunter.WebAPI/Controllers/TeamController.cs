using AutoMapper;
using Microsoft.AspNet.Identity;
using SportzHunter.Common;
using SportzHunter.Model;
using SportzHunter.Repository;
using SportzHunter.Repository.Common;
using SportzHunter.Service.Common;
using SportzHunter.WebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace SportzHunter.WebAPI.Controllers
{
    public class TeamController : ApiController
    {

        protected ITeamService TeamService {  get; set; }
        protected IPlayerRepository PlayerRepository { get; set; }
        
        protected IMapper Mapper { get; set; }
        public TeamController(ITeamService teamService, IPlayerRepository playerRepository, IMapper mapper)
        {
            TeamService = teamService;
            PlayerRepository = playerRepository;
            Mapper = mapper;
        }


        [Authorize(Roles = "Admin, Player, Teamleader")]
        public async Task<HttpResponseMessage> GetAllTeamsAsync(string name = "", string description = "", Guid? teamleaderid = null, DateTime? datecreated = null, string sortby = "DateCreated", string sortorder = "ASC", int pagenumber = 1, int pagesize = 3)
        {
            TeamFiltering filtering = new TeamFiltering(name, description, teamleaderid, datecreated);
            Sorting sorting = new Sorting(sortby, sortorder);
            Paging paging = new Paging(pagenumber, pagesize);

            try
            {
                Guid UserId = Guid.Parse(HttpContext.Current.User.Identity.GetUserId());
                var player = await PlayerRepository.GetPlayerByUserIdAsync(UserId);
                Guid sportCategoryId = player.SportCategoryId;

                PagedList<Team> teams = await TeamService.GetAllTeamsAsync(filtering, sorting, paging);

                PagedList<TeamView> teamViews = new PagedList<TeamView>();
                teamViews.TotalCount = teams.TotalCount;
                teamViews.PageSize = teams.PageSize;
                teamViews.PageCount = teams.PageCount;


                teamViews.List = teams.List
                        .Select(x => Mapper.Map<TeamView>(x)) 
                        .ToList(); 


                return Request.CreateResponse(HttpStatusCode.OK, teamViews); 
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }



        [Authorize(Roles = "Admin, Player, Teamleader")]
        public async Task<HttpResponseMessage> GetTeamByIdAsync(Guid id)
        {
            try
            {
                Team team = await TeamService.GetTeamByIdAsync(id);

                if (team != null)
                {
                    TeamViewById teammodel = Mapper.Map<TeamViewById>(team);


                    teammodel.PlayerList = Mapper.Map<List<GetTeamByIdViewModel>>(team.PlayerList);
                    return Request.CreateResponse(HttpStatusCode.OK, teammodel);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Team not found");
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }



        [Authorize(Roles = "Admin, Teamleader")]
        [HttpPost]
        public async Task<HttpResponseMessage> CreateTeamAsync(CreateTeamViewModel viewmodelteam)
        {
            try
            {
                var existingTeam = await TeamService.GetTeamByIdAsync(viewmodelteam.Id);
                if (existingTeam != null)
                {

                    return Request.CreateResponse(HttpStatusCode.BadRequest, "Team with the same ID already exists.");
                }
                var mappedTeam = Mapper.Map<Team>(viewmodelteam);

                if (mappedTeam == null)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest);
                }

                var createdTeam = await TeamService.CreateTeamAsync(mappedTeam);
                return Request.CreateResponse(HttpStatusCode.OK, createdTeam);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }




        [Authorize(Roles = "Admin, Teamleader")]
        [HttpPut]
        public async Task<HttpResponseMessage> UpdateTeamAsync(Guid id, UpdateTeamViewModel viewmodelteam)
        {
            try
            {
                var existingTeam = await TeamService.GetTeamByIdAsync(id);
                if (existingTeam == null)
                {

                    return Request.CreateResponse(HttpStatusCode.NotFound, "Team not found.");
                }
                Mapper.Map(viewmodelteam, existingTeam);
                existingTeam.DateUpdated = DateTime.UtcNow;

              
                var updatedTeam = await TeamService.UpdateTeamAsync(id, existingTeam);
                return Request.CreateResponse(HttpStatusCode.OK, updatedTeam);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }


        [Authorize(Roles = "Admin, Teamleader")]
        [HttpDelete]
        public async Task<HttpResponseMessage> DeleteTeamAsync(Guid id)
        {
            try
            {
                var deleteclub = await TeamService.DeleteTeamAsync(id);
                return Request.CreateResponse(System.Net.HttpStatusCode.OK, deleteclub);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(System.Net.HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}
