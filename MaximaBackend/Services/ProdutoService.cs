using MaximaBackend.Dto;
using MaximaBackend.Models;
using System.Data;
using Dapper;

namespace MaximaBackend.Services
{
    public class ProdutoService : IProdutoInterface
    {
        private readonly IDbConnection _connection;

        public ProdutoService(IDbConnection connection)
        {
            _connection = connection;
        }

        /// <summary>
        /// Obter todos os produtos com descrição do departamento
        /// </summary>
        public async Task<IEnumerable<DtoProdutoComDepartamento>> BuscarProdutosComDepartamento()
        {
            const string sql = @"
                SELECT 
                    p.id, 
                    p.codigo, 
                    p.descricao, 
                    p.preco, 
                    p.status, 
                    p.departamento as DepartamentoCodigo,
                    d.descricao as DepartamentoDescricao
                FROM produtos p
                LEFT JOIN departamentos d ON p.departamento = d.codigo
                ORDER BY p.codigo";

            return await _connection.QueryAsync<DtoProdutoComDepartamento>(sql);
        }

        /// <summary>
        /// Obter produto por ID com descrição do departamento
        /// </summary>
        public async Task<DtoProdutoComDepartamento> BuscarProdutoComDepartamentoPorId(Guid id)
        {
            const string sql = @"
                SELECT 
                    p.id, 
                    p.codigo, 
                    p.descricao, 
                    p.preco, 
                    p.status, 
                    p.departamento as DepartamentoCodigo,
                    d.descricao as DepartamentoDescricao
                FROM produtos p
                LEFT JOIN departamentos d ON p.departamento = d.codigo
                WHERE p.id = @Id";

            return await _connection.QueryFirstOrDefaultAsync<DtoProdutoComDepartamento>(sql, new { Id = id });
        }

        /// <summary>
        /// Método para obter todos produtos
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<Produto>> ObterTodosProdutos()
        {
            const string sql = @"
                SELECT id, codigo, descricao, departamento, preco, status
                FROM produtos 
                ORDER BY CODIGO";

            return await _connection.QueryAsync<Produto>(sql);
        }

        /// <summary>
        /// Método para obter produto por ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Produto?> ObterPorId(Guid id)
        {
            const string sql = @"
                SELECT id, codigo, descricao, departamento, preco, status
                FROM produtos 
                WHERE id = @Id ";

            return await _connection.QueryFirstOrDefaultAsync<Produto>(sql, new { Id = id });
        }

        /// <summary>
        /// Método para criar produto
        /// </summary>
        /// <param name="produtoDto"></param>
        /// <returns></returns>
        public async Task<Produto> CriarProduto(ProdutoCreateDto produtoDto)
        {
            var produto = new Produto
            {
                Id = Guid.NewGuid(),
                Codigo = produtoDto.Codigo,
                Descricao = produtoDto.Descricao,
                Departamento = produtoDto.DepartamentoCodigo,
                Preco = produtoDto.Preco,
                Status = true
            };

            string sql = @"INSERT INTO produtos (id, codigo, descricao, departamento, preco, status) 
            VALUES (@Id, @Codigo, @Descricao, @Departamento, @Preco, @Status)";

            await _connection.ExecuteAsync(sql, produto);
            return produto;
        }

        /// <summary>
        /// Método para atualizar produto com base no ID
        /// </summary>
        /// <param name="produtoDto"></param>
        /// <returns></returns>
        public async Task<Produto?> AtualizarProduto(Guid id, ProdutoUpdateDto produtoDto)
        {
            const string sql = @"
                UPDATE produtos 
                SET codigo = @Codigo, 
                    descricao = @Descricao, 
                    departamento = @DepartamentoCodigo, 
                    preco = @Preco, 
                    status = @Status
                WHERE id = @Id";

            var rowsAffected = await _connection.ExecuteAsync(sql, new
            {
                Codigo=produtoDto.Codigo,
                Descricao = produtoDto.Descricao,
                DepartamentoCodigo=produtoDto.DepartamentoCodigo,
                Preco = produtoDto.Preco,
                Status = produtoDto.Status,
                Id=id
            });

            if (rowsAffected > 0)
            {
                return await ObterPorId(id);
            }

            return null;
        }

        /// <summary>
        /// Método para colocar status como inativo - produto excluido
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> ExcluirProduto(Guid id)
        {
            const string sql = @"
                UPDATE produtos 
                SET status = False WHERE id = @Id ";

            var rowsAffected = await _connection.ExecuteAsync(sql, new
            {
                Id = id
            });

            return rowsAffected > 0;
        }

        /// <summary>
        /// Método para verificar se preoduto existe
        /// </summary>
        /// <param name="codigo"></param>
        /// <param name="excludeId"></param>
        /// <returns></returns>
        public async Task<bool> CodigoProdutoExiste(string codigo, Guid? excludeId = null)
        {
            string sql = "SELECT COUNT(1) FROM produtos WHERE codigo = '@Codigo'";
            object parameters = new { Codigo = codigo };

            if (excludeId.HasValue)
            {
                sql += " AND id != @ExcludeId";
                parameters = new { Codigo = codigo, ExcludeId = excludeId.Value };
            }

            var count = await _connection.QuerySingleAsync<int>(sql, parameters);
            return count > 0;
        }
    }
}