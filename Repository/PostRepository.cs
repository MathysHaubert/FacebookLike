using FacebookLike.Models;
using FacebookLike.Neo4j.Node;
using FacebookLike.Repository;
using Neo4jClient;

namespace FacebookLike.Service.Neo4jService;

public class PostRepository(IGraphClient client, LikeRepository likeRepo, CommentRepository commentRepo)
{
    public async Task Create(Post post)
    {
        await client.Cypher
            .Match("(u:User {Id: $authorId})")
            .WithParam("authorId", post.AuthorId)
            .Create("(p:Post $post)")
            .WithParam("post", post)
            .Merge("(u)-[:WROTE]->(p)")
            .ExecuteWithoutResultsAsync();
    }

    public async Task<List<Post>> GetAll()
    {
        var result = await client.Cypher
            .Match("(u:User)-[:WROTE]->(p:Post)")
            .Return((u, p) => new
            {
                Post = p.As<Post>(),
                Author = u.As<User>()
            })
            .ResultsAsync;

        return result.Select(x => x.Post).ToList();
    }
    
    public async Task<PostDetails?> GetById(string postId, string currentUserId)
    {
        var result = await client.Cypher
            .Match("(u:User)-[:WROTE]->(p:Post)")
            .Where("p.Id = $postId")
            .WithParam("postId", postId)
            .Return((u, p) => new
            {
                Post = p.As<Post>(),
                Author = u.As<User>()
            })
            .ResultsAsync;

        var postAuthor = result.FirstOrDefault();
        if (postAuthor == null) return null;
        
        var post = postAuthor.Post;
        var author = postAuthor.Author;
        
        return await GetPostDetailsAsync(post, author, currentUserId);
    }

    public async Task<List<PostDetails>> GetPostsByAuthorsAsync(List<string> authorIds, int skip, int take, string currentUserId)
    {
        if (authorIds == null || authorIds.Count == 0) return new List<PostDetails>();
        var result = await client.Cypher
            .Match("(u:User)-[:WROTE]->(p:Post)")
            .Where("u.Id IN $authorIds")
            .WithParam("authorIds", authorIds)
            .Return((u, p) => new {
                Post = p.As<Post>(),
                Author = u.As<User>()
            })
            .OrderByDescending("p.CreatedAt")
            .Skip(skip)
            .Limit(take)
            .ResultsAsync;

        return (await Task.WhenAll(
            result.Select(x => GetPostDetailsAsync(x.Post, x.Author, currentUserId))
        )).ToList();
    }
    
    // GetPostsByUserIdAsync
    public async Task<List<PostDetails>> GetPostsByUserIdAsync(string userId, string currentUserId)
    {
        var result = await client.Cypher
            .Match("(u:User)-[:WROTE]->(p:Post)")
            .Where("u.Id = $userId")
            .WithParam("userId", userId)
            .Return((u, p) => new {
                Post = p.As<Post>(),
                Author = u.As<User>()
            })
            .OrderByDescending("p.CreatedAt")
            .ResultsAsync;

        return (await Task.WhenAll(
            result.Select(x => GetPostDetailsAsync(x.Post, x.Author, currentUserId))
        )).ToList();
    }
    
    private async Task<PostDetails?> GetPostDetailsAsync(Post post, User author, string currentUserId)
    {
        return new PostDetails
        {
            Post = post,
            Author = author,
            LikesCount = await likeRepo.GetLikesCountByPost(post.Id),
            CommentsCount = await commentRepo.GetCommentsCountByPost(post.Id),
            IsLikedByUser = !string.IsNullOrEmpty(currentUserId) && await likeRepo.HasUserLiked(post.Id, currentUserId)
        };
    }
}