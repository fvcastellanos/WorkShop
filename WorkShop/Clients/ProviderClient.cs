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

        public IEnumerable<Provider> Find(string token, 
                                          int topRows, 
                                          string code, 
                                          string name)
        {
            AddAuthenticationHeader(token);

            // GET /restaurants?_where[0][stars]=1&_where[1][pricing_lte]=20

            var queryString = $"?_where[0][code_contains]={code}&_where[1][name_contains]={name}&_limit={topRows}";

            using (var response = HttpClient.GetAsync(ProvidersResource + queryString).Result)
            {
                if (response.IsSuccessStatusCode)
                {
                    var responsePayload = response.Content.ReadAsStringAsync()
                        .Result;

                    return JsonDeserialize<List<Provider>>(responsePayload);
                }
            }

            throw new HttpRequestException("Can't get provider list");
        }

    }
}