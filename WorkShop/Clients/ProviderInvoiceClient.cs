using System.Collections.Generic;
using System.Net.Http;
using WorkShop.Clients.Domain;

namespace WorkShop.Clients
{
    public class ProviderInvoiceClient : BaseHttpClient
    {
        private const string ProvicerInvoiceResource = "/provider-invoices";
        public ProviderInvoiceClient(IHttpClientFactory httpClientFactory) : base(httpClientFactory)
        {
        }

        public IEnumerable<ProviderInvoice> Find(string token, 
                                                 int topRows,
                                                 long providerId,
                                                 string number,
                                                 int active)
        {
            // var queryString = $"?_where[0][code_contains]={code}&_where[1][name_contains]={name}&_where[2][active]={active}&_limit={topRows}";
            var queryString = $"?_where[0][provider.id]={providerId}&_where[1][number_contains]={number}&_where[2][active]={active}&_limit={topRows}";
            var url = $"{ProvicerInvoiceResource}{queryString}";

            return Find<ProviderInvoice>(token, url, "Can't get invoice list");            
        }

        public void Add(string token, ProviderInvoice providerInvoice)
        {
            var content = CreateStringContent(providerInvoice);
            Add(token, ProvicerInvoiceResource, content, $"Can't add invoice for provider: {providerInvoice.Number}");
        }
    }
}