using Microsoft.AspNet.Identity;
using SportzHunter.Common;
using SportzHunter.Model;
using SportzHunter.Repository.Common;
using SportzHunter.Service.Common;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;

namespace SportzHunter.Service
{
    public class CommentService : ICommentService
    {
        private readonly ICommentRepository commentRepository;
        private readonly IPlayerRepository playerRepository;

        public CommentService(ICommentRepository _commentRepository, IPlayerRepository playerRepository)
        {
            commentRepository = _commentRepository;
            this.playerRepository = playerRepository;
        }

        public async Task<PagedList<Comment>> GetAllCommentsAsync(Guid tournamentId, Paging paging)
        {
            return await commentRepository.GetCommentsAsync(tournamentId, paging);
        }

        public async Task<Comment> GetCommentAsync(Guid commentId)
        {
            return await commentRepository.GetCommentByIdAsync(commentId);
        }

        public async Task<bool> AddAsync(Comment comment)
        {
            await UpdateSetValuesDateTime(comment);

            return await commentRepository.AddCommentAsync(comment);
        }

        public async Task<bool> UpdateAsync(Guid commentId, Comment comment)
        {
            await UpdateSetValuesDateTime(comment);

            return await commentRepository.UpdateCommentAsync(commentId, comment);
        }

        public async Task<bool> DeleteAsync(Guid commentId)
        {
            Comment comment = new Comment();
            comment.Id = commentId;
            await UpdateSetValuesDateTime(comment);

            return await commentRepository.DeleteCommentAsync(comment);
        }

        #region SetDateTimeValues

        private async Task<Guid> GetPlayerId()
        {
            Player player = await playerRepository.GetPlayerByUserIdAsync(Guid.Parse(HttpContext.Current.User.Identity.GetUserId()));
            return player.Id;
        }

        private async Task UpdateSetValuesDateTime(Comment comment)
        {
            Guid playerId = await GetPlayerId();

            comment.PlayerId = playerId;
            comment.DateCreated =  DateTime.UtcNow;
            comment.DateUpdated = DateTime.UtcNow;
            comment.CreatedBy =  playerId;
            comment.UpdatedBy = playerId;
        }

        #endregion
    }
}
