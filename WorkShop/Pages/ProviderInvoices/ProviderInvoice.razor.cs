
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using WorkShop.Domain;
using WorkShop.Services;

namespace WorkShop.Pages
{
    public class ProviderInvoiceBase : CrudBase
    {
        [Inject]
        protected ProviderService ProviderService { get; set; }

        [Inject]
        protected ProviderInvoiceService ProviderInvoiceService { get; set; }
        
        protected IEnumerable<InvoiceView> Invoices;
        protected IEnumerable<ProviderView> ProviderViews;

        protected InvoiceSearchView InvoiceSearchView;

        protected ProviderView ProviderView;

        protected InvoiceView InvoiceView;
        protected override void OnInitialized()
        {
            InvoiceSearchView = new InvoiceSearchView
            {
                TopRows = 25,
                Active = 1,
                Serial = "",
                Number = "",
                ProviderCode = "",
                ProviderName = "",
                Year = DateTime.Now.Year,
                Month = DateTime.Now.Month
            };

            // FindParentProvider(ProviderId);
            GetActiveProviders();
            FindInvoices();
        }

        protected void ShowAddModal()
        {
            InvoiceView = new InvoiceView
            {
                Created = DateTime.Now,
                Active = 1
            };

            EditContext = new EditContext(InvoiceView);
            
            ModifyModal = false;

            HideModalError();
            ShowModal();
        }

        protected override void Add()
        {
            var result = ProviderInvoiceService.Add(InvoiceView);

            result.Match(right => {

                HideAddModal();
                FindInvoices();
                GetActiveProviders();

            }, DisplayModalError);
        }

        protected override void Update()
        {
            // var result = ProviderInvoiceService.Update(ProviderId, ProviderInvoiceView);

            // result.Match(right => {

            //     HideModal();
            //     FindInvoices();
            // }, DisplayModalError);
        }

        protected void FindInvoices()
        {
            HideErrorMessage();
            Invoices = new List<InvoiceView>();

            var result = ProviderInvoiceService.GetInvoices(InvoiceSearchView);

            result.Match(right => {

                Invoices = right;

            }, ShowErrorMessage);
        }

        protected void GetInvoice(string id)
        {
            var holder = ProviderInvoiceService.GetInvoice(id);

            holder.Match(ShowEditModal, () => ShowErrorMessage($"Can't find invoice with Id: {id}"));
        }

        protected async Task<IEnumerable<ProviderView>> SearchProviders(string text)
        {
            return await Task.FromResult(
                ProviderViews
                    .Where(provider => provider.Name.ToLower().Contains(text.ToLower())
                        || provider.Code.ToLower().Contains(text.ToLower()))
                    .ToList()
            );
        }

        protected void GetActiveProviders()
        {
            ProviderViews = ProviderService.GetActiveProviders();
        }

        // -----------------------------------------------------------------------------------------------
        private void FindParentProvider(long id)
        {
            // var providerHolder = ProviderService.FindByCode(id.ToString());

            // providerHolder.Match(provider => {

            //     ProviderView = new ProviderView
            //     {
            //         Name = provider.Name,
            //         TaxId = provider.TaxId
            //     };

            // }, () => ShowErrorMessage("Can't get Provider information"));
        }

        private void ShowEditModal(InvoiceView view)
        {
            InvoiceView = view;
            EditContext = new EditContext(InvoiceView);

            ShowEditModal();
        }
    }
}
