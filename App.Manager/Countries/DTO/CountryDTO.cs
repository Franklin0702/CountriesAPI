using App.Access.Entities;
using App.Manager.Cities.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.Manager.Countries.DTO
{
    public class CountryDTO
    {
        public required int Id { get; set; }
        public required string Name { get; set; }
        public required string Code { get; set; }
        public required string Coordinates { get; set; }

        public required IList<CityDTO> Cities { get; set; }

        public static CountryDTO fromModel(Country country, bool loadCities = true)
        {
            return new CountryDTO
            {
                Id = country.Id,
                Name = country.CountryName,
                Code = country.CountryCode,
                Coordinates = country.Coordinates is null ? "" : country.Coordinates,
                Cities = country.Cities == null || !loadCities ? new List<CityDTO>() : [.. country.Cities.Select(CityDTO.fromModel)]
            };
        }

        public Country toModel()
        {
            return new Country
            {
                Id = Id,
                CountryName = Name,
                CountryCode = Code,
                Coordinates = Coordinates,
                Cities = Cities.Select(city => city.toModel()).ToList()
            };
        }

    }
}
