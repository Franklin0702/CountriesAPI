using App.Manager.Cities.DTO;

namespace App.Manager.Cities
{
    public interface ICityManager
    {
        Task<CityDTO> GetAsync(int cityId);
        Task<List<CityDTO>> GetAllAsync();
        Task<CityDTO> CreateAsync(CreateCityRequestDTO request);
        Task<CityDTO> UpdateAsync(UpdateCityRequestDTO request);
        Task DeleteAsync(int cityId);
    }
}
