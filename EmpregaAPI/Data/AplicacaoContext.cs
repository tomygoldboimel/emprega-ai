using Microsoft.EntityFrameworkCore;
using EmpregaAPI.Models;
using EmpregaAI.Models;

namespace EmpregaAPI.Data
{
    public class AplicacaoContext : DbContext
    {
        public AplicacaoContext(DbContextOptions<AplicacaoContext> options)
            : base(options)
        {
        }

        public DbSet<Curriculo> Curriculos { get; set; } = null!;
        public DbSet<Usuario> Usuarios { get; set; } = null!;
        public DbSet<Experiencia> Experiencias { get; set; } = null!;
        public DbSet<Formacao> Formacoes { get; set; } = null!;
        public DbSet<Certificacao> Certificacoes { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configurações adicionais (opcional)

            // Exemplo: definir nome de tabela
            // modelBuilder.Entity<Curriculo>().ToTable("Curriculos");

            // Exemplo: definir chave primária
            // modelBuilder.Entity<Curriculo>().HasKey(c => c.Id);

            // Exemplo: definir propriedade obrigatória
            // modelBuilder.Entity<Curriculo>()
            //     .Property(c => c.Nome)
            //     .IsRequired()
            //     .HasMaxLength(100);

            // Exemplo: relacionamento entre Curriculo e Usuario
            // modelBuilder.Entity<Curriculo>()
            //     .HasOne(c => c.Usuario)
            //     .WithMany(u => u.Curriculos)
            //     .HasForeignKey(c => c.UsuarioId);
        }
    }
}