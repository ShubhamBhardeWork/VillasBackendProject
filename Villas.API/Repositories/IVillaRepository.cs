using Villas.API.Models.Domain;

namespace Villas.API.Repositories
{
    public interface IVillaRepository
    {
        Task<IEnumerable<Villa>> GetAllAsync();
        Task<Villa?> GetByIdAsync(int id);
        Task<Villa> CreateAsync(Villa villa);
    }
}
