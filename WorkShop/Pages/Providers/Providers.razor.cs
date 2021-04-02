using System.Collections.Generic;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using WorkShop.Domain;
using WorkShop.Services;

namespace WorkShop.Pages
{
    public class ProviderBase : CrudBase
    {
        [Inject]
        protected ProviderService Service { get; set; }
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
            HideErrorMessage();

            var result = Service.GetProviders(SearchView.TopRows, SearchView.Code, SearchView.Name, SearchView.Active);
            result.Match(right => Providers = right, ShowErrorMessage);
        }

        protected void GetProvider(string code)
        {
            var holder = Service.FindByCode(code);

            holder.Match(ShowEditModal, 
                () => ShowErrorMessage($"Provider with code: {code} not found"));
        }

        protected void ShowAddModal()
        {
            ProviderView = new ProviderView();
            EditContext = new EditContext(ProviderView);
            ModifyModal = false;

            HideModalError();
            ShowModal();
        }

        protected override void Add()
        {
            var result = Service.Add(ProviderView);

            result.Match(right => {

                HideAddModal();
                GetProviders();

            }, DisplayModalError);
        }

        protected override void Update()
        {
            var result = Service.Update(ProviderView);

            result.Match(right => {

                HideAddModal();
                GetProviders();

            }, DisplayModalError);
        }

        // ------------------------------------------------------------------------

        private void ShowEditModal(ProviderView view)
        {
            ProviderView = view;
            EditContext = new EditContext(ProviderView);

            ShowEditModal();
        }
    }
}