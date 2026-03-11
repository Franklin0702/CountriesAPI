using App.Manager.Cities.DTO;
using App.Manager.Countries.DTO;

namespace App.Manager.Countries
{
    public interface ICountryManager
    {
        Task<CountryDTO> GetAsync(int cityId);
        Task<List<CountryDTO>> GetAllAsync();
        Task<CountryDTO> CreateAsync(CreateCountryRequestDTO request);
        Task<CountryDTO> UpdateAsync(UpdateCountryRequestDTO request);
        Task DeleteAsync(int cityId);
    }
}
