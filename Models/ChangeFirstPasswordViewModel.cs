using System.ComponentModel.DataAnnotations;

namespace PulseFit.Management.Web.Models
{
    public class ChangeFirstPasswordViewModel
    {
        public string Email { get; set; }

        public string Token { get; set; }

        [Required]
        [Display(Name = "Temporary Password")]
        [DataType(DataType.Password)]
        public string TemporaryPassword { get; set; }

        [Required]
        [Display(Name = "New Password")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [Required]
        [Display(Name = "Confirm New Password")]
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmNewPassword { get; set; }
    }
}
