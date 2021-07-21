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

            var queryString = $"?_where[0][code_contains]={code}&_where[1][name_contains]={name}&_where[2][active]={active}&_limit={topRows}";
            var url = $"{ProvidersResource}{queryString}";

            return Find<Provider>(token, url, "Can't get provider list");
        }

        public Option<Provider> FindById(string token, string id)
        {

            var url = $"{ProvidersResource}/{id}";
            return FindById<Provider>(token, url, $"Can't search provider with Id: {id}");
        }

        public void Add(string token, Provider provider)
        {
            var content = CreateStringContent(provider);            
            Add(token, ProvidersResource, content, $"Can't add provider: {provider.Name}");
        }

        public void Update(string token, Provider provider)
        {
            var url = ProvidersResource + $"/{provider.Id}";
            var content = CreateStringContent(provider);

            Update(token, url, content, $"Can't update provider: {provider.Name}");
        }
    }
}