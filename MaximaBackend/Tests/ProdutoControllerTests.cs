using Xunit;
using Microsoft.AspNetCore.Mvc;
using Moq;
using MaximaBackend.Controllers;
using MaximaBackend.Services;
using MaximaBackend.Models;
using MaximaBackend.Dto;


namespace MaximaBackend.Tests
{
    public class ProdutoControllerTests
    {
        private readonly Mock<IProdutoInterface> _mockProdutoService;
        private readonly Mock<ILogger<ProdutoController>> _mockLogger;
        private readonly ProdutoController _controller;

        public ProdutoControllerTests()
        {
            _mockProdutoService = new Mock<IProdutoInterface>();
            _mockLogger = new Mock<ILogger<ProdutoController>>();
            _controller = new ProdutoController(_mockProdutoService.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task Get_DeveRetornarListaDeProdutos()
        {
            var produtos = new List<Produto>
            {
                new Produto { Id = Guid.NewGuid(), Codigo = "001", Descricao = "Produto 1", Departamento = "010", Preco = 10.0m, Status = true },
                new Produto { Id = Guid.NewGuid(), Codigo = "002", Descricao = "Produto 2", Departamento = "020", Preco = 20.0m, Status = true }
            };

            _mockProdutoService.Setup(x => x.ObterTodosProdutos()).ReturnsAsync(produtos);

            var resultado = await _controller.Get();

            var okResult = Assert.IsType<OkObjectResult>(resultado.Result);
            var retornoProdutos = Assert.IsAssignableFrom<IEnumerable<Produto>>(okResult.Value);
            Assert.Equal(2, ((List<Produto>)retornoProdutos).Count);
        }

        [Fact]
        public async Task Post_ComDadosValidos_DeveCriarProduto()
        {
            // Arrange
            var produtoDto = new ProdutoCreateDto
            {
                Codigo = "TEST001",
                Descricao = "Produto Teste",
                DepartamentoCodigo = "010", 
                Preco = 15.99m,
                Status = true
            };

            var produtoCriado = new Produto
            {
                Id = Guid.NewGuid(),
                Codigo = produtoDto.Codigo,
                Descricao = produtoDto.Descricao,
                Departamento = produtoDto.DepartamentoCodigo,
                Preco = produtoDto.Preco,
                Status = produtoDto.Status
            };

            _mockProdutoService.Setup(x => x.CodigoProdutoExiste(produtoDto.Codigo, null)).ReturnsAsync(false);
            _mockProdutoService.Setup(x => x.CriarProduto(produtoDto)).ReturnsAsync(produtoCriado);

            var resultado = await _controller.Post(produtoDto);

            var createdResult = Assert.IsType<CreatedAtActionResult>(resultado.Result);
            var produto = Assert.IsType<Produto>(createdResult.Value);
            Assert.Equal(produtoDto.Codigo, produto.Codigo);
        }

        [Fact]
        public async Task Post_ComCodigoExistente_DeveRetornarConflict()
        {
            var produtoDto = new ProdutoCreateDto
            {
                Codigo = "EXIST001",
                Descricao = "Produto Teste",
                DepartamentoCodigo = "010", 
                Preco = 15.99m,
                Status = true
            };

            _mockProdutoService.Setup(x => x.CodigoProdutoExiste(produtoDto.Codigo, null)).ReturnsAsync(true);

            var resultado = await _controller.Post(produtoDto);

            var conflictResult = Assert.IsType<ConflictObjectResult>(resultado.Result);
            Assert.Equal("Código do produto já existe", conflictResult.Value);
        }

        [Fact]
        public async Task GetById_DeveRetornarProdutoQuandoExistir()
        {
            // Arrange
            var id = Guid.NewGuid();
            var produto = new Produto
            {
                Id = id,
                Codigo = "TEST001",
                Descricao = "Produto Teste",
                Departamento = "010",
                Preco = 10.50m,
                Status = true
            };

            _mockProdutoService.Setup(x => x.ObterPorId(id)).ReturnsAsync(produto);

            var resultado = await _controller.GetById(id);

            var okResult = Assert.IsType<OkObjectResult>(resultado.Result);
            var produtoRetornado = Assert.IsType<Produto>(okResult.Value);
            Assert.Equal(id, produtoRetornado.Id);
        }

        [Fact]
        public async Task GetById_DeveRetornarNotFoundQuandoNaoExistir()
        {
            var id = Guid.NewGuid();

            _mockProdutoService.Setup(x => x.ObterPorId(id)).ReturnsAsync((Produto)null);

            var resultado = await _controller.GetById(id);

            Assert.IsType<NotFoundResult>(resultado.Result);
        }
    }
}