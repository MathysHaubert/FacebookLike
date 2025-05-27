using FacebookLike.Models;
using FacebookLike.Service.Neo4jService;
using FacebookLike.Repository;

namespace FacebookLike.Service
{
    public class PostService : IPostService
    {
        private readonly PostRepository _postRepository;
        private readonly UserRelationRepository _userRelationRepository;
        private readonly UserRepository _userRepository;
        private List<Post> _posts = new();

        public PostService(PostRepository postRepository, UserRelationRepository userRelationRepository, UserRepository userRepository)
        {
            _postRepository = postRepository;
            _userRelationRepository = userRelationRepository;
            _userRepository = userRepository;
        }

        public async Task<List<Post>> GetPostsAsync()
        {
            _posts = await _postRepository.GetAll();
            return _posts;
        }

        public async Task<List<PostWithAuthor>> GetFriendsPostsAsync(string userId, int page, int pageSize)
        {
            var friendIds = await _userRelationRepository.GetFriendIds(userId);
            friendIds.Add(userId); // Inclure les posts de l'utilisateur lui-même
            int skip = (page - 1) * pageSize;
            return await _postRepository.GetPostsByAuthorsAsync(friendIds, skip, pageSize, userId);
        }
    }
}
