using MaximaBackend.Dto;
using MaximaBackend.Models;
using MaximaBackend.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace MaximaBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class ProdutoController : ControllerBase
    {
        private readonly IProdutoInterface _produtoService;
        private readonly ILogger<ProdutoController> _logger;

        public ProdutoController(IProdutoInterface produtoService, ILogger<ProdutoController> logger)
        {
            _produtoService = produtoService;
            _logger = logger;
        }

        /// <summary>
        /// Obter todos os produtos com descrição do departamento
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<DtoProdutoComDepartamento>), 200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<IEnumerable<DtoProdutoComDepartamento>>> Get()
        {
            try
            {
                _logger.LogInformation("Buscando todos os produtos");
                var produtos = await _produtoService.BuscarProdutosComDepartamento();
                return Ok(produtos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Erro interno do servidor" });
            }
        }

        /// <summary>
        /// Obter produto por ID com descrição do departamento
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(DtoProdutoComDepartamento), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<DtoProdutoComDepartamento>> GetById(Guid id)
        {
            try
            {
                _logger.LogInformation("Buscando produto com ID: {Id}", id);
                var produto = await _produtoService.BuscarProdutoComDepartamentoPorId(id);

                if (produto == null)
                    return NotFound();

                return Ok(produto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Erro interno do servidor" });
            }
        }

        /// <summary>
        /// Metodo post para cadastrar
        /// </summary>
        /// <param name="produtoDto"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(Produto), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(409)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<Produto>> Post([FromBody] ProdutoCreateDto produtoDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                Console.WriteLine($"Recebido: {JsonSerializer.Serialize(produtoDto)}");

                // Verificar se o código já existe
                var codigoExiste = await _produtoService.CodigoProdutoExiste(produtoDto.Codigo);
                if (codigoExiste)
                {
                    return Conflict(new { mensagem = "Código do produto já existe" });
                }

                _logger.LogInformation("Criando novo produto com código: {Codigo}", produtoDto.Codigo);
                var produto = await _produtoService.CriarProduto(produtoDto);

                return CreatedAtAction(nameof(Get), new { id = produto.Id }, produto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao criar produto");
                return StatusCode(500, new { mensagem = "Erro interno do servidor" });
            }
        }

        /// <summary>
        /// Atualiza um produto existente
        /// </summary>
        /// <param name="id">ID do produto</param>
        /// <param name="produtoDto">Dados atualizados do produto</param>
        /// <returns>Produto atualizado</returns>
        /// <response code="200">Produto atualizado com sucesso</response>
        /// <response code="400">Dados inválidos ou ID inconsistente</response>
        /// <response code="404">Produto não encontrado</response>
        /// <response code="409">Código do produto já existe</response>
        /// <response code="500">Erro interno do servidor</response>
        [HttpPut("{id:guid}")]
        [ProducesResponseType(typeof(Produto), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(409)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<Produto>> Put(Guid id, [FromBody] ProdutoUpdateDto produtoDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                // Verificar se o código já existe para outro produto
                var codigoExiste = await _produtoService.CodigoProdutoExiste(produtoDto.Codigo, id);
                if (codigoExiste)
                {
                    return Conflict(new { mensagem = "Código do produto já existe" });
                }

                _logger.LogInformation("Atualizando produto {Id}", id);
                var produto = await _produtoService.AtualizarProduto(id, produtoDto);

                if (produto == null)
                {
                    return NotFound(new { mensagem = "Produto não encontrado" });
                }

                return Ok(produto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao atualizar produto {Id}", id);
                return StatusCode(500, new { message = "Erro interno do servidor" });
            }
        }

        /// <summary>
        /// Colocar produto como inativo
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id:guid}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                if (id == Guid.Empty)
                {
                    return BadRequest(new { mensagem = "ID inválido" });
                }

                _logger.LogInformation("Excluindo produto {Id}", id);
                var sucesso = await _produtoService.ExcluirProduto(id);

                if (!sucesso)
                {
                    return NotFound(new { mensagem = "Produto não encontrado" });
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro excluindo produto {Id}", id);
                return StatusCode(500, new { mensagem = "Erro interno do servidor" });
            }
        }
    }
}
