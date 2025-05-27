using FacebookLike.Models;
using FacebookLike.Neo4j.Node;
using FacebookLike.Repository;
using FacebookLike.Service.Neo4jService;

namespace FacebookLike.Service;

public class PostService : IPostService
{
    private readonly PostRepository _postRepository;
    private readonly UserRelationRepository _userRelationRepository;
    private List<Post> _posts = new();

    public PostService(PostRepository postRepository, UserRelationRepository userRelationRepository)
    {
        _postRepository = postRepository;
        _userRelationRepository = userRelationRepository;
    }

    public async Task<List<Post>> GetPostsAsync()
    {
        _posts = await _postRepository.GetAll();
        return _posts;
    }

    public async Task<List<PostDetails>> GetFriendsPostsAsync(string userId, int page, int pageSize)
    {
        var friendIds = await _userRelationRepository.GetFriendIds(userId);
        friendIds.Add(userId); // Inclure les posts de l'utilisateur lui-même
        int skip = (page - 1) * pageSize;
        return await _postRepository.GetPostsByAuthorsAsync(friendIds, skip, pageSize, userId);
    }
}