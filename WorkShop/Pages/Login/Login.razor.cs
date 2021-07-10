
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using WorkShop.Services;

namespace WorkShop.Pages
{
    public class LoginBase: PageBase
    {
        [Inject]
        protected LoginService LoginService;       
    }
}