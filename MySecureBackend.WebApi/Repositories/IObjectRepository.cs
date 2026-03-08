using MySecureBackend.WebApi.Models;

namespace MySecureBackend.WebApi.Repositories
{
    public interface IObjectRepository
    {
        Task InsertAsync(Object2D object2D);
        Task DeleteAsync(string id);
        Task<IEnumerable<Object2D>> SelectByEnvironmentAsync(string environmentId);
        Task<Object2D?> SelectAsync(string id);
        Task UpdateAsync(Object2D object2D);
    }
}
