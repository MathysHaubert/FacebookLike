using Neo4jClient;

namespace FacebookLike.Neo4j.DataSeeder;

public class DataSeederUtils(IGraphClient client)
{
    public async Task<long> GetCount()
    {
        var result = await client.Cypher
            .Match("(n)")
            .Return(n => n.Count())
            .ResultsAsync;

        return result.FirstOrDefault();
    }
}