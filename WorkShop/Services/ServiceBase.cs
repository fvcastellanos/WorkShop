using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http;
using Microsoft.JSInterop;
using WorkShop.Providers;

namespace WorkShop.Services
{
    public abstract class ServiceBase
    {
        public const string DefaultTenant = "foo";
        private readonly HttpContext _httpContext;
        private readonly TokenProvider _tokenProvider;

        public ServiceBase(IHttpContextAccessor httpContextAccessor, TokenProvider tokenProvider)
        {
            _httpContext = httpContextAccessor.HttpContext;
            _tokenProvider = tokenProvider;
        }

        protected string GetStrapiToken()
        {
            return _tokenProvider.GetToken(_httpContext.User.Identity.Name);
        }
    }
}