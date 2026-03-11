namespace App.Manager.Countries.DTO
{
    public record CreateCountryRequestDTO(
        string Name,
        string Code,
        int Latitude,
        int Longitude
    );
}
