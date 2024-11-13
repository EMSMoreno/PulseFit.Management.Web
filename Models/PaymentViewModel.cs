using System.ComponentModel.DataAnnotations;
using PulseFit.Management.Web.Data.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace PulseFit.Management.Web.Models
{
    public class PaymentViewModel
    {
        public int Id { get; set; } // Identificador do pagamento

        public int SubscriptionId { get; set; }
        public string SubscriptionName { get; set; }

        [Required]
        public decimal Amount { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        public Payment.PaymentMethod SelectedMethod { get; set; }

        public string Description { get; set; }

        public List<SelectListItem> PaymentMethodOptions { get; set; } = new List<SelectListItem>();

        public DateTime PaymentDate { get; set; } // Data do pagamento
        public string TransactionId { get; set; } // Identificador da transação

        public Dictionary<string, (string Color, string Icon)> PaymentMethodStyles { get; set; } = new()
{
    { "Credit Card", ("#af4c4c", "credit_card_icon.png") },
    { "PayPal", ("#003087", "paypal_icon.png") },
    { "Cash", ("#FF9800", "cash_icon.png") },
    { "Crypto", ("#4c4caf", "crypto_icon.png") } // Adiciona a cor e o ícone para "Crypto"
};
    }
}
