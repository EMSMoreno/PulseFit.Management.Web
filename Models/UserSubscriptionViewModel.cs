using Microsoft.AspNetCore.Mvc.Rendering;
using PulseFit.Management.Web.Data.Entities;
using System.ComponentModel.DataAnnotations;

namespace PulseFit.Management.Web.Models
{
    public class UserSubscriptionViewModel
    {
        public int Id { get; set; }

        [Required]
        public int SubscriptionId { get; set; }
        public SubscriptionViewModel Subscription { get; set; }

        [Required]
        public int ClientId { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public SubscriptionStatus Status { get; set; }

        public bool IsActive => Status == SubscriptionStatus.Active && EndDate >= DateTime.UtcNow;

        // Lista de opções de assinaturas
        public IEnumerable<SelectListItem> SubscriptionOptions { get; set; }
    }
}
