using App.Access.Entities;

namespace App.Access.Countries
{
    public interface ICountryAccess
    {
        Task<IEnumerable<Country>> GetAllAsync();

        Task<Country?> GetByIdAsync(int id);

        Task<Country> AddAsync(Country country);

        Task<Country> UpdateAsync(Country country);

        Task DeleteAsync(int id);

        Task<bool> ExistsAsync(int id);
        Task<bool> IsCodeUniqueAsync(string code);
    }
}
