using PulseFit.Management.Web.Data.Entities;
using System.ComponentModel.DataAnnotations;

namespace PulseFit.Management.Web.Models
{
    public class OnlineClassViewModel : OnlineClass
    {
        [Display(Name = "Class Picture")]
        public IFormFile? ClassImageFile { get; set; }
    }
}
