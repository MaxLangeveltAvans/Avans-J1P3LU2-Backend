using Dapper;
using MySqlConnector;
using MySecureBackend.WebApi.Models;

namespace MySecureBackend.WebApi.Repositories
{
    public class SqlEnvironmentRepository(string sqlConnectionString) : IEnvironmentRepository
    {
        public async Task InsertAsync(Environment2D environment)
        {
            using (var sqlConnection = new MySqlConnection(sqlConnectionString))
            {
                await sqlConnection.ExecuteAsync(
                    "INSERT INTO Environment " +
                    "(Id, Name, MaxHeight, MaxLength, OwnerId) " +
                    "VALUES (@Id, @Name, @MaxHeight, @MaxLength, @OwnerId)", 
                    environment);
            }
        }
    
        public async Task DeleteAsync(string id)
        {
            using (var sqlConnection = new MySqlConnection(sqlConnectionString))
            {
                await sqlConnection.ExecuteAsync("DELETE FROM Environment WHERE Id = @Id", new { id });
            }
        }
    
        public async Task<IEnumerable<Environment2D>> SelectAsync()
        {
            using (var sqlConnection = new MySqlConnection(sqlConnectionString))
            {
                return await sqlConnection.QueryAsync<Environment2D>("SELECT * FROM Environment");
            }
        }

        public async Task<IEnumerable<Environment2D>> SelectByOwnerIdAsync(string ownerId)
        {
            using (var sqlConnection = new MySqlConnection(sqlConnectionString))
            {
                return await sqlConnection.QueryAsync<Environment2D>("SELECT * FROM Environment WHERE OwnerId = @OwnerId", new { ownerId });
            }
        }
        
        public async Task<Environment2D?> SelectAsync(string id)
        {
            using (var sqlConnection = new MySqlConnection(sqlConnectionString))
            {
                return await sqlConnection.QuerySingleOrDefaultAsync<Environment2D>("SELECT * FROM Environment WHERE Id = @Id", new { id });   
            }
        }
    
        public async Task UpdateAsync(Environment2D environment)
        {
            using (var sqlConnection = new MySqlConnection(sqlConnectionString))
            {
                await sqlConnection.ExecuteAsync("UPDATE Environment SET " +
                                                 "Name = @Name, " +
                                                 "MaxHeight = @MaxHeight, " +
                                                 "MaxLength = @MaxLength" +
                                                 "WHERE Id = @Id", environment);

            }
        }
    }
}

