using Microsoft.Owin.Security.OAuth;
using SportzHunter.Model;
using SportzHunter.Repository;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using BC = BCrypt.Net.BCrypt;
namespace SportzHunter.WebAPI.Models
{
    public class AuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
        }
        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            using (UserRepository userRepository = new UserRepository())
            {
                User loggedUser = await userRepository.PostLoginUserAsync(new User(context.UserName, context.Password));
               
                if (loggedUser == null || !loggedUser.IsActive || !BC.EnhancedVerify(context.Password, loggedUser.PasswordHash))
                {
                    context.SetError("Invalid_Credentials", "Provided username and password is incorrect");
                    return;
                }

                System.Guid SportCategoryId;
                Player player = null;
                using (PlayerRepository playerRepository = new PlayerRepository())
                {

                    SportCategoryId = await playerRepository.GetSportCategoryIdAsync(loggedUser.Id);
                    player = await playerRepository.GetPlayerByUserIdAsync(loggedUser.Id);
                }
                var identity = new ClaimsIdentity(context.Options.AuthenticationType);
                identity.AddClaim(new Claim(ClaimTypes.Role, loggedUser.UserRole.RoleName));
                identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, loggedUser.Id.ToString()));
                identity.AddClaim(new Claim(ClaimTypes.Name, loggedUser.Username));
                identity.AddClaim(new Claim("SportId", SportCategoryId.ToString()));
                identity.AddClaim(new Claim("TeamId", player.TeamId?.ToString()));

                context.Validated(identity);
                
            }
        }

        public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        {
            context.AdditionalResponseParameters.Add("role", context.Identity.FindFirst(ClaimTypes.Role)?.Value);
            context.AdditionalResponseParameters.Add("userId", context.Identity.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            context.AdditionalResponseParameters.Add("sportId", context.Identity.FindFirst("SportId")?.Value);
            context.AdditionalResponseParameters.Add("teamId", context.Identity.FindFirst("TeamId")?.Value);

            return Task.FromResult<object>(null);
        }

    }
}