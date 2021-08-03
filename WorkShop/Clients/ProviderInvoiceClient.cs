using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using LanguageExt;
using WorkShop.Clients.Domain;

namespace WorkShop.Clients
{
    public class ProviderInvoiceClient : BaseHttpClient
    {
        private const string ProviderInvoiceResource = "/provider-invoices";
        public ProviderInvoiceClient(IHttpClientFactory httpClientFactory) : base(httpClientFactory)
        {
        }

        public IEnumerable<ProviderInvoice> Find(string token, 
                                                 int topRows,
                                                 long providerId,
                                                 string serial,
                                                 string number,
                                                 int active)
        {
            // var queryString = $"?_where[0][code_contains]={code}&_where[1][name_contains]={name}&_where[2][active]={active}&_limit={topRows}";
            // var queryString = $"?_where[0][provider.id]={providerId}&_where[1][number_contains]={number}&_where[2][active]={active}&_limit={topRows}";
            var queryString = $"?_where[0][provider.id]={providerId}&_where[1][serial_contains]={serial}&_where[2][number_contains]={number}&_where[3][active]={active}&_limit={topRows}";
            var url = $"{ProviderInvoiceResource}{queryString}";

            return Find<ProviderInvoice>(token, url, "Can't get invoice list");            
        }

        public Option<ProviderInvoice> FindByNumber(string token, long providerId, string serial, string number)
        {
            var queryString = $"?_where[0][provider.id]={providerId}&_where[1][number]={number}&_where[2][serial]={serial}";

            var url = $"{ProviderInvoiceResource}{queryString}";
            return Find<ProviderInvoice>(token, url, "Can't get invoice")
                        .FirstOrDefault();
        }

        public Option<ProviderInvoice> FindById(string token, string invoiceId)
        {
            var url = $"{ProviderInvoiceResource}/{invoiceId}";
            return FindById<ProviderInvoice>(token, url, $"Can't get invoice with id: {invoiceId}");
        }

        public void Add(string token, ProviderInvoice providerInvoice)
        {
            var content = CreateStringContent(providerInvoice);
            Add(token, ProviderInvoiceResource, content, $"Can't add invoice for provider: {providerInvoice.Number}");
        }

        public void Update(string token, ProviderInvoice providerInvoice)
        {
            var content = CreateStringContent(providerInvoice);
            var url = $"{ProviderInvoiceResource}/{providerInvoice.Id}";
            Update(token, url, content, $"Can't update invoice: {providerInvoice.Number}");
        }
    }
}