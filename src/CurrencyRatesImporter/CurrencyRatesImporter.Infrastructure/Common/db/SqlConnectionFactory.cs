using Microsoft.Data.SqlClient;
using System.Data;

namespace CurrencyRatesImporter.Infrastructure.Common.db
{
    public class SqlConnectionFactory : ISqlConnectionFactory
    {
        private readonly string _connectionString;

        public SqlConnectionFactory(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<SqlConnection> CreateAsync()
        {
            var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            return connection;
        }
    }

}
