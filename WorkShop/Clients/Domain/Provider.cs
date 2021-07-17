using System.Text.Json.Serialization;

namespace WorkShop.Clients.Domain
{
    public class Provider
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("code")]
        public string Code { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("contact")]
        public string Contact { get; set; }

        [JsonPropertyName("taxId")]
        public string TaxId { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }
    }
}
