using App.Access.Cities;
using App.Access.Countries;

namespace App.Engine.Cities
{
    public class CityValidationEngine(ICityAccess CityAccess, ICountryAccess CountryAccess) : ICityValidationEngine
    {
        public async Task ValidateCreateRequestAsync(string name, int countryId)
        {
            ValidateName(name);
            await ValidateCountryId(countryId);
        }

        public async Task ValidateUpdateRequestAsync(int id, string name, int countryId)
        {
            var exists = await CityAccess.ExistsAsync(id);
            if (!exists)
                throw new ArgumentException($"City with Id {id} does not exist.", nameof(id));

            ValidateName(name);
            await ValidateCountryId(countryId);
        }

        private async Task ValidateCountryId(int countryId)
        {
            var countryExists = await CountryAccess.ExistsAsync(countryId);
            if (!countryExists)
                throw new ArgumentException("CountryId must be a positive integer.", nameof(countryId));
        }

        private void ValidateName(string name)
        {
            const int MaxNameLength = 100;
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name must not be null, empty or whitespace.", nameof(name));
            if (name.Length > MaxNameLength)
                throw new ArgumentException($"Name cannot exceed {MaxNameLength} characters.", nameof(name));
        }
    }
}
