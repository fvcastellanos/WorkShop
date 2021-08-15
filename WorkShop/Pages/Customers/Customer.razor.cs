using System.Collections.Generic;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using WorkShop.Domain;
using WorkShop.Services;

namespace WorkShop.Pages
{
    public class CustomerBase : CrudBase
    {
        [Inject]
        protected CustomerService CustomerService { get; set; }
        protected IEnumerable<CustomerView> Customers;
        protected CustomerSearchView SearchView;

        protected CustomerView CustomerView;

        protected override void OnInitialized()
        {

            SearchView = new CustomerSearchView
            {
                Code = "",
                Name = "",
                TopRows = 25,
                Active = 1,
                TaxId = ""
            };

            SearchCustomers();
        }

        protected void SearchCustomers()
        {
            Customers = new List<CustomerView>();

            var result = CustomerService.FindCustomers(SearchView);

            result.Match(right => {

                HideErrorMessage();
                Customers = right;
            }, ShowErrorMessage);
        }
        
        protected void ShowAddModal()
        {
            CustomerView = new CustomerView();
            EditContext = new EditContext(CustomerView);
            ModifyModal = false;
            HideModalError();
            ShowModal();
        }

        protected void GetClient(string id)
        {
            var clientHolder = CustomerService.FindById(id);

            clientHolder.Match(some => {
                
                CustomerView = some;
                EditContext = new EditContext(CustomerView);

                ShowEditModal();

            }, () => ShowErrorMessage($"No se encontró un cliente con Id: {id}"));
        }

        protected override void Add()
        {
            var result = CustomerService.Add(CustomerView);

            result.Match(right => {

                HideAddModal();
                SearchCustomers();
            }, DisplayModalError);
        }

        protected override void Update()
        {
            var result = CustomerService.Update(CustomerView);

            result.Match(right => {

                HideAddModal();
                SearchCustomers();
            }, DisplayModalError);
        }
    }
}