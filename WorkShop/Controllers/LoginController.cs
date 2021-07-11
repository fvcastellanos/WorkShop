using Microsoft.AspNetCore.Mvc;
using WorkShop.Controllers.Model;
using WorkShop.Services;

namespace WorkShop.Controllers
{
    [ApiController]
    [Route("/auth/login")]
    public class LoginController: ControllerBase
    {

        private LoginService _loginService;

        public LoginController(LoginService loginService)
        {
            _loginService = loginService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return Ok("hola mundo");
        }

        [HttpPost]
        public IActionResult Login(LoginRequest request) {

            var result = _loginService.LoginUser(request.User, request.Password);

            IActionResult response = null;
            
            result.Match(authResponse => {
                response = Ok(authResponse.Token);
            }, error => { 
                response = Unauthorized(error);
            });

            return response;
        }
    }
}
