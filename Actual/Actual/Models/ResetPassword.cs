using System.ComponentModel.DataAnnotations;

namespace Actual.Models
{
    public class ResetPassword
    {
        [Required]
        public string Token { get; set; }
        [Required]
        public string NewPassword { get; set; }
    }
}
