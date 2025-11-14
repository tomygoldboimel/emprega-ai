using System.ComponentModel.DataAnnotations;

namespace EmpregaAPI.Models
{
    public class Usuario
    {
        [Key]
        [Required]
        public Guid Id { get; set; }
        public string? Nome { get; set; }
        public string? Email { get; set; }
        public string? Senha { get; set; }
        public bool? Ativo { get; set; }
        public bool? Excluido { get; set; }
    }
}