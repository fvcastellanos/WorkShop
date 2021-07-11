using System.ComponentModel.DataAnnotations;

namespace WorkShop.Pages.Login.Model
{
    public class LoginModel
    {
        [Required]
        [MaxLength(50)]
        public string User { get; set; }

        [Required]
        [MaxLength(50)]
        public string Password { get; set; }
    }
}