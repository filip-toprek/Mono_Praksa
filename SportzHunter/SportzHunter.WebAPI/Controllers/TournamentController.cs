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

namespace SportzHunter.WebAPI.Controllers
{
    public class TournamentController : ApiController
    {
        protected ITournamentService _tournamentService;
        protected IMapper _mapper;

        public TournamentController(ITournamentService tournamentService, IMapper mapper)
        {
            _tournamentService = tournamentService;
            _mapper = mapper;
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<HttpResponseMessage> AddNewTournament([FromBody] PostNewTournament tournament)
        {
            if(tournament == null)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
            if (string.IsNullOrWhiteSpace(tournament.Name) ||
                string.IsNullOrWhiteSpace(tournament.Location) ||
                tournament.Date == default ||
                tournament.NumberOfTeams == default)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "One or more required properties are missing.");
            }

            try
            {
                Tournament tournamentToAdd = _mapper.Map<Tournament>(tournament);
                int rowsAffected = await _tournamentService.AddNewTournamentAsync(tournamentToAdd);
                if (rowsAffected != 0)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, "Tournament successfully added.");
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, "Failed to add tournament.");
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, $"Internal server error: {ex.Message}");
            }
        }

        [Authorize(Roles = "Admin, Teamleader, Player")]
        [HttpGet]
        public async Task<HttpResponseMessage> GetAllTournamentsAsync(string name = null, string description = null, string location = null, DateTime? date = null, int PageNumber = 1, int PageSize = 3, string SortBy = "Date", string SortOrder = "ASC", TournamentFiltering.TournamentStatus? status = null)
        {
            try
            {
                TournamentFiltering filtering = new TournamentFiltering(name, description, location, date, status);
                Sorting sorting = new Sorting(SortBy, SortOrder);
                Paging paging = new Paging(PageNumber, PageSize);
                PagedList<Tournament> tournaments = await _tournamentService.GetTournamentListAsync(filtering, sorting, paging);
                if(tournaments == null)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, $"No tournaments available");
                }
                PagedList<GetTournaments> tournamentList = new PagedList<GetTournaments>();
                tournamentList.TotalCount = tournaments.TotalCount;
                tournamentList.PageCount = tournaments.PageCount;
                tournamentList.PageSize = tournaments.PageSize;
                tournamentList.List = tournaments.List.Select(x => _mapper.Map<GetTournaments>(x)).ToList();
                return Request.CreateResponse(HttpStatusCode.OK, tournamentList);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, $"Internal server error: {ex.Message}");
            }
        }

        [Authorize(Roles = "Admin, Teamleader, Player")]
        [HttpGet]
        public async Task<HttpResponseMessage> GetTournamentByIdAsync(Guid id)
        {
            if (id == null)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
            try
            {
                var tournament = await _tournamentService.GetTournamentByIdAsync(id);
                TournamentBasicInfo getTournament = _mapper.Map<TournamentBasicInfo>(tournament);
                if (tournament != null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, getTournament);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Tournament not found.");
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, $"Internal server error: {ex.Message}");
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPut]
        public async Task<HttpResponseMessage> UpdateTournamentAsync(Guid id, [FromBody] PutTournamentDetails tournament)
        {
            try
            {
                if (tournament == null)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest);
                }
                Tournament tournamentToUpdate = _mapper.Map<Tournament>(tournament);
                int rowsAffected = await _tournamentService.UpdateTournamentAsync(id, tournamentToUpdate);
                if (rowsAffected != 0)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, "Tournament successfully updated.");
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, "Failed to update tournament.");
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, $"Internal server error: {ex.Message}");
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete]
        public async Task<HttpResponseMessage> DeleteByIdTournamentAsync(Guid id)
        {
            if (id == null){
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
            try
            {
                bool isDeleted = await _tournamentService.DeleteByIdTournamentAsync(id);
                if (isDeleted)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, "Tournament successfully deleted.");
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Tournament not found.");
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, $"Internal server error: {ex.Message}");
            }
        }

        [Authorize(Roles = "Admin, Teamleader, Player")]
        [Route("api/Tournament/Team/{teamId}")]
        [HttpGet]
        public async Task<HttpResponseMessage> GetTournamentByTeamIdAsync(Guid teamId, string SortBy = "Date", string SortOrder = "ASC")
        {
            try
            {
                Sorting sorting = new Sorting(SortBy, SortOrder);
                List<Tournament> tournamentList = new List<Tournament>();
                tournamentList = await _tournamentService.GetTournamentByTeamIdAsync(teamId, sorting);
                List<GetTournamentByTeamId> getTournament = _mapper.Map<List<GetTournamentByTeamId>>(tournamentList);
                if (tournamentList != null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, getTournament);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Tournament not found.");
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, $"Internal server error: {ex.Message}");
            }
        }
    }
}
