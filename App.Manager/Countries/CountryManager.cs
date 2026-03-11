using App.Access.Cities;
using App.Access.Countries;
using App.Access.Entities;
using App.Engine.Countries;
using App.Manager.Cities.DTO;
using App.Manager.Countries.DTO;

namespace App.Manager.Countries
{
    public class CountryManager(ICountryValidationEngine ValidationEngine, ICountryAccess CountryAccess) : ICountryManager
    {
        public async Task<CountryDTO> CreateAsync(CreateCountryRequestDTO request)
        {
            await ValidationEngine.ValidateCreateRequestAsync(request.Name, request.Code, request.Latitude, request.Longitude);
            var newCountry = await CountryAccess.AddAsync(new Country
            {
                CountryName = request.Name,
                CountryCode = request.Code,
                Coordinates = $"{request.Latitude},{request.Longitude}",
            });
            return CountryDTO.fromModel(newCountry);
        }

        public Task DeleteAsync(int countryId)
        {
            return CountryAccess.DeleteAsync(countryId);
        }

        public async Task<List<CountryDTO>> GetAllAsync()
        {
            // TODO: Add pagination and filtering
            var countries = await CountryAccess.GetAllAsync();
            return countries.Select(country => CountryDTO.fromModel(country)).ToList();
        }

        public async Task<CountryDTO> GetAsync(int countryId)
        {
            var country = await CountryAccess.GetByIdAsync(countryId);
            if (country == null)
            {
                throw new KeyNotFoundException($"Country with ID {countryId} not found.");
            }
            return CountryDTO.fromModel(country);
        }

        public async Task<CountryDTO> UpdateAsync(UpdateCountryRequestDTO request)
        {
            await ValidationEngine.ValidateUpdateRequestAsync(request.Id, request.Name, request.Code);
            var updatedCountry = await CountryAccess.UpdateAsync(new Country
            {
                Id = request.Id,
                CountryName = request.Name,
                CountryCode = request.Code
            });

            return CountryDTO.fromModel(updatedCountry);
        }
    }
}
