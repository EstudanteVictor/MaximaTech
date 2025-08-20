using MaximaBackend.Models;
using MaximaBackend.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MaximaBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class DepartamentoController : ControllerBase
    {
        private readonly IDepartamentoInterface _departamentoService;
        private readonly ILogger<DepartamentoController> _logger;

        public DepartamentoController(IDepartamentoInterface departamentoService, ILogger<DepartamentoController> logger)
        {
            _departamentoService = departamentoService;
            _logger = logger;
        }

        /// <summary>
        /// Obtém todos os departamentos
        /// </summary>
        /// <returns>Lista de departamentos</returns>
        /// <response code="200">Retorna a lista de departamentos</response>
        /// <response code="500">Erro interno do servidor</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Departamento>), 200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<IEnumerable<Departamento>>> Get()
        {
            try
            {
                _logger.LogInformation("Buscando todos os departamentos");
                var departamentos = await _departamentoService.BuscarDepartamentos();
                return Ok(departamentos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar departamentos");
                return StatusCode(500, new { message = "Erro interno do servidor" });
            }
        }
    }
}
