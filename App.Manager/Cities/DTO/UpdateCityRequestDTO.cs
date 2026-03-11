using System;
using System.Collections.Generic;
using System.Text;

namespace App.Manager.Cities.DTO
{
    public record UpdateCityRequestDTO(
        int Id,
        string Name,
        int CountryId
    );
}
