using System.Data;
using MySqlConnector;

namespace Avans.Identity.Dapper
{
    /// <summary>
    /// Creates a new <see cref="MySqlConnection"/> instance for connecting to Microsoft SQL Server.
    /// </summary>
    public class SqlServerDbConnectionFactory : IDbConnectionFactory
    {
        /// <summary>
        /// The connection string to use for connecting to Microsoft SQL Server.
        /// </summary>
        public string ConnectionString { get; set; }

        /// <inheritdoc/>
        public IDbConnection Create() {
            var sqlConnection = new MySqlConnection(ConnectionString);
            sqlConnection.Open();
            return sqlConnection;
        }
    }
}
