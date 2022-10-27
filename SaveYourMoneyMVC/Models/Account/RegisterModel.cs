using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace SaveYourMoneyMVC.Models.Account
{
    public class RegisterModel : LoginModel
    {
        [Required]
        [StringLength(50, ErrorMessage = "El nombre de usuario no debe de superar {1} caracteres")]
        [RegularExpression(@"[A-Za-z0-9]+", ErrorMessage = "El nombre de usuario solo debe contener caracteres alfanúmericos")]
        public string? Name { get; set; }

        [Required]
        [PasswordPropertyText]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Password mismatch")]
        public string? VerifiedPassword { get; set; }

        [Required]
        public string Language { get; set; }
    }
}
