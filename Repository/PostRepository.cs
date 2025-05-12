using FacebookLike.Models;
using FacebookLike.Neo4j.Node;
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
}