using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WorkShop.Clients;
using WorkShop.Clients.Domain;
using WorkShop.Domain.Views;
using WorkShop.Providers;

namespace WorkShop.Controllers
{
    [Route("/Login")]
    public class LoginController: Controller
    {
        private readonly LoginClient _loginClient;
        private readonly HttpContext _httpContext;
        private readonly TokenProvider _tokenProvider;
        private readonly ILogger _logger;

        public LoginController(LoginClient loginClient,
                               IHttpContextAccessor httpContextAccessor,
                               TokenProvider tokenProvider,
                               ILoggerFactory loggerFactory)
        {
            _loginClient = loginClient;
            _httpContext = httpContextAccessor.HttpContext;
            _tokenProvider = tokenProvider;
            _logger = loggerFactory.CreateLogger<LoginController>();
        }

        [HttpGet]
        public IActionResult Index()
        {
            if (Request.Query.ContainsKey("logout"))
            {
                _tokenProvider.RemoveToken(_httpContext.User.Identity.Name);
                _httpContext.SignOutAsync();
            }

            return View("Login");
        }

        [HttpPost]
        public IActionResult Login(LoginModel login)
        {
            if (ModelState.IsValid)
            {
                if (AuthenticateUser(login))
                {                    
                    return Redirect("/");
                }

                ModelState.AddModelError("LoginError", $"Error al autenticar al usuario: {login.User}");
            }

            return View("Login");
        }

        // ----------------------------------------------------------------------------------------

        private bool AuthenticateUser(LoginModel loginModel)
        {
            var result = _loginClient.PerformLogin(loginModel.User, loginModel.Password);
            var authenticated = false;

            result.Match(response => {

                var principal = BuildClaimsPrincipal(response);
                var authenticationProperties = new AuthenticationProperties()
                {
                    IsPersistent = true
                };

                _httpContext.SignInAsync(principal, authenticationProperties);
                _tokenProvider.StoreToken(loginModel.User, response.Jwt);
                authenticated = true;

                _logger.LogInformation("Success authentication for user: {0}", loginModel.User);

            }, () => _logger.LogWarning("Unable to authenticate user: {0}", loginModel.User));

            return authenticated;
        }

        private ClaimsPrincipal BuildClaimsPrincipal(LoginResponse loginResponse)
        {
            var claims = new Claim[] {
                new Claim(ClaimTypes.Email, loginResponse.User.Email),
                new Claim(ClaimTypes.Name, loginResponse.User.Username)
            };

            var identity = new ClaimsIdentity(claims, "Strapi Authentication");
            return new ClaimsPrincipal(identity);
        }
    }
}