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
            services.AddSingleton<CommentRepository>();
            services.AddSingleton<LikeRepository>();
            services.AddSingleton<IAuthService, AuthService>();

            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IPostService, PostService>();
            services.AddScoped<ICommentService, CommentService>();
          
            return services;
        }
    }
} 