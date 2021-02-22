using System.Collections.Generic;
using Microsoft.AspNetCore.Components;
using WorkShop.Domain;
using WorkShop.Services;

namespace WorkShop.Pages
{
    public class ProductsBase: PageBase
    {
        [Inject]
        protected ProductService ProductService { get; set; }

        protected IEnumerable<ProductView> Products;

        protected int TopRows;
        protected int Active;

        protected override void OnInitialized()
        {
            TopRows = 25;
            Active = 1;
            GetProducts();
        }

        protected void GetProducts()
        {
            HideErrorMessage();
            var result = ProductService.GetProducts(TopRows, Active);
            result.Match(right => Products = right, ShowErrorMessage);            
        }

        protected void Search()
        {
            GetProducts();
        }
    }
}