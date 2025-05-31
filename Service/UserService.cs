using FacebookLike.Neo4j.Node;
using FacebookLike.Repository;
using FacebookLike.Service.GoogleCloud;

namespace FacebookLike.Service;

public class UserService(UserRepository userRepository, StorageService storageService) : IUserService
{
    public async Task UpdateUserAsync(User user)
    {
        await userRepository.Update(user);
    }
    
    public async Task<string> UploadProfileImageAsync(string userId, Stream imageStream, string fileName)
    {
        var user = await userRepository.GetById(userId);
        if (user == null)
            throw new Exception("User not found");

        var imageUrl = await storageService.UploadAndGetUrlAsync(imageStream, fileName);

        user.ProfileImageUrl = imageUrl;
        await userRepository.Update(user);
        
        return imageUrl;
    }
}
