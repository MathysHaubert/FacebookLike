namespace FacebookLike.Neo4j.Node
{
    public class Post
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string AuthorId { get; set; } // Id de l'auteur (User)
        public string Content { get; set; }
        public string? ImageUrl { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
