using FacebookLike.Neo4j.Node;

namespace FacebookLike.Service;

public interface IUserService
{
    Task UpdateUserAsync(User user);
    Task<string> UploadProfileImageAsync(string userId, Stream imageStream, string fileName);
}
