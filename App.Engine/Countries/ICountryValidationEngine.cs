using App.Access.Entities;

namespace App.Engine.Countries
{
    public interface ICountryValidationEngine
    {
        Task ValidateCreateRequestAsync(string name, string code, int latitude, int longitude);
        Task ValidateUpdateRequestAsync(int id, string name, string code);
    }
}
