using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace CadastroPessoas.Infrastructure.Factory
{
    public class SqlFactory(IConfiguration configuration)
    {
        public IDbConnection CreateConnection()
        {
            string connectionString = configuration.GetConnectionString("DefaultConnection")!;
            return new SqlConnection(connectionString);
        }
    }
}
