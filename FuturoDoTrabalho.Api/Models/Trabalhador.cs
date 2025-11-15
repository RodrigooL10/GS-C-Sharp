using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FuturoDoTrabalho.Api.Models
{
    [Table("trabalhadores")]
    public class Trabalhador
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

        [StringLength(255)]
        public string Departamento { get; set; }

        [Range(0, double.MaxValue)]
        public decimal Salario { get; set; }

        [DataType(DataType.Date)]
        public DateTime DataAdmissao { get; set; }

        [StringLength(11)]
        public string CPF { get; set; }

        [StringLength(20)]
        public string Telefone { get; set; }

        [EmailAddress]
        [StringLength(150)]
        public string Email { get; set; }

        [StringLength(500)]
        public string Endereco { get; set; }

        public bool Ativo { get; set; } = true;

        public DateTime DataCriacao { get; set; } = DateTime.UtcNow;

        public DateTime? DataAtualizacao { get; set; }
    }
}
