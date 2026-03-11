using App.Access.Cities;
using App.Access.Countries;
using App.Access;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace App.Access
{
    public static class DIHelper
    {
        public static IServiceCollection AddAccess(
            this IServiceCollection services, 
            IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(options => 
                options.UseSqlite(configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped<ICountryAccess, CountryAccess>();
            services.AddScoped<ICityAccess, CityAccess>();
            return services;
        }
    }
}
