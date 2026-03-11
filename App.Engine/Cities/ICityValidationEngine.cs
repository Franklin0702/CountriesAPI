namespace App.Engine.Cities
{
    public interface ICityValidationEngine
    {
        Task ValidateCreateRequestAsync(string name, int countryId);
        Task ValidateUpdateRequestAsync(int id, string name, int countryId);
    }
}
