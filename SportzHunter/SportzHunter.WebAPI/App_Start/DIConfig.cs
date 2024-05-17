using SportzHunter.Service.Common;
using SportzHunter.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Http;
using SportzHunter.Repository;
using SportzHunter.Repository.Common;
using Autofac.Integration.WebApi;
using Autofac;
using SportzHunter.WebAPI.Controllers;
using Microsoft.Owin;
using AutoMapper;
using AutoMapper.Contrib.Autofac.DependencyInjection;
namespace SportzHunter.WebAPI.App_Start
{
    public class DIConfig
    {
        public static void Register(HttpConfiguration config)
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<TeamService>().As<ITeamService>();
            builder.RegisterType<TeamRepository>().As<ITeamRepository>();
            builder.RegisterType<MatchService>().As<IMatchService>();
            builder.RegisterType<MatchRepository>().As<IMatchRepository>();
            builder.RegisterType<UserService>().As<IUserService>();
            builder.RegisterType<UserRepository>().As<IUserRepository>();
            builder.RegisterType<TournamentService>().As<ITournamentService>();
            builder.RegisterType<TournamentRepository>().As<ITournamentRepository>();
            builder.RegisterType<PlayerService>().As<IPlayerService>();
            builder.RegisterType<PlayerRepository>().As<IPlayerRepository>();
            builder.RegisterType<CommentService>().As<ICommentService>();
            builder.RegisterType<CommentRepository>().As<ICommentRepository>();
            builder.RegisterType<InviteService>().As<IInviteService>();
            builder.RegisterType<InviteRepository>().As<IInviteRepository>();
            builder.RegisterType<AdminService>().As<IAdminService>();
            builder.RegisterType<AdminRepository>().As<IAdminRepository>();
            builder.RegisterType<TeamLeaderService>().As<ITeamLeaderService>();
            builder.RegisterType<TeamLeaderRepository>().As<ITeamLeaderRepository>();
            builder.RegisterType<AttendService>().As<IAttendService>();
            builder.RegisterType<AttendRepository>().As<IAttendRepository>();

            builder.AddAutoMapper(Assembly.GetExecutingAssembly());
            
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            var container = builder.Build();

            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);
        }
    }
}