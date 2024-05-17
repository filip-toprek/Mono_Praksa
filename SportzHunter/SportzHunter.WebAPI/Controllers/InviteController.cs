using AutoMapper;
using SportzHunter.Model;
using SportzHunter.Service.Common;
using SportzHunter.WebAPI.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace SportzHunter.WebAPI.Controllers
{

    [RoutePrefix("api/invite")]
    public class InviteController : ApiController
    {
        public IInviteService InviteService { get; set; }
        private readonly IMapper _mapper;

        public InviteController(IInviteService inviteService, IMapper mapper)
        {
            InviteService = inviteService;
            _mapper = mapper;
        }

        /// <summary>
        /// gets all the invites the player has, not the ones he has sent out to different teams
        /// </summary>
        /// <param name="playerId"></param>
        /// <returns></returns>
        [Authorize(Roles = "Teamleader, Player")]
        [HttpGet]
        [Route("{playerId}")]
        public async Task<HttpResponseMessage> GetAllInvitesAsync()
        {
            try
            {
                    List<Invite> invites = await InviteService.GetAllInvitesByIdAsync();

                if(invites.Count != 0) return Request.CreateResponse(HttpStatusCode.OK, invites); // View
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }
             catch (Exception e) { return Request.CreateResponse(HttpStatusCode.InternalServerError, e); }
        }

        /// <summary>
        /// If the teamleader is sending an invite, than the player's id is required for the PlayerId property
        /// if the player is sending an invite, then his Id is required
        /// receiving player is the player to which the invitation is going be linked with so we can filter invitations that are for the selected player
        /// </summary>
        /// /// <param name="inviteInfo"></param>
        /// <returns></returns>
        [Authorize(Roles = "Teamleader, Player")]
        [HttpPost]
        [Route("")]
        public async Task<HttpResponseMessage> SendInviteAsync([FromBody] PostInviteModel inviteInfo)
        {
            try
            {
                if(inviteInfo.UserId == Guid.Empty || inviteInfo.TeamId == Guid.Empty) return Request.CreateResponse(HttpStatusCode.BadRequest);

                bool isSuccessful = await InviteService.SendInviteAsync(inviteInfo.UserId, inviteInfo.TeamId);
                if(isSuccessful) return Request.CreateResponse(HttpStatusCode.OK, "Invitation sent successfully");
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
            catch { return Request.CreateResponse(HttpStatusCode.InternalServerError); }
        }

        /// <summary>
        /// Checks if the request is for accepting or declining an invitation
        /// "IsActive" value is updated and set do false after the request is sent
        /// </summary>
        /// /// <param name="invite"></param>
        /// <returns></returns>
        [Authorize(Roles = "Teamleader, Player")]
        [HttpPut]
        [Route("")]
        public async Task<HttpResponseMessage> AcceptDeclineInvite([FromBody] PutInviteModel invite)
        {
            try
            {
                switch(await InviteService.AcceptOrDeclineInviteAsync(invite))
                {
                    case -1:
                        return Request.CreateResponse(HttpStatusCode.BadRequest, "Player is already in a team");
                    case 0: 
                        return Request.CreateResponse(HttpStatusCode.BadRequest);
                    case 1: 
                        return Request.CreateResponse(HttpStatusCode.OK, "Response successfully sent");
                    default:
                        return Request.CreateResponse(HttpStatusCode.InternalServerError, "Server error");
                }


            }
            catch (Exception e) { return Request.CreateResponse(HttpStatusCode.InternalServerError, e); }
        }

        // Can't delete if teamleader or player can't see the invitation they have sent
        [Authorize(Roles = "Admin, Teamleader, Player")]
        [HttpDelete]
        [Route("{invitationId}")]
        public async Task<HttpResponseMessage> DeleteInvite([Required] [FromUri] Guid invitationId)
        {
            try
            {
                if(invitationId == Guid.Empty) return Request.CreateResponse(HttpStatusCode.BadRequest);

                bool isDeleted = await InviteService.DeleteInvitationAsync(invitationId);
                if(isDeleted) return Request.CreateResponse(HttpStatusCode.OK, "Invitation deleted successfully");
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
            catch { return Request.CreateResponse(HttpStatusCode.InternalServerError); }
        }

        [HttpGet]
        [Authorize(Roles = "Admin, Teamleader, Player")]
        [Route("check/{teamId}")]
        public async Task<bool> CheckIfInviteWasSent(Guid teamId)
        {
            return await InviteService.CheckIfInviteWasSentAsync(teamId);
        }
    }
}
