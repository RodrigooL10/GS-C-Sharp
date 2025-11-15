using Microsoft.EntityFrameworkCore;
using FuturoDoTrabalho.Api.Models;

namespace FuturoDoTrabalho.Api.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Trabalhador> Trabalhadores { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configurações adicionais do Trabalhador
            modelBuilder.Entity<Trabalhador>()
                .Property(t => t.Salario)
                .HasPrecision(10, 2);

            modelBuilder.Entity<Trabalhador>()
                .Property(t => t.DataCriacao)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            // Índices para melhor performance
            modelBuilder.Entity<Trabalhador>()
                .HasIndex(t => t.CPF)
                .IsUnique();

            modelBuilder.Entity<Trabalhador>()
                .HasIndex(t => t.Email);

            modelBuilder.Entity<Trabalhador>()
                .HasIndex(t => t.Ativo);
        }
    }
}
