using System.Collections.Generic;
using Microsoft.AspNetCore.Components;
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
            Search = new SearchView()
            {
                TopRows = 25,
                Name = ""
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

        }

        protected void ShowAddModal()
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