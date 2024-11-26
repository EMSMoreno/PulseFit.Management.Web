using System.ComponentModel.DataAnnotations;

namespace PulseFit.Management.Web.Models
{
    public class ResetPasswordViewModel
    {
        // The [Required] attribute indicates that this field is mandatory.
        // If the field is empty, an error message will be displayed.
        [Required(ErrorMessage = "Username is required.")]
        public string UserName { get; set; }

        // The [Required] attribute indicates that this field is mandatory.
        // The [DataType(DataType.Password)] attribute configures the field to be treated as a password,
        // hiding the text entered in the input field.
        [Required(ErrorMessage = "Password is required.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        // The [Required] attribute indicates that this field is mandatory.
        // The [DataType(DataType.Password)] attribute configures the field to be treated as a password.
        // The [Compare("Password")] attribute validates that the value entered in ConfirmPassword
        // matches the main password (Password).
        [Required(ErrorMessage = "Password confirmation is required.")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords do not match.")]
        public string ConfirmPassword { get; set; }

        // The [Required] attribute indicates that this field is mandatory.
        // This field stores the password reset token, which is necessary to validate the reset request.
        [Required(ErrorMessage = "Token is required.")]
        public string Token { get; set; }
    }
}
