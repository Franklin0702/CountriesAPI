using App.Manager.Cities;
using App.Manager.Countries;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.Manager
{
    public static class DIHelper
    {
        public static IServiceCollection AddManagers(this IServiceCollection services)
        {
            services.AddScoped<ICountryManager, CountryManager>();
            services.AddScoped<ICityManager, CityManager>();
            return services;
        }
    }
}
