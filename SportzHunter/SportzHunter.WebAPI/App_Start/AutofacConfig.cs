using SportzHunter.Service.Common;
using SportzHunter.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Http;
using Autofac;
using Autofac.Integration.WebApi;
using SportzHunter.Repository;
using SportzHunter.Repository.Common;

namespace SportzHunter.WebAPI.App_Start
{
    public class AutofacConfig
    {
        public static void Configure()
        {
            var builder = new ContainerBuilder();
            var config = GlobalConfiguration.Configuration;

            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            builder.RegisterType<TournamentService>().As<ITournamentService>();
            builder.RegisterType<TournamentRepository>().As<ITournamentRepository>();

            var container = builder.Build();

            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);
        }
    }
}