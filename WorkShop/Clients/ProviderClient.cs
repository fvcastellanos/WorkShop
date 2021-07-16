using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using WorkShop.Clients.Domain;

namespace WorkShop.Clients
{
    public class ProviderClient : BaseHttpClient
    {
        private const string ProvidersResource = "/providers";

        public ProviderClient(IHttpClientFactory httpClientFactory) : base(httpClientFactory)
        {
        }

        public IEnumerable<Provider> Find(string token)
        {
            HttpClient.DefaultRequestHeaders.Add("Authorization", token);

            using (var response = HttpClient.GetAsync(ProvidersResource).Result)
            {
                if (response.IsSuccessStatusCode)
                {
                    var responsePayload = response.Content.ReadAsStringAsync()
                        .Result;

                    return JsonDeserialize<List<Provider>>(responsePayload);
                }
            }

            return null;
        }

    }
}