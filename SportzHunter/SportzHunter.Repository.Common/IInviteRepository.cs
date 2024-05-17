using SportzHunter.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SportzHunter.Repository.Common
{
    public interface IInviteRepository
    {
        Task<List<Invite>> GetAllInvitesByIdAsync(Guid teamId, Guid userId, Guid playerId);
        Task<bool> SendInviteAsync(Invite invite, Guid receivingPlayerId, Guid teamId);
        Task<int> AcceptOrDeclineInviteAsync(PutInviteModel invite);
        Task<bool> DeleteInviteAsync(DeleteInviteModel invite);
        Task<bool> CheckIfInviteWasSentAsync(Guid playerId, Guid teamId);
    }
}
