using System.Text.Json.Serialization;

namespace WorkShop.Clients.Domain
{
    public class OperationType
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("inbound")]
        public bool Inbound { get; set; }

        [JsonPropertyName("active")]
        public bool Active { get; set; }
    }
}