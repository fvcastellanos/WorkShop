
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using WorkShop.Domain;
using WorkShop.Services;

namespace WorkShop.Pages
{
    public class ProviderInvoiceBase : CrudBase
    {
        [Parameter]
        public long ProviderId { get; set; }

        [Inject]
        protected ProviderService ProviderService { get; set; }

        [Inject]
        protected ProviderInvoiceService ProviderInvoiceService { get; set; }
        
        protected IEnumerable<ProviderInvoiceView> ProviderInvoices;

        protected SearchView SearchView;

        protected ProviderView ProviderView;

        protected ProviderInvoiceView ProviderInvoiceView;
        protected override void OnInitialized()
        {
            SearchView = new SearchView
            {
                TopRows = 25,
                Active = 1,
                Number = "",
                Year = DateTime.Now.Year,
                Month = DateTime.Now.Month
            };

            FindParentProvider(ProviderId);
            FindInvoices();
        }

        protected void ShowAddModal()
        {
            ProviderInvoiceView = new ProviderInvoiceView
            {
                Created = DateTime.Now,
                Active = 1
            };

            EditContext = new EditContext(ProviderInvoiceView);
            ModifyModal = false;

            HideModalError();
            ShowModal();
        }

        protected override void Add()
        {
            var result = ProviderInvoiceService.Add(ProviderId, ProviderInvoiceView);

            result.Match(right => {

                HideAddModal();
                FindInvoices();

            }, DisplayModalError);
        }

        protected override void Update()
        {
            throw new System.NotImplementedException();
        }

        protected void FindInvoices()
        {
            ProviderInvoices = new List<ProviderInvoiceView>();

            var result = ProviderInvoiceService.GetInvoices(ProviderId, SearchView.Number, SearchView.Active);

            result.Match(right => {

                ProviderInvoices = right;

            }, ShowErrorMessage);
        }

        protected void GetInvoice(string id)
        {
            var holder = ProviderInvoiceService.GetInvoice(id);

            holder.Match(ShowEditModal, () => ShowErrorMessage($"Can't find invoice with Id: {id}"));
        }

        // -----------------------------------------------------------------------------------------------
        private void FindParentProvider(long id)
        {
            var providerHolder = ProviderService.FindById(id.ToString());

            providerHolder.Match(provider => {

                ProviderView = new ProviderView
                {
                    Name = provider.Name,
                    TaxId = provider.TaxId
                };

            }, () => ShowErrorMessage("Can't get Provider information"));
        }

        private void ShowEditModal(ProviderInvoiceView view)
        {
            ProviderInvoiceView = view;
            EditContext = new EditContext(ProviderInvoiceView);

            ShowEditModal();
        }

    }
}
