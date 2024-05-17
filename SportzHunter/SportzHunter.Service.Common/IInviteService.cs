using SportzHunter.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SportzHunter.Service.Common
{
    public interface IInviteService
    {
        Task<List<Invite>> GetAllInvitesByIdAsync();
        Task<bool> SendInviteAsync(Guid receivingPlayerId, Guid teamId);
        Task<int> AcceptOrDeclineInviteAsync(PutInviteModel invite);
        Task<bool> DeleteInvitationAsync(Guid invitationId);
        Task<bool> CheckIfInviteWasSentAsync(Guid teamId);
    }
}
