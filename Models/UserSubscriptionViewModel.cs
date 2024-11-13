using Microsoft.AspNetCore.Mvc.Rendering;
using PulseFit.Management.Web.Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PulseFit.Management.Web.Models
{
    public class UserSubscriptionViewModel
    {
        public int Id { get; set; }

        // ID e detalhes da assinatura
        [Required]
        public int SubscriptionId { get; set; }
        public SubscriptionViewModel Subscription { get; set; }

        // ID e detalhes do cliente
        [Required]
        public int ClientId { get; set; }
        public ClientViewModel Client { get; set; } // Pode ser adicionado para representar o cliente

        // Datas de início e término
        [Required]
        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        // Estado da assinatura e pagamento
        public SubscriptionStatus Status { get; set; }
        public bool IsPaid { get; set; }

        // Propriedades de pagamento
        public string TransactionId { get; set; }
        public decimal AmountPaid { get; set; }

        // Verificação se a assinatura está ativa
        public bool IsActive => Status == SubscriptionStatus.Active && EndDate >= DateTime.UtcNow;

        // Propriedades auxiliares para exibição
        public string SubscriptionName => Subscription?.Name;
        public decimal SubscriptionPrice => Subscription?.Price ?? 0; // Acesso ao preço da subscrição


        public IEnumerable<SelectListItem> SubscriptionOptions { get; set; }

        // Métodos auxiliares
        public void CalculateEndDate()
        {
            DateTime calculatedEndDate = StartDate;
            if (Subscription != null)
            {
                switch (Subscription.DurationType)
                {
                    case DurationType.Days:
                        calculatedEndDate = calculatedEndDate.AddDays(Subscription.DurationValue);
                        break;
                    case DurationType.Weeks:
                        calculatedEndDate = calculatedEndDate.AddDays(Subscription.DurationValue * 7);
                        break;
                    case DurationType.Months:
                        calculatedEndDate = calculatedEndDate.AddMonths(Subscription.DurationValue);
                        break;
                    case DurationType.Years:
                        calculatedEndDate = calculatedEndDate.AddYears(Subscription.DurationValue);
                        break;
                }
            }
            EndDate = calculatedEndDate;
        }

        public List<PaymentViewModel> Payments { get; set; } = new List<PaymentViewModel>();

    }
}
