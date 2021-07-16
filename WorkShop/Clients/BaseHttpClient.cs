
using System.Net.Http;
using System.Text.Json;

namespace WorkShop.Clients
{
    public abstract class BaseHttpClient
    {
        protected readonly HttpClient HttpClient;
        protected readonly JsonSerializerOptions JsonSerializerOptions;

        public BaseHttpClient(IHttpClientFactory httpClientFactory)
        {
            HttpClient = httpClientFactory.CreateClient("strapi");

            JsonSerializerOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
        }

        protected T JsonDeserialize<T> (string jsonPayload)
        {
            return JsonSerializer.Deserialize<T>(jsonPayload, JsonSerializerOptions);
        }
    }
}