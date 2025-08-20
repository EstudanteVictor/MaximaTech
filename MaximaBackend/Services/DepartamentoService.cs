using MaximaBackend.Models;
using System.Data;
using Dapper;

namespace MaximaBackend.Services
{
    public class DepartamentoService : IDepartamentoInterface
    {
        private readonly IDbConnection _connection;

        public DepartamentoService(IDbConnection connection)
        {
            _connection = connection;
        }

        /// <summary>
        /// método para obter todos os departamentos
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<Departamento>> BuscarDepartamentos()
        {
            const string sql = "SELECT codigo, descricao FROM departamentos ORDER BY codigo";
            return await _connection.QueryAsync<Departamento>(sql);
        }

        /// <summary>
        /// Obter o texto do departamento
        /// </summary>
        /// <param name = "codigo" ></ param >
        /// < returns ></ returns >
        public async Task<string> BuscarDepartamentoPorCodigo(string codigo)
        {
            string sql = "SELECT descricao FROM departamentos WHERE codigo = '"+ codigo +"'";
            return await _connection.QuerySingleAsync<string>(sql, sql);
        }
    }
}

