namespace WorkShop.Pages
{
    public abstract class CrudBase: PageBase
    {
        protected bool DisplayAddModal;
        protected bool DisplayEditModal;
        protected bool DisplayDeleteModal;
        protected bool DisplayViewModal;
        protected bool HasModalError;
        protected string ModalErrorMessage;

        protected abstract void ShowAddModal();

        protected abstract void HideAddModal();

        protected void DisplayModalError(string error)
        {
            HasModalError = true;
            ModalErrorMessage = error;
        }

        protected void HideModalError()
        {
            HasModalError = false;
            ModalErrorMessage = "";
        }
    }
}