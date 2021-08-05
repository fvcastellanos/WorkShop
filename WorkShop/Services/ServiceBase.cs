using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http;
using Microsoft.JSInterop;
using WorkShop.Providers;

namespace WorkShop.Services
{
    public abstract class ServiceBase
    {
        public const string DefaultTenant = "defaultTenant";
    }
}