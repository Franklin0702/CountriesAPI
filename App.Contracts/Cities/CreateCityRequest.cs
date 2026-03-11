using App.Contracts.Countries;

namespace App.Contracts.Cities;

public record CreateCityRequest(
	string Name,
	string CountryId,
);