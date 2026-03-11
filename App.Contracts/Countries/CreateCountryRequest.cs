namespace App.Contracts.Countries;

public record CreateCountryRequest(
	string CountryName,
	string CountryCode,
	int Latitude,
	int Longitude
);