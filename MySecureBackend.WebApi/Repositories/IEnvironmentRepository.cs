using MySecureBackend.WebApi.Models;

namespace MySecureBackend.WebApi.Repositories
{
    public interface IEnvironmentRepository
    {
        Task InsertAsync(Environment2D environment);
        Task DeleteAsync(string id);
        Task<IEnumerable<Environment2D>> SelectAsync();
        Task<IEnumerable<Environment2D>> SelectByOwnerIdAsync(string ownerId);
        Task<Environment2D?> SelectAsync(string id);
        Task UpdateAsync(Environment2D environment);
    }
}
