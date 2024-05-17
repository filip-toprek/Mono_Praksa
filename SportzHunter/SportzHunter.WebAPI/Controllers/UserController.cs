using SportzHunter.Model;
using SportzHunter.WebAPI.Models;
using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Threading.Tasks;
using SportzHunter.Service.Common;
using System.Web;
using AutoMapper;

namespace SportzHunter.WebAPI.Controllers
{
    public class UserController : ApiController
    {
        protected IUserService UserService { get; set; }
        protected IMapper Mapper { get; set; }

        public UserController(IUserService _userService, IMapper mapper)
        {
            UserService = _userService;
            Mapper = mapper;
        }

        [Route("Api/User/register")]
        // POST api/User/register
        public async Task<HttpResponseMessage> PostUserRegisterAsync([FromBody] RegisterUserModel postRegisterUser)
        {
            if (postRegisterUser == null)
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Please fill in the register form!");

            if (!postRegisterUser.Password.Equals(postRegisterUser.VerifyPassword))
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Please enter a correct password!");

            User newUser = Mapper.Map<User>(postRegisterUser);
            Player newPlayer = Mapper.Map<Player>(postRegisterUser);
            switch (await UserService.PostRegisterUserAsync(newUser, newPlayer))
            {
                case 0:
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, "Failed to create user!");
                case 1:
                    return Request.CreateResponse(HttpStatusCode.OK, "User created successfully!");
                case -1:
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "User with that username already exists!");
                default:
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, "There was an server error!");
            }
        }

        [Route("Api/User/SportCategories")]
        public async Task<HttpResponseMessage> GetSportCategoryAsync()
        {
            return Request.CreateResponse(HttpStatusCode.OK, await UserService.GetSportCategoriesAsync());
        }

        [Route("Api/User/Counties")]

        public async Task<HttpResponseMessage> GetCountiesAsync()
        {
            return Request.CreateResponse(HttpStatusCode.OK, await UserService.GetCountiesAsync());
        }

        [Route("Api/User/PrefPositions")]
        public async Task<HttpResponseMessage> GetPrefPositionsAsync()
        {
            return Request.CreateResponse(HttpStatusCode.OK, await UserService.GetPrefPositionsAsync());
        }

        public HttpResponseMessage GetUserLogout()
        {
            HttpContext.Current.GetOwinContext().Authentication.SignOut();
            return Request.CreateResponse(HttpStatusCode.OK, "User was signed out!");
        }
    }
}
