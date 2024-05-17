using AutoMapper;
using SportzHunter.Common;
using SportzHunter.Model;
using SportzHunter.Repository;
using SportzHunter.Service.Common;
using SportzHunter.WebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace SportzHunter.WebAPI.Controllers
{
    public class MatchController : ApiController
    {
        protected IMatchService MatchService { get; set; }
        protected IMapper Mapper { get; set; }
        public MatchController(IMatchService matchService, IMapper mapper)
        {
            MatchService = matchService;
            Mapper = mapper;
        }

        [Authorize(Roles = "Admin, Teamleader, Player")]
        public async Task<HttpResponseMessage> GetAllMatchesAsync()
        {
            try
            {
                Paging paging = new Paging(pageNumber: 1, pageSize: 10);
                PagedList<Match> matches = await MatchService.GetAllMatchesAsync(paging);

                PagedList<MatchView> matchesView = new PagedList<MatchView>
                {
                    PageSize = matches.PageSize,
                    PageCount = matches.PageCount,
                    TotalCount = matches.TotalCount,
                    List = matches.List.Select(x => Mapper.Map<MatchView>(x)).ToList()
                };

                return Request.CreateResponse(HttpStatusCode.OK, matchesView);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [Authorize(Roles = "Admin, Teamleader, Player")]
        public async Task<HttpResponseMessage> GetAllMatchesByTournamentId(Guid tournamentId)
        {
            try
            {
                Paging paging = new Paging(pageNumber: 1, pageSize: 10);
                PagedList<Match> matches = await MatchService.GetAllMatchesByTournamentId(tournamentId, paging);

                PagedList<MatchDetailsViewByTournament> matchesView = new PagedList<MatchDetailsViewByTournament>
                {
                    PageSize = matches.PageSize,
                    PageCount = matches.PageCount,
                    TotalCount = matches.TotalCount,
                    List = matches.List.Select(x => Mapper.Map<MatchDetailsViewByTournament>(x)).ToList()
                };

                return Request.CreateResponse(HttpStatusCode.OK, matchesView);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }



        [Authorize(Roles = "Admin, Teamleader, Player")]
        public async Task<HttpResponseMessage> GetMatchByIdAsync(Guid id)
        {
            try
            {
                Match match = await MatchService.GetMatchByIdAsync(id);
                MatchView matchView = Mapper.Map<MatchView>(match);

                return Request.CreateResponse(System.Net.HttpStatusCode.OK, matchView);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(System.Net.HttpStatusCode.InternalServerError, ex.Message);
            }
        }


        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<HttpResponseMessage> CreateMatchAsync(CreateMatchViewModel viewmodelmatch)
        {
            try
            {

                Match match = Mapper.Map<Match>(viewmodelmatch);
                if (match == null)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest);
                }
                var createdmatch = await MatchService.CreateMatchAsync(match);
                return Request.CreateResponse(HttpStatusCode.OK, createdmatch);
            }
            catch (Exception ex)
            { 
                
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }



        [Authorize(Roles = "Admin")]
        [HttpPut]
        public async Task<HttpResponseMessage> UpdateMatchAsync(Guid id, UpdateMatchViewModel viewmodelmatch)
        {
            try
                {
    
                Match existingMatch = await MatchService.GetMatchByIdAsync(id);
                if (existingMatch == null)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound);
                }

                Mapper.Map(viewmodelmatch, existingMatch);
                var updatedmatch = await MatchService.UpdateMatchAsync(id, existingMatch);
                return Request.CreateResponse(HttpStatusCode.OK, updatedmatch);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }



        [Authorize(Roles = "Admin")]
        public async Task<HttpResponseMessage> DeleteMatchAsync(Guid id)
        {
            try
            {
                var deletedmatch = await MatchService.DeleteMatchAsync(id);
                return Request.CreateResponse(System.Net.HttpStatusCode.OK, deletedmatch);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(System.Net.HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}
