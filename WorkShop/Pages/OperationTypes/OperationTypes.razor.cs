
using System.Collections.Generic;
using Microsoft.AspNetCore.Components.Forms;
using WorkShop.Domain;

namespace WorkShop.Pages
{
    public class OperationTypesBase : CrudBase
    {
        // protected 
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
            GetOperationTypes();
        }

        protected void GetOperationTypes()
        {

        }

        protected void HideAddModal()
        {
            HideModalError();
            HideModal();
        }

        protected void ShowAddModal()
        {
            HideModalError();
            ShowModal();
            ModifyModal = false;
            OperationTypeView = new OperationTypeView();
            EditContext = new EditContext(OperationTypeView);
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