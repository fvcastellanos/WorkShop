
using System.Collections.Generic;
using WorkShop.Domain;

namespace WorkShop.Pages
{
    public class OperationTypesBase : CrudBase
    {
        protected OperationTypeView OperationTypeView;
        protected IEnumerable<OperationTypeView> OperationTypes = new List<OperationTypeView>();
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
            // GetProducts();
        }

        protected void HideAddModal()
        {

        }

        protected void ShowAddModal()
        {

        }

        protected void Search()
        {

        }

        protected void GetOperationType(string code)
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