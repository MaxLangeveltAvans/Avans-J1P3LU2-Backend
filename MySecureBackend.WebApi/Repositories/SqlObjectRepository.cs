using Dapper;
using MySqlConnector;
using MySecureBackend.WebApi.Models;

namespace MySecureBackend.WebApi.Repositories
{
    public class SqlObjectRepository(string sqlConnectionString) : IObjectRepository
    {
        public async Task InsertAsync(Object2D object2D)
        {
            using (var sqlConnection = new MySqlConnection(sqlConnectionString))
            {
                await sqlConnection.ExecuteAsync(
                    "INSERT INTO Object " +
                    "(Id, PrefabId, PositionX, PositionY, ScaleX, ScaleY, RotationZ, SortingLayer) " +
                    "VALUES (@Id, @PrefabId, @PositionX, @PositionY, @ScaleX, @ScaleY, @RotationZ, @SortingLayer)", 
                    object2D);
            }
        }

        public async Task DeleteAsync(string id)
        {
            using (var sqlConnection = new MySqlConnection(sqlConnectionString))
            {
                await sqlConnection.ExecuteAsync("DELETE FROM Object WHERE Id = @Id", new { id });
            }
        }

        public async Task<IEnumerable<Object2D>> SelectAsync()
        {
            using (var sqlConnection = new MySqlConnection(sqlConnectionString))
            {
                return await sqlConnection.QueryAsync<Object2D>("SELECT * FROM Object");
            }
        }

        public async Task<Object2D?> SelectAsync(string id)
        {
            using (var sqlConnection = new MySqlConnection(sqlConnectionString))
            {
                return await sqlConnection.QuerySingleOrDefaultAsync<Object2D>("SELECT * FROM Object WHERE Id = @Id", new { id });   
            }
        }

        public async Task UpdateAsync(Object2D object2D)
        {
            using (var sqlConnection = new MySqlConnection(sqlConnectionString))
            {
                await sqlConnection.ExecuteAsync("UPDATE Object SET " +
                                                 "PrefabId = @PrefabId, " +
                                                 "PositionX = @PositionX, " +
                                                 "PositionY = @PositionY, " +
                                                 "ScaleX = @ScaleX, " +
                                                 "ScaleY = @ScaleY, " +
                                                 "RotationZ = @RotationZ, " +
                                                 "SortingLayer = @SortingLayer " +
                                                 "WHERE Id = @Id", object2D);

            }
        }
    }
}

