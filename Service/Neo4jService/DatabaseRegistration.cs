using Neo4j.Driver;
using Neo4jClient;

namespace FacebookLike.Service.Neo4jService
{
    public static class DatabaseRegistration
    {
        public static IServiceCollection AddNeo4jDatabase(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddSingleton<IGraphClient>(provider =>
            {
                var uri = "bolt://localhost:7687";
                var user = "neo4j";
                var password = configuration["Neo4j:Password"];

                if (string.IsNullOrWhiteSpace(password))
                    throw new InvalidOperationException("The password is not configurated");
                
                var driver = GraphDatabase.Driver(
                    uri,
                    AuthTokens.Basic(user, password)
                );           
                
                var client = new BoltGraphClient(driver);
                client.ConnectAsync().Wait();
                
                return client;
            });

            return services;
        }
    }
} 