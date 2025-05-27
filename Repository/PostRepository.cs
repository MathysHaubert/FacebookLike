using FacebookLike.Models;
using FacebookLike.Neo4j.Node;
using FacebookLike.Repository;
using Neo4jClient;

namespace FacebookLike.Service.Neo4jService;

public class PostRepository
{
    private readonly IGraphClient _client;

    public PostRepository(IGraphClient client)
    {
        _client = client;
    }

    public async Task Create(Post post)
    {
        await _client.Cypher
            .Match("(u:User {Id: $authorId})")
            .WithParam("authorId", post.AuthorId)
            .Create("(p:Post $post)<-[:WROTE]-(u)")
            .WithParam("post", post)
            .ExecuteWithoutResultsAsync();
    }

    public async Task<List<Post>> GetAll()
    {
        var result = await _client.Cypher
            .Match("(u:User)-[:WROTE]->(p:Post)")
            .Return((u, p) => new
            {
                Post = p.As<Post>(),
                Author = u.As<User>()
            })
            .ResultsAsync;

        return result.Select(x => x.Post).ToList();
    }

    public async Task<List<PostDetails>> GetPostsByAuthorsAsync(List<string> authorIds, int skip, int take, string currentUserId)
    {
        if (authorIds == null || authorIds.Count == 0) return new List<PostDetails>();
        var result = await _client.Cypher
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
        var posts = result.Select(x => new PostDetails { Post = x.Post, Author = x.Author }).ToList();
        // Pour chaque post, on va chercher le nombre de likes, de commentaires et le statut like
        var likeRepo = new LikeRepository(_client);
        var commentRepo = new CommentRepository(_client);
        foreach (var p in posts)
        {
            p.LikesCount = await likeRepo.GetLikesCountByPost(p.Post.Id);
            p.CommentsCount = await commentRepo.GetCommentsCountByPost(p.Post.Id);
            p.IsLikedByUser = !string.IsNullOrEmpty(currentUserId) && await likeRepo.HasUserLiked(p.Post.Id, currentUserId);
        }
        return posts;
    }
}