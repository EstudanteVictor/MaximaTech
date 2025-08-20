using MaximaBackend.Models;

namespace MaximaBackend.Services
{
    public interface IDepartamentoInterface
    {
        Task<IEnumerable<Departamento>> BuscarDepartamentos();

        Task<string> BuscarDepartamentoPorCodigo(string codigo);
    }
}
