using Microsoft.AspNetCore.Components;
using WorkShop.Services;

namespace WorkShop.Pages
{
    public class ProductsBase: PageBase
    {
        [Inject]
        protected ProductService ProductService { get; set; }

        protected override void OnInitialized()
        {
            
        }

        protected void GetProducts()
        {
            ProductService.GetProducts();
        }
    }
}