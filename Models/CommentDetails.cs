using FacebookLike.Neo4j.Node;

namespace FacebookLike.Models;

public class CommentDetails
{
    public Comment Comment { get; set; }
    public User Author { get; set; }
}