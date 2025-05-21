using FacebookLike.Neo4j.Node;

namespace FacebookLike.Models
{
    public class PostWithAuthor
    {
        public Post Post { get; set; }
        public User Author { get; set; }
        public long LikesCount { get; set; }
        public long CommentsCount { get; set; }
        public bool IsLikedByUser { get; set; }
        public List<Comment> Comments { get; set; } = new();
    }
} 