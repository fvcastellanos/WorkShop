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

            AddProduct();
        }

        protected override void ShowAddModal()
        {
            HideModalError();
            DisplayAddModal = true;
            ProductView = new ProductView();
        }

        protected override void HideAddModal()
        {
            HideModalError();
            DisplayAddModal = false;
        }

        // ------------------------------------------------------------------------------------------

        private void AddProduct()
        {
            var result = ProductService.AddProduct(ProductView);

            result.Match(right => 
            {
                HideAddModal();
                HideModalError();
                GetProducts();

            }, DisplayModalError);

        }
    }
}