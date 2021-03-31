
using System.Collections.Generic;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using WorkShop.Domain;
using WorkShop.Services;

namespace WorkShop.Pages
{
    public class OperationTypesBase : CrudBase
    {
        [Inject]
        protected OperationTypeService OperationTypeService { get; set; }
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
            HideErrorMessage();
            var result = OperationTypeService.GetOperationTypes(TopRows, Name, Active);
            result.Match(right => OperationTypes = right, ShowErrorMessage);            
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

        protected void GetOperationType(string code)
        {

        }

        protected override void Add()
        {
            var result = OperationTypeService.Add(OperationTypeView);

            result.Match(right => {

                HideAddModal();
                HideModal();
                GetOperationTypes();
                
            }, DisplayModalError);
        }

        protected override void Update()
        {
            throw new System.NotImplementedException();
        }
    }
}