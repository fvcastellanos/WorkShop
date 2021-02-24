using System.Collections.Generic;
using Microsoft.AspNetCore.Components;
using WorkShop.Domain;
using WorkShop.Services;

namespace WorkShop.Pages
{
    public class ProductsBase: CrudBase
    {
        [Inject]
        protected ProductService ProductService { get; set; }

        protected IEnumerable<ProductView> Products;

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

        protected void SaveChanges()
        {
            if (ModifyModal)
            {
                UpdateProduct();
                return;
            }

            AddProduct();
        }

        protected void GetProduct(string code)
        {
            var result = ProductService.FindByCode(code);

            result.Some(ShowEditModal)
                .None(() => ShowErrorMessage($"Code {code} not found"));
        }

        protected void ShowEditModal()
        {
            HideModalError();
            ShowModal();
            ModifyModal = true;
        }

        protected void ShowAddModal()
        {
            HideModalError();
            ShowModal();
            ModifyModal = false;
            ProductView = new ProductView();
        }

        protected void HideAddModal()
        {
            HideModalError();
            HideModal();
        }


        // ------------------------------------------------------------------------------------------

        private void AddProduct()
        {
            var result = ProductService.Add(ProductView);

            result.Match(right => 
            {
                HideAddModal();
                HideModalError();
                GetProducts();

            }, DisplayModalError);

        }

        private void UpdateProduct()
        {
            var result = ProductService.Update(ProductView);

            result.Match(right => {

                HideModal();
                HideModalError();
                GetProducts();

            }, DisplayModalError);
        }

        private void ShowEditModal(ProductView productView)
        {
            ProductView = productView;
            ShowEditModal();
        }
    }
}