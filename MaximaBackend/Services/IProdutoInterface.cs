using MaximaBackend.Dto;
using MaximaBackend.Models;

namespace MaximaBackend.Services
{
    public interface IProdutoInterface
    {
        Task<IEnumerable<DtoProdutoComDepartamento>> BuscarProdutosComDepartamento();
        Task<DtoProdutoComDepartamento> BuscarProdutoComDepartamentoPorId(Guid id);
        Task<IEnumerable<Produto>> ObterTodosProdutos();
        Task<Produto?> ObterPorId(Guid id);
        Task<Produto> CriarProduto(ProdutoCreateDto produto);
        Task<Produto?> AtualizarProduto(Guid id, ProdutoUpdateDto produto);
        Task<bool> ExcluirProduto(Guid id);
        Task<bool> CodigoProdutoExiste(string codigo, Guid? excludeId = null);

    }
}