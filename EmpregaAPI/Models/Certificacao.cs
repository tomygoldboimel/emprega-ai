using EmpregaAPI.Models;
using System.ComponentModel.DataAnnotations;

namespace EmpregaAI.Models
{
    public class Certificacao
    {
        [Key]
        [Required]
        public Guid Id { get; set; }

        [Required]
        public Guid CurriculoId { get; set; } // FK

        public string? Nome { get; set; }
        public string? Instituicao { get; set; }
        public DateTime? DataConclusao { get; set; }
        public int? CargaHoraria { get; set; }
        public string? UrlCertificado { get; set; }
        public bool? Excluido { get; set; }

        // Navigation
        public Curriculo? Curriculo { get; set; }
    }
}
