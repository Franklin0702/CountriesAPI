using App.Manager;
using App.Engine;
using App.Access;

namespace App.Host
{
    public static class DependencyInjectionConfig
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAccess(configuration);
            services.AddEngines();
            services.AddManagers();

            return services;
        }
    }
}
