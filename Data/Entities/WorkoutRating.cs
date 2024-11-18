namespace PulseFit.Management.Web.Data.Entities
{
    public class WorkoutRating
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public virtual User User { get; set; } // Relacionamento com a entidade User
        public int WorkoutId { get; set; } // Workout sendo avaliado
        public int RatingValue { get; set; } // Valor da classificação (1 a 5 estrelas)
        public string Comment { get; set; } // Comentário opcional
        public DateTime DateCreated { get; set; }
    }
}
