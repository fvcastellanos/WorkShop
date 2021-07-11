using System.Security.Claims;

namespace WorkShop.Services.Model
{
    public class AuthenticationResponse
    {
        public string Token { get; set; }
        public ClaimsPrincipal Principal { get; set; }
    }
}