using System;
using System.Text.Json.Serialization;

namespace WorkShop.Clients.Domain
{
    public class ProviderInvoice
    {
        [JsonPropertyName("provider")]
        public Provider Provider { get; set; }

        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("created")]
        public DateTime Created { get; set; }

        [JsonPropertyName("suffix")]
        public string Suffix { get; set; }

        [JsonPropertyName("number")]
        public string Number { get; set; }

        [JsonPropertyName("amount")]
        public double Amount { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("active")]
        public bool Active { get; set; }
    }
}