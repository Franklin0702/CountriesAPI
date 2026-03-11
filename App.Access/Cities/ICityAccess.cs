using App.Access.Entities;

namespace App.Access.Cities
{
    public interface ICityAccess
    {
        Task<IEnumerable<City>> GetAllAsync();

        Task<City?> GetByIdAsync(int id);

        Task<City> AddAsync(City country);

        Task<City> UpdateAsync(City country);

        Task DeleteAsync(int id);

        Task<bool> ExistsAsync(int id);
    }
}
