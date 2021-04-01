using System.Collections.Generic;
using WorkShop.Domain;

namespace WorkShop.Pages
{
    public class ProviderBase : CrudBase
    {
        protected SearchView SearchView;
        protected ProviderView ProviderView;
        protected IEnumerable<ProviderView> Providers;

        protected override void OnInitialized()
        {

            SearchView = new SearchView()
            {
                TopRows = 25,
                Active = 1,
                Code = "",
                Name = ""
            };

            HideAddModal();
            GetProviders();
        }

        protected void GetProviders()
        {
            Providers = new List<ProviderView>();
        }

        protected void GetProvider(string code)
        {

        }

        protected void ShowAddModal()
        {

        }

        protected void HideAddModal()
        {

        }

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