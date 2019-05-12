using System.ComponentModel.DataAnnotations;

namespace Authentication.ViewModel
{
    public class CreateUserModel
    {
        [Required(ErrorMessage = "Email is required")]
        [MaxLength(50, ErrorMessage = "Max Length is 50 caracters")]
        public string Email { get; set; }

        [Required(ErrorMessage = "UserName is required")]
        [MaxLength(50, ErrorMessage = "Max Length is 50 caracters")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Passswords is required")]
        [MaxLength(50, ErrorMessage = "Max Length is 50 caracters")]
        public string Password { get; set; }
    }
}
