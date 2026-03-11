using App.Engine.Cities;
using App.Engine.Countries;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace App.Engine
{
    public static class DIHelper
    {
        public static IServiceCollection AddEngines(this IServiceCollection services)
        {
            services.AddScoped<ICountryValidationEngine, CountryValidationEngine>();
            services.AddScoped<ICityValidationEngine, CityValidationEngine>();
            return services;
        }
    }
}
