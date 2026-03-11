using App.Contracts.Countries;

namespace App.Contracts.Cities;

public record CityDTO(
	int Id,
	string Name,
	int CountryId,
    CountryDTO Country
);