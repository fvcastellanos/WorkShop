using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using LanguageExt;
using WorkShop.Clients.Domain;

namespace WorkShop.Clients
{
    public class DiscountTypeClient : BaseHttpClient
    {
        private const string DiscountTypesResource = "/discount-types";

        public DiscountTypeClient(IHttpClientFactory httpClientFactory) : base(httpClientFactory)
        {
        }

        public IEnumerable<DiscountType> Find(string token, int topRows, string name, int active)
        {
            var queryString = $"?_where[0][name_contains]={name}&_where[1][active]={active}&_limit={topRows}";
            var url = $"{DiscountTypesResource}{queryString}";

            return Find<DiscountType>(token, url, "Can't get discount type list");
        }

        public Option<DiscountType> FindByName(string token, string name)
        {
            var queryString = $"?_where[0][name]={name}";
            var url = $"{DiscountTypesResource}{queryString}";

            return Find<DiscountType>(token, url, $"Can't get discount type: {name}")
                .FirstOrDefault();
        }

        public Option<DiscountType> FindById(string token, string id)
        {
            var url = $"{DiscountTypesResource}/{id}";
            return FindById<DiscountType>(token, url, $"Can't get discount type with Id: {id}");
        }

        public void Add(string token, DiscountType discountType)
        {
            var content = CreateStringContent(discountType);            
            Add(token, DiscountTypesResource, content, $"Can't add Discount Type: {discountType.Name}");
        }

        public void Update(string token, DiscountType discountType)
        {
            var content = CreateStringContent(discountType);
            var url = $"{DiscountTypesResource}/{discountType.Id}";
            Update(token, url, content, $"Can't update Discount Type {discountType.Name}");
        }

    }
}
