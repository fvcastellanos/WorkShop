using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http;
using Microsoft.JSInterop;

namespace WorkShop.Services
{
    public abstract class ServiceBase
    {

        private readonly IJSRuntime _jSRuntime;

        protected const string DefaultTenant = "default";
        protected const string StrapiTokenKey = "strapiToken";

        private HttpContext _httpContext;

        public ServiceBase(IHttpContextAccessor httpContextAccessor, IJSRuntime jSRuntime)
        {
            _httpContext = httpContextAccessor.HttpContext;
            _jSRuntime = jSRuntime;
        }

        protected string GetStrapiToken()
        {
            return _httpContext.Request.Cookies[StrapiTokenKey];
        }
    }
}