using System.Text.Json.Serialization;

namespace WorkShop.Clients.Domain
{
    public class Product
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("code")]
        public string Code { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("minimalAmount")]
        public int MinimalAmount { get; set; }

        [JsonPropertyName("salePrice")]
        public double SalePrice { get; set; }

        [JsonPropertyName("active")]
        public bool Active { get; set; }        
    }
}