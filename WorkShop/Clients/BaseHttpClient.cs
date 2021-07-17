
using System.Net.Http;
using System.Text;
using System.Text.Json;

namespace WorkShop.Clients
{
    public abstract class BaseHttpClient
    {
        private const string AuthorizationHeaderName = "Authorization";
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

        protected void AddAuthenticationHeader(string token) 
        {
            
            if (HttpClient.DefaultRequestHeaders.Contains(AuthorizationHeaderName))
            {
                HttpClient.DefaultRequestHeaders.Remove(AuthorizationHeaderName);
            }            

            HttpClient.DefaultRequestHeaders.Add(AuthorizationHeaderName, "Bearer " + token);
        }

        protected StringContent CreateStringContent(object obj)
        {
            var payload = JsonSerializer.Serialize(obj);
            return new StringContent(payload, Encoding.UTF8, "application/json");
        }

    }
}