﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PulseFit.Management.Web.Data.Entities
{
    public class Subscription : IEntity
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        [Range(0, double.MaxValue, ErrorMessage = "The field {0} must be greater than or equal to {1}.")]
        public decimal Price { get; set; }

        public int MaxWorkouts { get; set; }

        // Propriedades para definir a duração personalizada da subscrição
        [Required]
        public int DurationValue { get; set; } = 1; // Ex: 1, 7, 30, etc.

        [Required]
        public DurationType DurationType { get; set; } = DurationType.Months; // Dias, Semanas, Meses ou Anos

        public SubscriptionType SubscriptionType { get; set; }

        // Exclusividade, limitando o usuário a uma única subscrição ativa deste tipo
        public bool IsExclusive { get; set; } = false;

        public SubscriptionStatus Status { get; set; } = SubscriptionStatus.Active;

        public ICollection<UserSubscription> UserSubscriptions { get; set; } = new List<UserSubscription>();

        // Benefícios específicos
        public bool IsAllGymsAccessible { get; set; } = false;
        public List<Gym> IncludedGyms { get; set; } = new List<Gym>();
        public List<Workout> IncludedWorkouts { get; set; } = new List<Workout>();
        public int MaxPersonalTrainerSessions { get; set; } = 0;
        public List<NutritionPlan> IncludedNutritionPlans { get; set; } = new List<NutritionPlan>();
        public List<OnlineClass> IncludedOnlineClasses { get; set; } = new List<OnlineClass>();

        public bool Has24HourAccess { get; set; } = false;
        public bool HasVIPAccess { get; set; } = false;
        public int PerformanceReportFrequencyInMonths { get; set; } = 0;
        
        [Range(0, 100, ErrorMessage = "Discount must be between 0 and 100.")]
        public int DiscountPercentage { get; set; } = 0;

        [NotMapped]
        public decimal CalculatedPrice => Price * (1 - DiscountPercentage / 100m);

        // Propriedades de Imagem
        public Guid ImageId { get; set; } = Guid.Empty;

        [NotMapped]
        public string ImageUrl => ImageId == Guid.Empty
            ? "/images/noimage.png"
            : $"/uploads/subscription-images/{ImageId}.jpg";
    }

    public enum SubscriptionStatus
    {
        Active,
        Inactive,
        Expired,
        Pending
    }

    public enum SubscriptionType
    {
        General,
        Monthly,
        Annual,
        Individual,
        OnlineClasses
    }

    // Enum para definir o tipo de duração (dias, semanas, meses ou anos)
    public enum DurationType
    {
        Days,
        Weeks,
        Months,
        Years
    }
}
