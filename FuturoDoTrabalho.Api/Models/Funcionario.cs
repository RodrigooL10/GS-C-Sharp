using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FuturoDoTrabalho.Api.Models
{
    [Table("funcionarios")]
    public class Funcionario
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(150)]
        public string Nome { get; set; }

        [Required]
        [StringLength(100)]
        public string Cargo { get; set; }

        [StringLength(11)]
        public string? CPF { get; set; }

        [EmailAddress]
        [StringLength(150)]
        public string? Email { get; set; }

        [StringLength(20)]
        public string? Telefone { get; set; }

        [Required]
        public DateTime DataAdmissao { get; set; }

        [Required]
        public int DepartamentoId { get; set; }

        [ForeignKey("DepartamentoId")]
        public Departamento? Departamento { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        [Range(0.01, 9999999999.99)]
        public decimal Salario { get; set; }

        [StringLength(500)]
        public string? Endereco { get; set; }

        [Range(1, 5)]
        public int NivelSenioridade { get; set; } = 1; // 1: Júnior, 2: Pleno, 3: Sênior, 4: Especialista, 5: Arquiteto

        public bool Ativo { get; set; } = true;

        public DateTime DataCriacao { get; set; } = DateTime.UtcNow;

        public DateTime? DataAtualizacao { get; set; }
    }
}
