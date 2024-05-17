using AutoMapper;
using SportzHunter.Common;
using SportzHunter.Model;
using SportzHunter.Service.Common;
using SportzHunter.WebAPI.AutoMapper;
using SportzHunter.WebAPI.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace SportzHunter.WebAPI.Controllers
{
    [RoutePrefix("api/comment")]
    public class CommentController : ApiController
    {
        private ICommentService CommentService { get; set; }
        private readonly IMapper _mapper;

        public CommentController(ICommentService commentService, IMapper mapper)
        {
            CommentService = commentService;
            _mapper = mapper;
        }

        [Authorize(Roles = "Admin, Teamleader, Player")]
        [HttpGet]
        [Route("{tournamentId}")]
        public async Task<HttpResponseMessage> GetAllCommentsAsync(Guid tournamentId, int pageSize = 3, int pageNumber = 1)
        {
            try
            {
                Paging paging = new Paging(pageNumber, pageSize);

                PagedList<Comment> comments = await CommentService.GetAllCommentsAsync(tournamentId, paging);

                if (comments == null) return Request.CreateResponse(HttpStatusCode.NotFound);

                PagedList<CommentView> commentView = new PagedList<CommentView>();
                InitializePagedList(comments, commentView);

                return Request.CreateResponse(HttpStatusCode.OK, commentView);
            }
            catch { return Request.CreateResponse(HttpStatusCode.InternalServerError); }
        }

        [Authorize(Roles = "Admin, Player")]
        [HttpGet]
        [Route("GetOneComment/{commentId}")]
        public async Task<HttpResponseMessage> GetCommentAsync(Guid commentId)
        {
            try
            {
                Comment comment = await CommentService.GetCommentAsync(commentId);

                if (comment == null) return Request.CreateResponse(HttpStatusCode.NotFound);
                return Request.CreateResponse(HttpStatusCode.OK, comment);
            }
            catch { return Request.CreateResponse(HttpStatusCode.InternalServerError); }
        }

        [Authorize(Roles = "Admin, Player")]
        [HttpPost]
        [Route("")]
        public async Task<HttpResponseMessage> PostAsync([FromBody] PostCommentModelView comment)
        {
            try
            {
                bool isSuccessful = await CommentService.AddAsync(_mapper.Map<Comment>(comment));

                if (isSuccessful) return Request.CreateResponse(HttpStatusCode.OK, "Comment added successfully");
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
            catch (Exception e) { return Request.CreateResponse(HttpStatusCode.InternalServerError, e); }
        }

        

        [Authorize(Roles = "Admin, Player")]
        [HttpPut]
        [Route("{commentId}")]
        public async Task<HttpResponseMessage> PutAsync([Required] Guid commentId, [Required] [FromBody] PutCommentModelView comment)
        {
            try
            {
                bool isSuccessful = await CommentService.UpdateAsync(commentId, _mapper.Map<Comment>(comment));

                if (isSuccessful) return Request.CreateResponse(HttpStatusCode.OK, "Comment updated successfully");
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            catch { return Request.CreateResponse(HttpStatusCode.InternalServerError); }
        }

        [Authorize(Roles = "Admin, Player")]
        [HttpDelete]
        [Route("{commentId}")]
        public async Task<HttpResponseMessage> DeleteAsync([Required] Guid commentId) 
        {
            try
            {
                bool isSuccessful = await CommentService.DeleteAsync(commentId);

                if(isSuccessful) return Request.CreateResponse(HttpStatusCode.OK, "Comment deleted successfully");
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            catch { return Request.CreateResponse(HttpStatusCode.InternalServerError); }
        }

        private void InitializePagedList(PagedList<Comment> commentList, PagedList<CommentView> commentView)
        {
            commentView.TotalCount = commentList.TotalCount;
            commentView.PageCount = commentList.PageCount;
            commentView.PageSize = commentList.PageSize;
            commentView.List = commentList.List.Select(x => _mapper.Map<CommentView>(x)).ToList();
        }
    }
}
