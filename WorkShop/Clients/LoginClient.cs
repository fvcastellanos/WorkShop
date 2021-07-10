
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using LanguageExt;
using WorkShop.Clients.Domain;

namespace WorkShop.Clients
{
    public class LoginClient: BaseHttpClient
    {
        private const string LoginResource = "/local/auth";

        public LoginClient(HttpClient httpClient) : base(httpClient)
        {
        }

        public async Task<LoginResponse> PerformLoginAsync(string user, string password)
        {
            var request = new LoginRequest()
            {
                Identifier = user,
                Password = password
            };

            var payload = JsonSerializer.Serialize(request);
            var content = new StringContent(payload, Encoding.UTF8, "application/json");

            using (var response = await HttpClient.PostAsync(LoginResource, content))
            {
                if (response.IsSuccessStatusCode)
                {
                    var responsePayload = await response.Content.ReadAsStringAsync();
                    return JsonSerializer.Deserialize<LoginResponse>(responsePayload);
                }

            }

            return null;
        }
    }
}