using System.ComponentModel.DataAnnotations;

namespace Actual.Models
{
    public class ForgotPassword
    {
        [Required(ErrorMessage = "El correo electrónico es obligatorio")]
        [EmailAddress(ErrorMessage = "El correo electrónico no es válido")]
        public string Email { get; set; }
    }
}
