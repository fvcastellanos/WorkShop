
using System.Net.Http;
using System.Text;
using System.Text.Json;
using LanguageExt;
using WorkShop.Clients.Domain;

namespace WorkShop.Clients
{
    public class LoginClient: BaseHttpClient
    {
        private const string LoginResource = "/auth/local";

        public LoginClient(IHttpClientFactory httpClientFactory) : base(httpClientFactory)
        {
        }

        public Option<LoginResponse> PerformLogin(string user, string password)
        {
            var request = new LoginRequest()
            {
                Identifier = user,
                Password = password
            };

            var payload = JsonSerializer.Serialize(request);
            var content = new StringContent(payload, Encoding.UTF8, "application/json");

            // return new LoginResponse
            // {
            //     Jwt = "test-jwt",
            //     User = new User
            //     {
            //         Email = "jpenas@test.com",
            //         Username = "jpenas"
            //     }
            // };

            using (var response = HttpClient.PostAsync(LoginResource, content).Result)
            {
                if (response.IsSuccessStatusCode)
                {
                    var responsePayload = response.Content.ReadAsStringAsync()
                        .Result;

                    return JsonSerializer.Deserialize<LoginResponse>(responsePayload, JsonSerializerOptions);
                }
            }

            return null;
        }
    }
}