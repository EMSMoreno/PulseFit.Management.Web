using PulseFit.Management.Web.Data.Entities;
using System.Collections.Generic;

namespace PulseFit.Management.Web.ViewModels
{
    public class PaymentViewModel
    {
        public int SubscriptionId { get; set; }
        public decimal Amount { get; set; }
        public string UserId { get; set; }
        public Payment.PaymentMethod PaymentMethodSelected { get; set; }

        // Lista de assinaturas disponíveis para o dropdown na view
        public List<Subscription> AvailableSubscriptions { get; set; }
    }
}
