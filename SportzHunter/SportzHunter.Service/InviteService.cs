using Microsoft.AspNet.Identity;
using SportzHunter.Model;
using SportzHunter.Repository;
using SportzHunter.Repository.Common;
using SportzHunter.Service.Common;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;

namespace SportzHunter.Service
{
    public class InviteService : IInviteService
    {
        private readonly IInviteRepository inviteRepository;
        private readonly IPlayerRepository playerRepository;
        private readonly ITeamLeaderRepository teamleaderRepository;

        public InviteService(IInviteRepository inviteRepository, IPlayerRepository playerRepository, ITeamLeaderRepository teamleaderRepository)
        {
            this.inviteRepository = inviteRepository;
            this.playerRepository = playerRepository;
            this.teamleaderRepository = teamleaderRepository;
        }

        public async Task<List<Invite>> GetAllInvitesByIdAsync()
        {
            Guid userId = Guid.Parse(HttpContext.Current.User.Identity.GetUserId());
            Player player = await playerRepository.GetPlayerByUserIdAsync(userId);
            TeamLeader teamleader = await teamleaderRepository.GetTeamLeaderByUserId(userId);
            return await inviteRepository.GetAllInvitesByIdAsync(teamleader == null ? Guid.Empty : teamleader.Team.Id, userId, player.Id);
        }

        public async Task<bool> SendInviteAsync(Guid receivingUserId, Guid teamId)
        {
            Invite invite = new Invite();
            CreateValuesDateTime(invite);

            Player player = await playerRepository.GetPlayerByUserIdAsync(receivingUserId);
            if(player == null)
                player = await playerRepository.GetPlayerByIdAsync(receivingUserId);
            Guid receivingPlayerId = player.Id;

            return await inviteRepository.SendInviteAsync(invite, receivingPlayerId, teamId);
        }

        public async Task<int> AcceptOrDeclineInviteAsync(PutInviteModel invite)
        {
            UpdateValuesDateTime(invite);

            return await inviteRepository.AcceptOrDeclineInviteAsync(invite);
        }

        public async Task<bool> DeleteInvitationAsync(Guid invitationId)
        {
            DeleteInviteModel invite = new DeleteInviteModel();
            invite.Id = invitationId;
            UpdateValuesDateTime(invite);

            return await inviteRepository.DeleteInviteAsync(invite);
        }

        public async Task<bool> CheckIfInviteWasSentAsync(Guid teamId)
        {
            Guid userId = Guid.Parse(HttpContext.Current.User.Identity.GetUserId());
            Player player = await playerRepository.GetPlayerByUserIdAsync(userId);
            Guid receivingPlayerId = player.Id;

            return await inviteRepository.CheckIfInviteWasSentAsync(receivingPlayerId, teamId);
        }

        #region SetDateTimeValuesForProperties

        private Guid GetLoggedInPlayerId()
        {
            return Guid.Parse(HttpContext.Current.User.Identity.GetUserId());
        }

        private void UpdateValuesDateTime(DeleteInviteModel invite)
        {
            invite.DateUpdated = DateTime.UtcNow;
            invite.UpdatedBy = GetLoggedInPlayerId();
        }

        private void UpdateValuesDateTime(PutInviteModel invite)
        {
            invite.DateUpdated = DateTime.UtcNow;
            invite.UpdatedBy = GetLoggedInPlayerId();
        }

        private void CreateValuesDateTime(Invite invite)
        {
            Guid loggedInPlayer = Guid.Parse(HttpContext.Current.User.Identity.GetUserId());

            invite.PlayerId = loggedInPlayer;
            invite.DateCreated = DateTime.UtcNow;
            invite.CreatedBy = loggedInPlayer;
            invite.DateUpdated = DateTime.UtcNow;
            invite.UpdatedBy = loggedInPlayer;
        }

        #endregion
    }
}
