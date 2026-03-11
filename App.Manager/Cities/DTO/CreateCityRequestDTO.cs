namespace App.Manager.Cities.DTO
{
    public record CreateCityRequestDTO(
        string Name,
        int CountryId
    );
}
