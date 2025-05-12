using FacebookLike.Models;
using FacebookLike.Service.Neo4jService;

namespace FacebookLike.Service
{
    public class PostService : IPostService
    {
        private readonly PostRepository _postRepository;
        private List<Post> _posts = new();

        public PostService(PostRepository postRepository)
        {
            _postRepository = postRepository;
        }

        public async Task<List<Post>> GetPostsAsync()
        {
            _posts = await _postRepository.GetAll();
            return _posts;
        }

        public void LikePost(string postId)
        {
            var post = _posts.FirstOrDefault(p => p.Id == postId);
            if (post != null)
            {
                post.LikesCount++;
            }
        }

        public void UnlikePost(string postId)
        {
            var post = _posts.FirstOrDefault(p => p.Id == postId);
            if (post != null && post.LikesCount > 0)
            {
                post.LikesCount--;
            }
        }

        public void AddOrRemoveLike(Post post)
        {
            if (post.LikesCount > 0)
            {
                UnlikePost(post.Id);
            }
            else
            {
                LikePost(post.Id);
            }
        }
    }
}
