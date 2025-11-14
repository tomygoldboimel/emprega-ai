using EmpregaAPI.Models;
using System.ComponentModel.DataAnnotations;

namespace EmpregaAI.Models
{
    public class Formacao
    {
        [Key]
        [Required]
        public Guid Id { get; set; }

        [Required]
        public Guid CurriculoId { get; set; } // FK

        public string? Instituicao { get; set; }
        public string? Curso { get; set; }
        public string? Nivel { get; set; } // Técnico, Graduação, Pós, etc
        public string? Status { get; set; } // Cursando, Concluído, Trancado
        public DateTime? DataInicio { get; set; }
        public DateTime? DataConclusao { get; set; }
        public bool? Excluido { get; set; }

        // Navigation
        public Curriculo? Curriculo { get; set; }
    }
}
