using App.Access.Entities;
using App.Manager.Countries.DTO;

namespace App.Manager.Cities.DTO
{
    public class CityDTO
    {
        public required int Id { get; set; }
        public required string Name { get; set; }
        public required int CountryId { get; set; }
        public CountryDTO? Country { get; set; }

        public City toModel()
        {
            return new City
            {
                Id = Id,
                Name = Name,
                CountryId = CountryId,
                Country = Country.toModel()
            };
        }

        public static CityDTO fromModel(City city)
        {
            return new CityDTO
            {
                Id = city.Id,
                Name = city.Name,
                CountryId = city.CountryId,
                Country = city.Country != null ? CountryDTO.fromModel(city.Country, loadCities: false) : null
            };
        }
    }
}
