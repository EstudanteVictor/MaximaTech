using System.ComponentModel.DataAnnotations;

namespace MaximaBackend.Models
{
    public class Produto
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required(ErrorMessage = "Código é obrigatório")]
        [StringLength(50, ErrorMessage = "Código deve ter no máximo 50 caracteres")]
        public string Codigo { get; set; } = string.Empty;

        [Required(ErrorMessage = "Descrição é obrigatória")]
        [StringLength(500, ErrorMessage = "Descrição deve ter no máximo 500 caracteres")]
        public string Descricao { get; set; } = string.Empty;

        [Required(ErrorMessage = "Departamento é obrigatório")]
        public string Departamento { get; set; } = string.Empty;

        [Range(0.01, double.MaxValue, ErrorMessage = "Preço deve ser maior que zero")]
        public decimal Preco { get; set; }

        public bool Status { get; set; } = true;
    }
}