using Dapper;
using System.Data;

namespace MaximaBackend.Data
{
    public static class DatabaseInitializer
    {
        public static void Initialize(IDbConnection connection)
        {
            connection.Open();

            var createProductTable = @"
                CREATE TABLE IF NOT EXISTS produtos (
                    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
                    codigo VARCHAR(50) NOT NULL UNIQUE,
                    descricao VARCHAR(500) NOT NULL,
                    departamento VARCHAR(50) NOT NULL,
                    preco DECIMAL(10,2) NOT NULL,
                    status BOOLEAN NOT NULL DEFAULT TRUE
                );";

            connection.Execute(createProductTable);

            var createDepartmentTable = @"
                CREATE TABLE IF NOT EXISTS departamentos (
                    codigo VARCHAR(10) PRIMARY KEY,
                    descricao VARCHAR(100) NOT NULL
                );";

            connection.Execute(createDepartmentTable);

            var insertDepartments = @"
                INSERT INTO departamentos (codigo, descricao) VALUES
                ('010', 'BEBIDAS'),
                ('020', 'CONGELADOS'),
                ('030', 'LATICÍNIOS'),
                ('040', 'VEGETAIS')
                ON CONFLICT (codigo) DO NOTHING;";

            connection.Execute(insertDepartments);

            var createIndexes = @"
                CREATE INDEX IF NOT EXISTS idx_produtos_codigo ON produtos(codigo);
                CREATE INDEX IF NOT EXISTS idx_produtos_departamento ON produtos(departamento);
                CREATE INDEX IF NOT EXISTS idx_produtos_status ON produtos(status);";

            connection.Execute(createIndexes);

            connection.Close();
        }
    }
}
