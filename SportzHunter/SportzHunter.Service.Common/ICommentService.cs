using SportzHunter.Common;
using SportzHunter.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SportzHunter.Service.Common
{
    public interface ICommentService
    {
        Task<PagedList<Comment>> GetAllCommentsAsync(Guid tournamentId, Paging paging);
        Task<Comment> GetCommentAsync(Guid commentId);
        Task<bool> AddAsync(Comment comment);
        Task<bool> UpdateAsync(Guid commentId, Comment comment);
        Task<bool> DeleteAsync(Guid id);
    }
}
