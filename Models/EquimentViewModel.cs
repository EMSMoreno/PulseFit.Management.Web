using PulseFit.Management.Web.Data.Entities;
using System.ComponentModel.DataAnnotations;

namespace PulseFit.Management.Web.Models
{
    public class EquimentViewModel : Equipment
    {
        [Display(Name = "Equipment Picture")]
        public IFormFile? EquipmentImageFile { get; set; }
    }
}
