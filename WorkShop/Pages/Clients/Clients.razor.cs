using System.Collections.Generic;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using WorkShop.Domain;
using WorkShop.Services;

namespace WorkShop.Pages
{
    public class ClientBase : CrudBase
    {
        [Inject]
        protected ClientService ClientService { get; set; }
        protected IEnumerable<ClientView> Clients;
        protected ClientSearchView SearchView;

        protected ClientView ClientView;

        protected override void OnInitialized()
        {

            SearchView = new ClientSearchView
            {
                Code = "",
                Name = "",
                TopRows = 25,
                Active = 1,
                TaxId = ""
            };

            SearchClients();
        }


        protected void SearchClients()
        {
            Clients = new List<ClientView>();
        }
        
        protected void ShowAddModal()
        {
            ClientView = new ClientView();
            EditContext = new EditContext(ClientView);
        }

        protected void GetClient(string id)
        {

        }


        protected override void Add()
        {
            throw new System.NotImplementedException();
        }

        protected override void Update()
        {
            throw new System.NotImplementedException();
        }
    }
}