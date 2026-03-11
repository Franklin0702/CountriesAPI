using App.Access.Cities;
using App.Access.Entities;
using App.Engine.Cities;
using App.Manager.Cities.DTO;

namespace App.Manager.Cities
{
    internal class CityManager(ICityValidationEngine ValidationEngine, ICityAccess CityAccess) : ICityManager
    {
        public async Task<CityDTO> CreateAsync(CreateCityRequestDTO request)
        {
            await ValidationEngine.ValidateCreateRequestAsync(request.Name, request.CountryId);
            var newCity = await CityAccess.AddAsync(new City
            {
                Name = request.Name,
                CountryId = request.CountryId
            });

            return CityDTO.fromModel(newCity);
        }

        public Task DeleteAsync(int cityId)
        {
            return CityAccess.DeleteAsync(cityId);
        }

        public async Task<List<CityDTO>> GetAllAsync()
        {
            // TODO: Add pagination and filtering
            var cities = await CityAccess.GetAllAsync();
            return cities.Select(CityDTO.fromModel).ToList();
        }

        public async Task<CityDTO> GetAsync(int cityId)
        {
            var city = await CityAccess.GetByIdAsync(cityId);
            if (city == null) { 
                throw new KeyNotFoundException($"City with ID {cityId} not found.");
            }
            return CityDTO.fromModel(city);
        }

        public async Task<CityDTO> UpdateAsync(UpdateCityRequestDTO request)
        {
            await ValidationEngine.ValidateUpdateRequestAsync(request.Id, request.Name, request.CountryId);
            var updatedCity = await CityAccess.UpdateAsync(new City
            {
                Id = request.Id,
                Name = request.Name,
                CountryId = request.CountryId
            });

            return CityDTO.fromModel(updatedCity);
        }
    }
}
