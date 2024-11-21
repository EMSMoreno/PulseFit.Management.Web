using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using PulseFit.Management.Web.Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PulseFit.Management.Web.Models
{
    public class SubscriptionViewModel
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "The field {0} must be greater than or equal to {1}.")]
        public decimal Price { get; set; }

        public int MaxWorkouts { get; set; }

        // Propriedades para definir a duração personalizada da subscrição
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Duration must be greater than zero.")]
        public int DurationValue { get; set; } = 1; // Ex: 1, 7, 30, etc.

        [Required]
        public DurationType DurationType { get; set; } = DurationType.Months;

        public SubscriptionType SubscriptionType { get; set; }
        public bool IsExclusive { get; set; }

        public SubscriptionStatus Status { get; set; } = SubscriptionStatus.Active;

        // Benefícios e configurações de acesso
        public bool IsAllGymsAccessible { get; set; }
        public List<SelectListItem> GymOptions { get; set; } = new List<SelectListItem>();
        public List<int> SelectedGymIds { get; set; } = new List<int>();

        public int MaxPersonalTrainerSessions { get; set; }
        public List<SelectListItem> WorkoutOptions { get; set; } = new List<SelectListItem>();
        public List<int> SelectedWorkoutIds { get; set; } = new List<int>();

        public List<SelectListItem> NutritionPlanOptions { get; set; } = new List<SelectListItem>();
        public List<int> SelectedNutritionPlanIds { get; set; } = new List<int>();

        public List<SelectListItem> OnlineClassOptions { get; set; } = new List<SelectListItem>();
        public List<int> SelectedOnlineClassIds { get; set; } = new List<int>();

        public bool Has24HourAccess { get; set; }
        public bool HasVIPAccess { get; set; }
        public int PerformanceReportFrequencyInMonths { get; set; }
        public int DiscountPercentage { get; set; }

        public decimal CalculatedPrice => Price * (1 - DiscountPercentage / 100m);

        public bool IncludeNutritionPlans { get; set; }
        public bool IncludeOnlineClasses { get; set; }


        // Propriedades de Imagem
        public Guid ImageId { get; set; }
        public IFormFile? SubscriptionImageFile { get; set; }

        public string ImageUrl => ImageId == Guid.Empty
            ? "/images/noimage.png"
            : $"/uploads/subscription-images/{ImageId}.jpg";
    }
}
