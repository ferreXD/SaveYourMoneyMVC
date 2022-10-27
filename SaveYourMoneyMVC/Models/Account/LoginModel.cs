using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Security.Policy;

namespace SaveYourMoneyMVC.Models.Account
{
    public class LoginModel
    {
        [Required]
        [EmailAddress]
        [RegularExpression(@"^[\w-_]+(\.[\w!#$%'*+\/=?\^`{|}]+)*@((([\-\w]+\.)+[a-zA-Z]{2,20})|(([0-9]{1,3}\.){3}[0-9]{1,3}))$", ErrorMessage = "El formato del email no es válido")]
        public string? Email { get; set; }

        [Required]
        [PasswordPropertyText]
        [DataType(DataType.Password)]
        [StringLength(50, ErrorMessage = "La contraseña debe de tener entre {2} y {1} caracteres", MinimumLength = 6)]
        public string? Password { get; set; }

        public bool RememberMe { get; set; }
    }
}
