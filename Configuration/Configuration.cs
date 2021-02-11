using BSUIR.BL.ServicesConfiguration;
using BSUIR.DAL.ServicesConfiguration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BSUIR.Configuration
{
    public static class Configuration
    {
        public static void ConfigureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.ConfigureBusinessLogicServices(configuration);
            services.ConfigureDataAccessServices(configuration);
        }
    }
}
