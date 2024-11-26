using System.ComponentModel.DataAnnotations;

namespace PulseFit.Management.Web.Models
{
    public class ChangePasswordViewModel
    {
        // The current password is required
        [Required]
        [Display(Name = "Current password")]
        // Field to input the current password
        public string OldPassword { get; set; }

        // The new password is required
        [Required]
        [Display(Name = "New password")]
        // Field to input the new password
        public string NewPassword { get; set; }

        // Confirmation field to ensure the user inputs the new password twice
        [Required]
        // Ensures the value entered in this field matches the value of "NewPassword"
        [Compare("NewPassword", ErrorMessage = "The confirmation password does not match the new password.")]
        public string Confirm { get; set; }
    }
}
