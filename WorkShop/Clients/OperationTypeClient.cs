using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using LanguageExt;
using WorkShop.Clients.Domain;

namespace WorkShop.Clients
{
    public class OperationTypeClient : BaseHttpClient
    {
        private const string OperationTypesResource = "/operation-types";

        public OperationTypeClient(IHttpClientFactory httpClientFactory) : base(httpClientFactory)
        {
        }

        public IEnumerable<OperationType> Find(string token, 
                                               int topRows, 
                                               string name, 
                                               int active)
        {
            var queryString = $"?_where[0][name_contains]={name}&_where[1][active]={active}&_limit={topRows}";
            var url = $"{OperationTypesResource}{queryString}";

            return Find<OperationType>(token, url, "Can't get Operation Types");
        }

        public Option<OperationType> FindById(string token, string id)
        {
            var url = $"{OperationTypesResource}/{id}";
            return FindById<OperationType>(token, url, $"Can't search provider with id: {id}");
        }

        public Option<OperationType> FindByName(string token, string name)
        {
            var query = $"?_where[0][name]={name}";
            var url = $"{OperationTypesResource}/{query}";

            return Find<OperationType>(token, url, $"Can't search provider: {name}")
                .FirstOrDefault();
        }

        public void Add(string token, OperationType operationType)
        {
            var content = CreateStringContent(operationType);            
            Add(token, OperationTypesResource, content, $"Can't add Operation Type: {operationType.Name}");
        }

        public void Update(string token, OperationType operationType)
        {
            var url = $"{OperationTypesResource}/{operationType.Id}";
            var content = CreateStringContent(operationType);

            Update(token, url, content, $"Can't update Operation Type: {operationType.Name}");
        }

    }
}