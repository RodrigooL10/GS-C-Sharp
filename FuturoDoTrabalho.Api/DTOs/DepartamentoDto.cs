using System.ComponentModel.DataAnnotations;

namespace FuturoDoTrabalho.Api.DTOs
{
    public class DepartamentoCreateDto
    {
        [Required]
        [StringLength(100, MinimumLength = 3)]
        public string Nome { get; set; }

        [StringLength(500)]
        public string? Descricao { get; set; }

        [Required]
        [StringLength(150)]
        public string Lider { get; set; }

        public bool Ativo { get; set; } = true;
    }

    public class DepartamentoUpdateDto
    {
        [Required]
        [StringLength(100, MinimumLength = 3)]
        public string Nome { get; set; }

        [StringLength(500)]
        public string? Descricao { get; set; }

        [Required]
        [StringLength(150)]
        public string Lider { get; set; }

        public bool Ativo { get; set; }
    }

    public class DepartamentoPatchDto
    {
        [StringLength(100, MinimumLength = 3)]
        public string? Nome { get; set; }

        [StringLength(500)]
        public string? Descricao { get; set; }

        [StringLength(150)]
        public string? Lider { get; set; }

        public bool? Ativo { get; set; }
    }

    public class DepartamentoReadDto
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string? Descricao { get; set; }
        public string Lider { get; set; }
        public bool Ativo { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime? DataAtualizacao { get; set; }
    }
}
