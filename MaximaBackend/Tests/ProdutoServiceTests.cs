using Xunit;
using Moq;
using System.Data;
using MaximaBackend.Services;
using MaximaBackend.Models;
using MaximaBackend.Dto;
using Dapper;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace MaximaBackend.Tests
{
    public class ProdutoServiceTests
    {
        private readonly Mock<IDbConnection> _mockConnection;
        private readonly Mock<ILogger<ProdutoService>> _mockLogger;
        private readonly ProdutoService _produtoService;

        public ProdutoServiceTests()
        {
            _mockConnection = new Mock<IDbConnection>();
            _mockLogger = new Mock<ILogger<ProdutoService>>();
            _produtoService = new ProdutoService(_mockConnection.Object);
        }

        [Fact]
        public async Task CriarProduto_DeveCriarProdutoComSucesso()
        {
            var produtoDto = new ProdutoCreateDto
            {
                Codigo = "TEST001",
                Descricao = "Produto Teste",
                DepartamentoCodigo = "010", 
                Preco = 10.50m,
                Status = true
            };

            _mockConnection.Setup(x => x.ExecuteScalarAsync<int>(
                It.IsAny<string>(),
                It.IsAny<object>(),
                It.IsAny<IDbTransaction>(),
                It.IsAny<int?>(),
                It.IsAny<CommandType?>()))
                .ReturnsAsync(0); 

            _mockConnection.Setup(x => x.ExecuteAsync(
                It.IsAny<string>(),
                It.IsAny<object>(),
                It.IsAny<IDbTransaction>(),
                It.IsAny<int?>(),
                It.IsAny<CommandType?>()))
                .ReturnsAsync(1);

            // Act
            var resultado = await _produtoService.CriarProduto(produtoDto);

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal(produtoDto.Codigo, resultado.Codigo);
            Assert.Equal(produtoDto.Descricao, resultado.Descricao);
            Assert.Equal(produtoDto.DepartamentoCodigo, resultado.Departamento);
            Assert.Equal(produtoDto.Preco, resultado.Preco);
            Assert.True(resultado.Status); 
            Assert.NotEqual(Guid.Empty, resultado.Id);
        }

        [Fact]
        public async Task CodigoProdutoExiste_DeveRetornarTrueQuandoCodigoExiste()
        {
            // Arrange
            var codigo = "TEST001";

            _mockConnection.Setup(x => x.ExecuteScalarAsync<int>(
                It.IsAny<string>(),
                It.IsAny<object>(),
                It.IsAny<IDbTransaction>(),
                It.IsAny<int?>(),
                It.IsAny<CommandType?>()))
                .ReturnsAsync(1); 

            var resultado = await _produtoService.CodigoProdutoExiste(codigo);

            Assert.True(resultado);
        }

        [Fact]
        public async Task CodigoProdutoExiste_DeveRetornarFalseQuandoCodigoNaoExiste()
        {
            var codigo = "TEST999";

            _mockConnection.Setup(x => x.ExecuteScalarAsync<int>(
                It.IsAny<string>(),
                It.IsAny<object>(),
                It.IsAny<IDbTransaction>(),
                It.IsAny<int?>(),
                It.IsAny<CommandType?>()))
                .ReturnsAsync(0); 

            // Act
            var resultado = await _produtoService.CodigoProdutoExiste(codigo);

            // Assert
            Assert.False(resultado);
        }

        [Fact]
        public async Task ObterPorId_DeveRetornarProdutoQuandoExistir()
        {
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

            _mockConnection.Setup(x => x.QueryFirstOrDefaultAsync<Produto>(
                It.IsAny<string>(),
                It.IsAny<object>(),
                It.IsAny<IDbTransaction>(),
                It.IsAny<int?>(),
                It.IsAny<CommandType?>()))
                .ReturnsAsync(produto);

            var resultado = await _produtoService.ObterPorId(id);

            Assert.NotNull(resultado);
            Assert.Equal(id, resultado.Id);
            Assert.Equal(produto.Codigo, resultado.Codigo);
        }

        [Fact]
        public async Task ObterPorId_DeveRetornarNullQuandoNaoExistir()
        {
            var id = Guid.NewGuid();

            _mockConnection.Setup(x => x.QueryFirstOrDefaultAsync<Produto>(
                It.IsAny<string>(),
                It.IsAny<object>(),
                It.IsAny<IDbTransaction>(),
                It.IsAny<int?>(),
                It.IsAny<CommandType?>()))
                .ReturnsAsync((Produto)null);

            var resultado = await _produtoService.ObterPorId(id);

            Assert.Null(resultado);
        }
    }
}