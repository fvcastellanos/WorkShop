using System.Security.Claims;
using System.Threading.Tasks;
using LanguageExt;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using WorkShop.Clients;
using WorkShop.Clients.Domain;

namespace WorkShop.Services
{
    public class LoginService
    {
        private readonly LoginClient _loginClient;
        private readonly HttpContext _httpContext;

        public LoginService(LoginClient loginClient, HttpContext httpContext)
        {
            _loginClient = loginClient;
            _httpContext = httpContext;
        }

        public async Task<Either<string, ClaimsPrincipal>> LoginUser(string user, string password)
        {
            var loginResponses = await _loginClient.PerformLoginAsync(user, password);

            if (loginResponses != null) 
            {
                return await SignInUserAsync(loginResponses);

            }

            return $"Unable to authenticate: {user}";
        }

        private async Task<ClaimsPrincipal> SignInUserAsync(LoginResponse loginResponse)
        {
            var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
            identity.AddClaim(new Claim(ClaimTypes.Email, loginResponse.User.Email));
            identity.AddClaim(new Claim(ClaimTypes.UserData, loginResponse.User.Username));
            identity.AddClaim(new Claim(ClaimTypes.Actor, loginResponse.Jwt));

            var principal = new ClaimsPrincipal(identity);

            await _httpContext.SignInAsync(principal);

            return principal;
        }
    }
}