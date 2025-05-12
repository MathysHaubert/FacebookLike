using FacebookLike.Models;
    
namespace FacebookLike.Services
{
    public interface IPostService
    {
        List<Post> GetPosts();
        void LikePost(int postId);
        void UnlikePost(int postId);
    }

    public class PostService : IPostService
    {
        private readonly List<Post> _posts;
        private readonly IAuthService _authService;

        public PostService(IAuthService authService)
        {
            _authService = authService;
            _posts = new List<Post>
            {
                new Post
                {
                    Id = 1,
                    Author = _authService.GetUserById(1),
                    Content = "Juste un magnifique coucher de soleil ce soir ! 🌅 #nature #beauté",
                    ImageUrl = "https://source.unsplash.com/random/800x600?sunset",
                    CreatedAt = DateTime.Now.AddHours(-2),
                    LikesCount = 42,
                    CommentsCount = 5,
                    IsLiked = false
                },
                new Post
                {
                    Id = 2,
                    Author = _authService.GetUserById(2),
                    Content = "Mon nouveau projet de développement est en cours ! #coding #webdev",
                    ImageUrl = "https://source.unsplash.com/random/800x600?coding",
                    CreatedAt = DateTime.Now.AddHours(-5),
                    LikesCount = 28,
                    CommentsCount = 3,
                    IsLiked = true
                },
                new Post
                {
                    Id = 3,
                    Author = _authService.GetUserById(1),
                    Content = "Première randonnée de l'année ! Les paysages sont incroyables 🏔️",
                    ImageUrl = "https://source.unsplash.com/random/800x600?mountain",
                    CreatedAt = DateTime.Now.AddHours(-8),
                    LikesCount = 156,
                    CommentsCount = 12,
                    IsLiked = false
                }
            };
        }

        public List<Post> GetPosts()
        {
            return _posts.OrderByDescending(p => p.CreatedAt).ToList();
        }

        public void LikePost(int postId)
        {
            var post = _posts.FirstOrDefault(p => p.Id == postId);
            if (post != null && !post.IsLiked)
            {
                post.LikesCount++;
                post.IsLiked = true;
            }
        }

        public void UnlikePost(int postId)
        {
            var post = _posts.FirstOrDefault(p => p.Id == postId);
            if (post != null && post.IsLiked)
            {
                post.LikesCount--;
                post.IsLiked = false;
            }
        }
    }
} 