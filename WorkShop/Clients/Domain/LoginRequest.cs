
using System.Text.Json.Serialization;

namespace WorkShop.Clients.Domain
{
    public class LoginRequest
    {
        [JsonPropertyName("identifier")]
        public string Identifier { get; set; }

        [JsonPropertyName("password")]
        public string Password { get; set; }
    }
}