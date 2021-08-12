using System.Collections.Generic;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using WorkShop.Domain;
using WorkShop.Services;

namespace WorkShop.Pages
{
    public class DiscountTypesBase : CrudBase
    {
        [Inject]
        protected DiscountTypeService Service { get; set; }

        protected SearchView Search;

        protected DiscountTypeView DiscountTypeView;

        protected IEnumerable<DiscountTypeView> DiscountTypes = new List<DiscountTypeView>();

        protected override void OnInitialized()
        {
            Search = new SearchView
            {
                TopRows = 25,
                Name = "",
                Active = 1
            };

            GetDiscountTypes();
        }

        protected void GetDiscountTypes()
        {
            HideErrorMessage();
            var result = Service.GetDiscountTypes(Search.TopRows, Search.Name, Search.Active);

            result.Match(right => {

                DiscountTypes = right;

            }, ShowErrorMessage);
        }

        protected void GetDiscountType(string id)
        {
            var result = Service.FindById(id);

            result.Match(ShowEditModal, 
                () => DisplayModalError($"Discount Type with Id: {id} not found"));
        }

        protected void ShowAddModal()
        {
            DiscountTypeView = new DiscountTypeView();
            EditContext = new EditContext(DiscountTypeView);
            ModifyModal = false;
            HideModalError();
            ShowModal();
        }

        protected override void Add()
        {
            var result = Service.Add(DiscountTypeView);

            result.Match(right => {

                HideAddModal();
                GetDiscountTypes();

            }, DisplayModalError);
        }

        protected override void Update()
        {
            var result = Service.Update(DiscountTypeView);

            result.Match(right => {
                
                HideModalError();
                HideModal();
                GetDiscountTypes();

            }, DisplayModalError);

        }

        // -------------------------------------------------------------------

        private void ShowEditModal(DiscountTypeView view)
        {
            DiscountTypeView = view;
            EditContext = new EditContext(DiscountTypeView);
            ShowEditModal();
        }
    }
}