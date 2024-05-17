using System;
using System.Threading.Tasks;

namespace SportzHunter.Repository.Common
{
    public interface IInviteStatusRepository
    {
        Task<Guid> GetInviteStatus(string status);
    }
}
