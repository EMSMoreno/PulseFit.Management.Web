using System.ComponentModel.DataAnnotations;

namespace PulseFit.Management.Web.Models
{
    public class AlertViewModel
    {
        [Required(ErrorMessage = "Please enter a message for the alert.")]
        [MaxLength(500, ErrorMessage = "Message cannot exceed 500 characters.")]
        public string Message { get; set; }
    }
}
