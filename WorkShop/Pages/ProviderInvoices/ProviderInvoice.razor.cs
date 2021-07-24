
using Microsoft.AspNetCore.Components;

namespace WorkShop.Pages
{
    public class ProviderInvoiceBase : CrudBase
    {
        [Parameter]
        public long ProviderId { get; set; }
        
        protected override void Add()
        {
            throw new System.NotImplementedException();
        }

        protected override void Update()
        {
            throw new System.NotImplementedException();
        }
    }
}