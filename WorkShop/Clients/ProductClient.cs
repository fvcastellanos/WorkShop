using System.Collections.Generic;
using System.Net.Http;
using LanguageExt;
using WorkShop.Clients.Domain;

namespace WorkShop.Clients
{
    public class ProductClient : BaseHttpClient
    {
        private const string ProductsResource = "/products";

        public ProductClient(IHttpClientFactory httpClientFactory) : base(httpClientFactory)
        {
        }

        public IEnumerable<Product> Find(string token,
                                         int topRows,
                                         string code,
                                         string name,
                                         int active)
        {
            AddAuthenticationHeader(token);

            // GET /restaurants?_where[0][stars]=1&_where[1][pricing_lte]=20

            var queryString = $"?_where[0][code_contains]={code}&_where[1][name_contains]={name}&_where[2][active]={active}&_limit={topRows}";
            var url = $"{ProductsResource}{queryString}";

            using (var response = HttpClient.GetAsync(url).Result)
            {
                if (response.IsSuccessStatusCode)
                {
                    var responsePayload = response.Content.ReadAsStringAsync()
                        .Result;

                    return JsonDeserialize<List<Product>>(responsePayload);
                }
            }

            throw new HttpRequestException("Can't get product list");
        }

        public Option<Product> FindById(string token, string id)
        {
            AddAuthenticationHeader(token);

            var url = $"{ProductsResource}/{id}";

            using (var response = HttpClient.GetAsync(url).Result)
            {
                if (response.IsSuccessStatusCode)
                {
                    var responsePayload = response.Content.ReadAsStringAsync()
                        .Result;

                    return JsonDeserialize<Product>(responsePayload);
                }

                if (response.StatusCode.Equals(404))
                {
                    return null;
                }
            }

            throw new HttpRequestException($"Can't search provider with Id: {id}");
        }

        public void Add(string token, Product provider)
        {
            AddAuthenticationHeader(token);

            var content = CreateStringContent(provider);
            using (var response = HttpClient.PostAsync(ProductsResource, content).Result)
            {
                if (!response.IsSuccessStatusCode)
                {
                    throw new HttpRequestException("Can't add product");
                }
            }
        }

        public void Update(string token, Product provider)
        {
            AddAuthenticationHeader(token);

            var url = $"{ProductsResource}/{provider.Id}";
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