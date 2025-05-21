using FacebookLike.Repository;
using FacebookLike.Service;
using FacebookLike.Service.Neo4jService;

namespace FacebookLike
{
    public static class IoC
    {
        public static IServiceCollection AddFacebookLikeServices(this IServiceCollection services)
        {
            services.AddSingleton<UserRepository>();
            services.AddSingleton<UserRelationRepository>();
            services.AddSingleton<PostRepository>();
            services.AddSingleton<IAuthService, AuthService>();

            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IPostService, PostService>();
          
            return services;
        }
    }
} 