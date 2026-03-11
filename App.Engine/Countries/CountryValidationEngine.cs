using App.Access.Countries;
using App.Access.Entities;
using System.Text.RegularExpressions;

namespace App.Engine.Countries;

public class CountryValidationEngine(ICountryAccess CountryAccess) : ICountryValidationEngine
{
    private static readonly Regex _twoLetterRegex = new Regex("^[A-Za-z]{2}$", RegexOptions.Compiled);
    private static readonly Regex _threeLetterRegex = new Regex("^[A-Za-z]{3}$", RegexOptions.Compiled);

    public async Task ValidateCreateRequestAsync(string name, string code, int latitude, int longitude)
    {
        await ValidateCodeAsync(code);
        ValidateName(name);
        ValidateCoordinates(latitude, longitude);
    }

    public async Task ValidateUpdateRequestAsync(int id, string name, string code)
    {
        var exists = await CountryAccess.ExistsAsync(id);
        if (!exists)
            throw new ArgumentException($"Country with Id {id} does not exist.", nameof(id));
        await ValidateCodeAsync(code);
        ValidateName(name);
    }


    private async Task ValidateCodeAsync(string code)
    {
        if (string.IsNullOrWhiteSpace(code))
            throw new ArgumentException("Code must not be null, empty or whitespace.", nameof(code));
        if (!_twoLetterRegex.IsMatch(code) && !_threeLetterRegex.IsMatch(code))
            throw new ArgumentException("Code must be either 2 or 3 letters.", nameof(code));
        var isUnique = await CountryAccess.IsCodeUniqueAsync(code);
        if (!isUnique)
            throw new ArgumentException("Code must be unique.", nameof(code));
    }

    private void ValidateName(string name)
    {
        const int MaxNameLength = 100;
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name must not be null, empty or whitespace.", nameof(name));
        if (name.Length > MaxNameLength)
            throw new ArgumentException($"Name cannot exceed {MaxNameLength} characters.", nameof(name));
    }

    private void ValidateCoordinates(int latitude, int longitude)
    {
        if (latitude < -90 || latitude > 90)
            throw new ArgumentException("Latitude must be between -90 and 90.", nameof(latitude));
        if (longitude < -180 || longitude > 180)
            throw new ArgumentException("Longitude must be between -180 and 180.", nameof(longitude));
    }
}
