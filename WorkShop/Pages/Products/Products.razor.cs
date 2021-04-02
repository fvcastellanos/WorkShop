using System.Collections.Generic;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using WorkShop.Domain;
using WorkShop.Services;

namespace WorkShop.Pages
{
    public class ProductsBase: CrudBase
    {
        [Inject]
        protected ProductService ProductService { get; set; }

        protected IEnumerable<ProductView> Products = new List<ProductView>();

        protected ProductView ProductView;
        protected int TopRows;
        protected int Active;
        protected string Code;
        protected string Name;

        protected override void OnInitialized()
        {
            TopRows = 25;
            Active = 1;
            Code = "";
            Name = "";

            HideAddModal();
            GetProducts();
        }

        protected void GetProducts()
        {
            HideErrorMessage();
            var result = ProductService.GetProducts(TopRows, Code, Name, Active);
            result.Match(right => Products = right, ShowErrorMessage);            
        }

        protected void Search()
        {
            GetProducts();
        }

        protected void GetProduct(string code)
        {
            var result = ProductService.FindByCode(code);

            result.Some(ShowEditModal)
                .None(() => ShowErrorMessage($"Code {code} not found"));
        }

        protected void ShowAddModal()
        {
            HideModalError();
            ShowModal();
            ModifyModal = false;
            ProductView = new ProductView();
            EditContext = new EditContext(ProductView);
        }

        protected override void Update()
        {
            var result = ProductService.Update(ProductView);

            result.Match(right => {

                HideModal();
                HideModalError();
                GetProducts();

            }, DisplayModalError);
        }

        protected override void Add()
        {
            var result = ProductService.Add(ProductView);

            result.Match(right => 
            {
                HideAddModal();
                HideModalError();
                GetProducts();

            }, DisplayModalError);
        }

        // ------------------------------------------------------------------------------------------

        private void ShowEditModal(ProductView productView)
        {
            ProductView = productView;
            EditContext = new EditContext(ProductView);

            ShowEditModal();
        }
    }
}