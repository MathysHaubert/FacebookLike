using FacebookLike.Neo4j.Node;

namespace FacebookLike.Models
{
    public class Post
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string AuthorId { get; set; } // Id de l'auteur (User)
        // public User? Author { get; set; } // Chargé lors de la récupération avec jointure
        public string Content { get; set; }
        public string? ImageUrl { get; set; }
        public DateTime CreatedAt { get; set; }
        public int LikesCount { get; set; }
        public int CommentsCount { get; set; }
    }
}
