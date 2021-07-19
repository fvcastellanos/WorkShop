using System.Collections.Generic;
using System.Net.Http;
using LanguageExt;
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
                                          string name,
                                          int active)
        {
            AddAuthenticationHeader(token);

            // GET /restaurants?_where[0][stars]=1&_where[1][pricing_lte]=20

            var queryString = $"?_where[0][code_contains]={code}&_where[1][name_contains]={name}&_where[2][active]={active}&_limit={topRows}";

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

        public Option<Provider> FindById(string token, string id)
        {
            AddAuthenticationHeader(token);

            var pathParam = $"/{id}";

            using (var response = HttpClient.GetAsync(ProvidersResource + pathParam).Result)
            {
                if (response.IsSuccessStatusCode)
                {
                    var responsePayload = response.Content.ReadAsStringAsync()
                        .Result;

                    return JsonDeserialize<Provider>(responsePayload);
                }
            }

            return null;
        }

        public void Add(string token, Provider provider)
        {
            var content = CreateStringContent(provider);
            using (var response = HttpClient.PostAsync(ProvidersResource, content).Result)
            {
                if (!response.IsSuccessStatusCode)
                {
                    throw new HttpRequestException("Can't add provider");
                }
            }
        }

        public void Update(string token, Provider provider)
        {
            var url = ProvidersResource + $"/{provider.Id}";
            var content = CreateStringContent(provider);
            using (var response = HttpClient.PutAsync(url, content).Result)
            {
                if (!response.IsSuccessStatusCode)
                {
                    throw new HttpRequestException("Can't update provider");
                }
            }
        }
    }
}