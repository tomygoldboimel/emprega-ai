using EmpregaAPI.Models;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace EmpregaAI.Models
{
    public class Experiencia
    {
        [Key]
        [Required]
        public Guid Id { get; set; }

        [Required]
        public Guid CurriculoId { get; set; }

        public string? Empresa { get; set; }
        public string? Cargo { get; set; }
        public DateTime? DataInicio { get; set; }
        public DateTime? DataFim { get; set; }
        public bool EmpregoAtual { get; set; }
        public string? Descricao { get; set; }
        public bool? Excluido { get; set; }
        [JsonIgnore]
        public Curriculo? Curriculo { get; set; }
    }
}
