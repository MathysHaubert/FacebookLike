using FacebookLike.Service;
using FacebookLike.Service.Neo4jService;

namespace FacebookLike.Neo4j.DataSeeder
{
    public static class DataSeederRegistration
    {
        public static IServiceCollection AddSeederServices(this IServiceCollection services)
        {
            services.AddScoped<DataSeederUtils>();
            services.AddScoped<InitSeeder>();
          
            return services;
        }
    }
} 