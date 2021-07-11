using System;
using System.Security.Claims;
using System.Threading.Tasks;
using LanguageExt;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using WorkShop.Clients;
using WorkShop.Clients.Domain;
using WorkShop.Services.Model;

namespace WorkShop.Services
{
    public class LoginService
    {
        private readonly LoginClient _loginClient;
        private readonly HttpContext _httpContext;

        private readonly ILogger _logger;

        public LoginService(LoginClient loginClient, IHttpContextAccessor httpContextAccessor, ILoggerFactory loggerFactory)
        {
            _loginClient = loginClient;
            _httpContext = httpContextAccessor.HttpContext;
            _logger = loggerFactory.CreateLogger<LoginService>();
        }

        public Either<string, AuthenticationResponse> LoginUser(string user, string password)
        {
            var loginResponse = _loginClient.PerformLogin(user, password);

            if (loginResponse != null) 
            {
                try {

                    var principal = SignInUserAsync(loginResponse)
                        .Result;

                    return new AuthenticationResponse()
                    {
                        Token = loginResponse.Jwt,
                        Principal = principal
                    };                                    
                }
                catch (Exception ex) 
                {
                    _logger.LogError("Can't authenticate - {0}", ex.Message);
                    return ex.Message;
                }
            }

            return $"Unable to authenticate: {user}";
        }

        private async Task<ClaimsPrincipal> SignInUserAsync(LoginResponse loginResponse)
        {
            var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
            identity.AddClaim(new Claim(ClaimTypes.Email, loginResponse.User.Email));
            identity.AddClaim(new Claim(ClaimTypes.Name, loginResponse.User.Username));

            var principal = new ClaimsPrincipal(identity);

            await _httpContext.SignInAsync(principal);

            return principal;
        }
    }
}