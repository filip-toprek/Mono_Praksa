using SportzHunter.Model;
using SportzHunter.Service.Common;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using SportzHunter.Common;

namespace SportzHunter.WebAPI.Controllers
{
    public class AdminController : ApiController
    {
        protected IAdminService AdminService { get; set; }

        public AdminController(IAdminService adminService)
        {
            AdminService = adminService;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        // Get api/Admin/
        public async Task<HttpResponseMessage> GetPendingApplicationsAsync()
        {
            List<TeamLeader> pendingApplications = await AdminService.GetPendingApplicationsAsync();

            if(pendingApplications == null)
                return Request.CreateResponse(HttpStatusCode.NotFound, "No applications found!");

            return Request.CreateResponse(HttpStatusCode.OK, pendingApplications);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        [Route("Api/admin/users")]
        // Get api/Admin/
        public async Task<HttpResponseMessage> GetUsersAsync(string sortBy = "RoleName", string sortOrder = "ASC", int pageNumber = 1, int pageSize = 3)
        {
            Paging paging = new Paging(pageNumber, pageSize);
            Sorting sorting = new Sorting(sortBy, sortOrder);

            PagedList<User> userList = await AdminService.GetUsersAsync(paging, sorting);

            if (userList == null)
                return Request.CreateResponse(HttpStatusCode.NotFound, "No users found!");

            return Request.CreateResponse(HttpStatusCode.OK, userList);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut]
        public async Task<HttpResponseMessage> PutApproveApplicationAsync(Guid id)
        {
            switch(await AdminService.ApproveApplicationAsync(id))
            {
                case 0:
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "Application was not approved!");
                case 1:
                    return Request.CreateResponse(HttpStatusCode.OK, "Application approved!");
                default:
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, "No applications found!");
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete]
        public async Task<HttpResponseMessage> DeleteApplicationAsync(Guid id)
        {
            switch (await AdminService.DisapproveApplicationAsync(id))
            {
                case 0:
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "Application was not disapproved!");
                case 1:
                    return Request.CreateResponse(HttpStatusCode.OK, "Application disapproved!");
                default:
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, "No applications found!");
            }
        }
        [Authorize(Roles = "Admin")]
        [Route("Api/Admin/user/{id}")]
        [HttpDelete]
        public async Task<HttpResponseMessage> DeleteUserByIdAsync(Guid id)
        {
            switch (await AdminService.DeleteUserByIdAsync(id))
            {
                case 0:
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "User was not deleted!");
                case 1:
                    return Request.CreateResponse(HttpStatusCode.OK, "User deleted!");
                default:
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, "Error deleteing a user!");
            }
        }

        [Authorize(Roles = "Admin")]
        [Route("Api/Admin/attend/{id}")]
        [HttpPut]
        public async Task<HttpResponseMessage> ApproveAttendAsync(Guid id)
        {
            switch (await AdminService.ApproveAttendAsync(id))
            {
                case 0:
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "Error!");
                case 1:
                    return Request.CreateResponse(HttpStatusCode.OK, "Approved!");
                default:
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, "Error!");
            }
        }

        [Authorize(Roles = "Admin")]
        [Route("Api/Admin/attend/{id}")]
        [HttpDelete]
        public async Task<HttpResponseMessage> DisapproveAttendAsync(Guid id)
        {
            switch (await AdminService.DisapproveAttendAsync(id))
            {
                case 0:
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "Error!");
                case 1:
                    return Request.CreateResponse(HttpStatusCode.OK, "Disapproved!");
                default:
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, "Error!");
            }
        }
    }
}