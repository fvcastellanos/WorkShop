
using System.Net.Http;

namespace WorkShop.Clients
{
    public abstract class BaseHttpClient
    {
        protected readonly HttpClient HttpClient;

        public BaseHttpClient(HttpClient httpClient)
        {
            HttpClient = httpClient;
        }
    }
}