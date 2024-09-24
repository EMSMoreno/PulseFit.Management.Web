namespace PulseFit.Management.Web.Data.Entities
{
    public class ClientPreference : IEntity
    {
        public int Id { get; set; }

        public int ClientId { get; set; }

        public User Client { get; set; }

        public string DietaryRestrictions { get; set; }

        public string HealthConditions { get; set; }

        public int CaloricGoal { get; set; }

        public string Notes { get; set; }

        public int NutritionistId { get; set; }

        public Nutritionist Nutritionist { get; set; }
    }
}
