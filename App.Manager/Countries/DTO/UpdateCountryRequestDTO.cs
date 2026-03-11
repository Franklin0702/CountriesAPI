using System;
using System.Collections.Generic;
using System.Text;

namespace App.Manager.Countries.DTO
{
    public record UpdateCountryRequestDTO(
        int Id,
        string Name,
        string Code
    );
}
