using System.ComponentModel.DataAnnotations;

namespace WorkShop.Domain.Views
{
    public class LoginModel
    {
        [Required(ErrorMessage = "El usuario es requerido")]
        public string User { get; set; }

        [Required(ErrorMessage = "La contraseña es requerida")]
        public string Password { get; set; }
    }
}