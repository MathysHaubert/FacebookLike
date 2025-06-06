using FacebookLike.Models;
using FacebookLike.Neo4j.Node;
using FacebookLike.Repository;
using FacebookLike.Service.GoogleCloud;
using FacebookLike.Service.Neo4jService;

namespace FacebookLike.Service;

public class PostService : IPostService
{
    private readonly PostRepository _postRepository;
    private readonly UserRelationRepository _userRelationRepository;
    private readonly StorageService _storageService;
    private readonly LikeRepository _likeRepository;
    private List<Post> _posts = new();

    public PostService(PostRepository postRepository, UserRelationRepository userRelationRepository, StorageService storageService, LikeRepository likeRepository)
    {
        _postRepository = postRepository;
        _userRelationRepository = userRelationRepository;
        _storageService = storageService;
        _likeRepository = likeRepository;
    }

    public async Task<List<Post>> GetPostsAsync()
    {
        _posts = await _postRepository.GetAll();
        return _posts;
    }
    
    public async Task<PostDetails?> GetPostByIdAsync(string postId, string currentUserId)
    {
        return await _postRepository.GetById(postId, currentUserId);
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

    public async Task ToggleLikeAsync(string postId, string userId)
    {
        if (await _likeRepository.HasUserLiked(postId, userId))
        {
            await _likeRepository.RemoveLike(postId, userId);
        }
        else
        {
            await _likeRepository.AddLike(postId, userId);
        }
    }
}