using Microsoft.Owin;
using Owin;
using System;
using System.Threading.Tasks;
using Microsoft.Owin.Security.OAuth;
using System.Web.Http;
using SportzHunter.WebAPI.Models;

[assembly: OwinStartup(typeof(SportzHunter.WebAPI.App_Start.OwinStartup))]

namespace SportzHunter.WebAPI.App_Start
{
    public class OwinStartup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);

            OAuthAuthorizationServerOptions options = new OAuthAuthorizationServerOptions
            {
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/Api/User/login"),
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(1),
                Provider = new AuthorizationServerProvider()
            };
            app.UseOAuthAuthorizationServer(options);
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());

            HttpConfiguration config = new HttpConfiguration();
            WebApiConfig.Register(config);
        }
    }   
}
