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
            // GET /restaurants?_where[0][stars]=1&_where[1][pricing_lte]=20

            var queryString = $"?_where[0][code_contains]={code}&_where[1][name_contains]={name}&_where[2][active]={active}&_limit={topRows}";
            var url = $"{ProductsResource}{queryString}";

            return Find<Product>(token, url, "Can't get product list");
        }

        public Option<Product> FindById(string token, string id)
        {
            var url = $"{ProductsResource}/{id}";

            return FindById<Product>(token, url, $"Can't search provider with Id: {id}");
        }

        public void Add(string token, Product product)
        {

            var content = CreateStringContent(product);
            Add(token, ProductsResource, content, $"Can't add product: {product.Name}");
        }

        public void Update(string token, Product provider)
        {

            var url = $"{ProductsResource}/{provider.Id}";
            var content = CreateStringContent(provider);

            Update(token, url, content, $"Can't update provider {provider.Name}");
        }
    }
}