using SportzHunter.Common;
using SportzHunter.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SportzHunter.Repository.Common
{
    public interface ICommentRepository
    {
        Task<PagedList<Comment>> GetCommentsAsync(Guid tournamentId, Paging paging);
        Task<Comment> GetCommentByIdAsync(Guid commentId);
        Task<bool> AddCommentAsync(Comment comment);
        Task<bool> DeleteCommentAsync(Comment comment);
        Task<bool> UpdateCommentAsync(Guid commentId, Comment comment);
    }
}
