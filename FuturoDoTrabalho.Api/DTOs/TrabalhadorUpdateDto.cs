using System;
using System.ComponentModel.DataAnnotations;

namespace FuturoDoTrabalho.Api.DTOs
{
    public class TrabalhadorUpdateDto
    {
        [StringLength(150, MinimumLength = 3, 
            ErrorMessage = "O nome deve ter entre 3 e 150 caracteres")]
        public string Nome { get; set; }

        [StringLength(100)]
        public string Cargo { get; set; }

        [StringLength(255)]
        public string Departamento { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "O salário deve ser maior que zero")]
        public decimal Salario { get; set; }

        [DataType(DataType.Date)]
        public DateTime? DataAdmissao { get; set; }

        [StringLength(11)]
        public string CPF { get; set; }

        [StringLength(20)]
        [Phone(ErrorMessage = "O telefone deve estar em um formato válido")]
        public string Telefone { get; set; }

        [EmailAddress(ErrorMessage = "O email deve estar em um formato válido")]
        [StringLength(150)]
        public string Email { get; set; }

        [StringLength(500)]
        public string Endereco { get; set; }

        public bool? Ativo { get; set; }
    }
}
