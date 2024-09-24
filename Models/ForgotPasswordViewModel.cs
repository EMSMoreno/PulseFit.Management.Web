using System.ComponentModel.DataAnnotations;

namespace PulseFit.Management.Web.Models
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
