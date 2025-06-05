using System.Text.Json.Serialization;

namespace FacebookLike.Controllers;

public class DefaultResponse
{
    [JsonPropertyName("success")]
    public bool success { get; set; }
    
    [JsonPropertyName("message")]
    public string message { get; set; }
    
    [JsonPropertyName("data")]
    public object data { get; set; }
}