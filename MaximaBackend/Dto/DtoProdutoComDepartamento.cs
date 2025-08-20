using System.ComponentModel.DataAnnotations;

namespace MaximaBackend.Dto
{
    public class DtoProdutoComDepartamento
    {

        public Guid Id { get; set; }

        [Required(ErrorMessage = "Código é obrigatório")]
        [StringLength(50, ErrorMessage = "Código deve ter no máximo 50 caracteres")]
        public string Codigo { get; set; }

        [Required(ErrorMessage = "Descrição é obrigatória")]
        [StringLength(500, ErrorMessage = "Descrição deve ter no máximo 500 caracteres")]
        public string Descricao { get; set; }
        public decimal Preco { get; set; }
        public bool Status { get; set; }
        public string DepartamentoCodigo { get; set; }     
        public string DepartamentoDescricao { get; set; }  
    }
}
