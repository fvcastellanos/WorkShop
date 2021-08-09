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
    public class InvoiceDetailsBase : CrudBase
    {
        [Parameter]
        public string InvoiceId { get; set; }

        [Inject]
        protected ProviderInvoiceService ProviderInvoiceService { get; set; }

        [Inject]
        protected ProductService ProductService { get; set; }

        protected InvoiceView InvoiceView;

        protected InvoiceDetailView InvoiceDetailView;

        protected IEnumerable<InvoiceDetailView> InvoiceDetails;

        protected IEnumerable<ProductView> Products;

        protected override void OnInitialized()
        {
            GetProductList();
            LoadInvoiceInformation();
            InvoiceDetails = new List<InvoiceDetailView>();
        }

        protected override void Add()
        {
            var result = ProviderInvoiceService.AddDetail(InvoiceDetailView);

            result.Match(right => {

                HideAddModal();
                GetProductList();
            }, DisplayModalError);
        }

        protected override void Update()
        {
            throw new System.NotImplementedException();
        }

        protected void ShowAddModal()
        {
            InvoiceDetailView = new InvoiceDetailView
            {
                InvoiceId = InvoiceId
            };

            EditContext = new EditContext(InvoiceDetailView);
            
            ModifyModal = false;

            HideModalError();
            ShowModal();
        }

        protected void GetProductList()
        {
            Products = ProductService.GetActiveProducts();
        }

        protected async Task<IEnumerable<ProductView>> SearchProducts(string text)
        {
            return await Task.FromResult(
                Products
                    .Where(product => product.Name.ToLower().Contains(text.ToLower())
                        || product.Code.ToLower().Contains(text.ToLower()))
                    .ToList()
            );
        }

        // ------------------------------------------------------------------------------------------------------------

        private void LoadInvoiceInformation()
        {
            InvoiceView = new InvoiceView();

            var holder = ProviderInvoiceService.GetInvoice(InvoiceId);

            holder.Match(some => {
                InvoiceView = some;
            },
            () => ShowErrorMessage($"No se encontra información de la factura: {InvoiceId}"));
        }

    }
}