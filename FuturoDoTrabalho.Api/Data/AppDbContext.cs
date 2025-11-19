using Microsoft.EntityFrameworkCore;
using FuturoDoTrabalho.Api.Models;

namespace FuturoDoTrabalho.Api.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // Tabelas GD Solutions
        public DbSet<Funcionario> Funcionarios { get; set; }
        public DbSet<Departamento> Departamentos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Funcionario
            modelBuilder.Entity<Funcionario>()
                .Property(f => f.Salario)
                .HasPrecision(10, 2);

            modelBuilder.Entity<Funcionario>()
                .Property(f => f.DataCriacao)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            modelBuilder.Entity<Funcionario>()
                .HasIndex(f => f.CPF)
                .IsUnique();

            modelBuilder.Entity<Funcionario>()
                .HasIndex(f => f.Email);

            modelBuilder.Entity<Funcionario>()
                .HasIndex(f => f.DepartamentoId);

            modelBuilder.Entity<Funcionario>()
                .HasIndex(f => f.Ativo);

            // Relacionamento Funcionario -> Departamento
            modelBuilder.Entity<Funcionario>()
                .HasOne(f => f.Departamento)
                .WithMany(d => d.Funcionarios)
                .HasForeignKey(f => f.DepartamentoId)
                .OnDelete(DeleteBehavior.Restrict);

            // Departamento
            modelBuilder.Entity<Departamento>()
                .Property(d => d.DataCriacao)
                .HasDefaultValue(DateTime.UtcNow);

            modelBuilder.Entity<Departamento>()
                .HasIndex(d => d.Nome)
                .IsUnique();
        }
    }
}
