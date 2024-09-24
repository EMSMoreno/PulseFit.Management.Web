using System.ComponentModel.DataAnnotations;

namespace PulseFit.Management.Web.Data.Entities
{
    public class Report : IEntity
    {
        public int Id { get; set; }

        public string ReportType { get; set; } // Ex: Financial, Usage

        public DateTime GenerationDate { get; set; }

        public string Description { get; set; }

        public string Data { get; set; } // Armazena o conteúdo do relatório

        // Identificador do utilizador. É obrigatório e serve como chave estrangeira.
        [Required]
        public string UserId { get; set; }

        // Navegação para a entidade `User`. Representa o utilizador.
        public User User { get; set; }
    }
}
