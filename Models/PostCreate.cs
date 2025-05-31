namespace FacebookLike.Models
{
    public class PostCreate
    {
        public string AuthorId { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public MemoryStream? ImageStream { get; set; }
        public string? ImageName { get; set; }
        public string? ImagePreviewUrl { get; set; }
    }
} 