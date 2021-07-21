
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using LanguageExt;

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

        protected IEnumerable<T> Find<T>(string token, string url, string errorMessage)
        {
            AddAuthenticationHeader(token);

            using (var response = HttpClient.GetAsync(url).Result)
            {
                if (response.IsSuccessStatusCode)
                {
                    var responsePayload = response.Content.ReadAsStringAsync()
                        .Result;

                    return JsonDeserialize<List<T>>(responsePayload);
                }
            }

            throw new HttpRequestException(errorMessage);            
        }

        protected Option<T> FindById<T>(string token, string url, string errorMessage)
        {
            AddAuthenticationHeader(token);

            using (var response = HttpClient.GetAsync(url).Result)
            {
                if (response.IsSuccessStatusCode)
                {
                    var responsePayload = response.Content.ReadAsStringAsync()
                        .Result;

                    return JsonDeserialize<T>(responsePayload);
                }

                if (response.StatusCode.Equals(404))
                {
                    return Option<T>.None;
                }
            }

            throw new HttpRequestException(errorMessage);
        }

        protected void Add(string token, string url, StringContent content, string errorMessage)
        {
            AddAuthenticationHeader(token);

            using (var response = HttpClient.PostAsync(url, content).Result)
            {
                if (!response.IsSuccessStatusCode)
                {
                    throw new HttpRequestException(errorMessage);
                }
            }
        }

        protected void Update(string token, string url, StringContent content, string errorMessage)
        {
            AddAuthenticationHeader(token);

            using (var response = HttpClient.PutAsync(url, content).Result)
            {
                if (!response.IsSuccessStatusCode)
                {
                    throw new HttpRequestException(errorMessage);
                }
            }
        }
    }
}