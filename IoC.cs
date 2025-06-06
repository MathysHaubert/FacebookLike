using FacebookLike.Repository;
using FacebookLike.Service;
using FacebookLike.Service.GoogleCloud;
using FacebookLike.Service.Neo4jService;
using FacebookLike.Service.Security;
using Microsoft.AspNetCore.Authentication;

namespace FacebookLike
{
    public static class IoC
    {
        public static IServiceCollection AddFacebookLikeServices(this IServiceCollection services)
        {
            services.AddScoped<UserRepository>();
            services.AddScoped<UserRelationRepository>();
            services.AddScoped<PostRepository>();
            services.AddScoped<CommentRepository>();
            services.AddScoped<LikeRepository>();
            services.AddScoped<ConversationRepository>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddSingleton<StorageService>();
            services.AddScoped<AuthorizationHandler>();

            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IPostService, PostService>();
            services.AddScoped<ICommentService, CommentService>();
            services.AddScoped<IMessageService, MessageService>();
            services.AddScoped<IFriendService, FriendService>();
          
            services.AddScoped<UserProfileService>();
            
            return services;
        }
    }
} 