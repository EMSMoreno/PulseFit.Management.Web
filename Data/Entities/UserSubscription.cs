using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PulseFit.Management.Web.Data.Entities
{
    public class UserSubscription : IEntity
    {
        public int Id { get; set; }

        // Relação com User
        [Required]
        public string UserId { get; set; }
        public User User { get; set; }

        // Relação com Subscription
        [Required]
        public int SubscriptionId { get; set; }
        public Subscription Subscription { get; set; }

        public bool IsPaid { get; set; }

        // Relação com Client
        [Required]
        public int ClientId { get; set; }
        public Client Client { get; set; }

        // Data de início da subscrição
        public DateTime StartDate { get; set; }

        // Propriedades de pagamento
        public string TransactionId { get; set; }
        public decimal AmountPaid { get; set; }

        // Data de término calculada dinamicamente com base na Subscription
        public DateTime EndDate { get; set; }

        public SubscriptionStatus Status { get; set; }

        // Calcula a data de término com base na duração da Subscription
        public void CalculateEndDate()
        {
            DateTime endDate = StartDate;

            switch (Subscription.DurationType)
            {
                case DurationType.Days:
                    endDate = endDate.AddDays(Subscription.DurationValue);
                    break;
                case DurationType.Weeks:
                    endDate = endDate.AddDays(Subscription.DurationValue * 7);
                    break;
                case DurationType.Months:
                    endDate = endDate.AddMonths(Subscription.DurationValue);
                    break;
                case DurationType.Years:
                    endDate = endDate.AddYears(Subscription.DurationValue);
                    break;
            }

            EndDate = endDate;
        }
    }
}
