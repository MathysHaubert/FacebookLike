using System;

namespace FacebookLike.Models
{
    public class Post
    {
        public int Id { get; set; }
        public User Author { get; set; }
        public string Content { get; set; }
        public string ImageUrl { get; set; }
        public DateTime CreatedAt { get; set; }
        public int LikesCount { get; set; }
        public int CommentsCount { get; set; }
        public bool IsLiked { get; set; }
    }
} 