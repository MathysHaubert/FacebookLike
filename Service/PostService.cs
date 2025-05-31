using FacebookLike.Models;
using FacebookLike.Neo4j.Node;
using FacebookLike.Service.GoogleCloud;
using FacebookLike.Service.Neo4jService;

namespace FacebookLike.Service;

public class PostService : IPostService
{
    private readonly PostRepository _postRepository;
    private readonly UserRelationRepository _userRelationRepository;
    private readonly StorageService _storageService;
    private List<Post> _posts = new();

    public PostService(PostRepository postRepository, UserRelationRepository userRelationRepository, StorageService storageService)
    {
        _postRepository = postRepository;
        _userRelationRepository = userRelationRepository;
        _storageService = storageService;
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

    public async Task<PostDetails> CreatePostAsync(string userId, string content, Stream imageStream, string fileName)
    {
        var post = new Post
        {
            Id = Guid.NewGuid().ToString(),
            AuthorId = userId,
            Content = content,
            CreatedAt = DateTime.Now
        };

        if (imageStream != null && fileName != null)
        {
            post.ImageUrl = await _storageService.UploadAndGetUrlAsync(imageStream, fileName);
        }

        await _postRepository.Create(post);
        return new PostDetails { Post = post };
    }
}