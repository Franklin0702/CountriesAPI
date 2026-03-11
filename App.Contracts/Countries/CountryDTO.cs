using App.Contracts.Cities;

namespace App.Contracts.Countries;

public record CountryDTO(
	string Name,
	string Code,
	string Coordinates,
	List<CityDTO> Cities
);