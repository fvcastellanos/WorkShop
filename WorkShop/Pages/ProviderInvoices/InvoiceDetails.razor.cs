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

        protected string DeleteDetailId;
        protected bool DisplayDeleteModal;

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
            DisplayDeleteModal = false;
            GetProductList();
            GetInvoiceDetails();
            LoadInvoiceInformation();
        }

        protected override void Add()
        {
            var result = ProviderInvoiceService.AddDetail(InvoiceDetailView);

            result.Match(right => {

                HideAddModal();
                GetInvoiceDetails();
            }, DisplayModalError);
        }

        protected override void Update()
        {
            var result = ProviderInvoiceService.UpdateDetail(InvoiceDetailView);

            result.Match(right => {

                HideAddModal();
                GetInvoiceDetails();
            }, DisplayModalError);
        }

        protected void ShowDeleteModal(string id)
        {
            DisplayDeleteModal = true;
            HideModalError();
            DeleteDetailId = id;
        }

        protected void HideDeleteModal()
        {
            DisplayDeleteModal = false;
            HideModalError();
            DeleteDetailId = "";
        }

        protected void DeleteDetail()
        {
            var result = ProviderInvoiceService.DeleteDetail(DeleteDetailId);

            result.Match(right => {

                HideDeleteModal();
                GetInvoiceDetails();
            }, ShowErrorMessage);
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

        protected void GetInvoiceDetails()
        {
            InvoiceDetails = new List<InvoiceDetailView>();

            var result = ProviderInvoiceService.GetInvoiceDetails(InvoiceId);

            result.Match(right => {

                InvoiceDetails = right;
                LoadInvoiceInformation();
                HideErrorMessage();
            }, ShowErrorMessage);
        }

        protected void GetDetail(string id)
        {
            var holder = ProviderInvoiceService.GetDetail(id);

            holder.Match(some => {
                
                InvoiceDetailView = some;
                EditContext = new EditContext(InvoiceDetailView);

                ShowEditModal();
            }, () => ShowErrorMessage($"No se encuentra el detalle de la factura con id: {id}"));
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