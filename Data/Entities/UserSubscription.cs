using System;
using System.ComponentModel.DataAnnotations;

namespace PulseFit.Management.Web.Data.Entities
{
    public class UserSubscription : IEntity
    {
        public int Id { get; set; }

        // Relação com User
        public string UserId { get; set; }

        public User User { get; set; }

        // Relação com Subscription
        public int SubscriptionId { get; set; }

        public Subscription Subscription { get; set; }

        // Relação com Client
        public int ClientId { get; set; }  // Adiciona a chave estrangeira para Client
        public Client Client { get; set; }  // Adiciona a propriedade de navegação para Client

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public SubscriptionStatus Status { get; set; }
    }
}