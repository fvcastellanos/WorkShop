using System.Net.Http;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using WorkShop.Pages.Login.Model;
using WorkShop.Controllers.Model;
using System.Text.Json;
using System.Text;
using Blazored.SessionStorage;

namespace WorkShop.Pages
{
    public class LoginBase: PageBase
    {

        [Inject]
        protected IHttpClientFactory HttpClientFactory { get; set; }

        [Inject]
        protected ISessionStorageService SessionStorageService { get; set; }

        [Inject]
        protected NavigationManager NavigationManager { get; set; }

        protected EditContext EditContext;
        public LoginModel Model;

        protected override void OnInitialized()
        {
            Model = new LoginModel();
            EditContext = new EditContext(Model);
        }

        protected void Login()
        {
            HideErrorMessage();

            if (EditContext.Validate()) 
            {
                if (PerformLogin(Model.User, Model.Password))
                {
                    NavigationManager.NavigateTo("/");
                }

                ShowErrorMessage($"Error al autenticar al usuario: {Model.User}");

            }            
        }

        private bool PerformLogin(string user, string password)
        {
            var request = new LoginRequest()
            {
                User = user,
                Password = password
            };

            var httpClient = HttpClientFactory.CreateClient("api");

            var payload = JsonSerializer.Serialize(request);
            var content = new StringContent(payload, Encoding.UTF8, "application/json");

            using (var response = httpClient.PostAsync("/auth/login", content).Result)
            {
                if (response.IsSuccessStatusCode)
                {
                    var token = response.Content.ReadAsStringAsync()
                        .Result;

                    SessionStorageService.SetItemAsync<string>("token", token);

                    return true;
                }
            }

            return false;
        }
    }

}