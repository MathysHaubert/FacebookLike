using FacebookLike.Neo4j.Node;
using FacebookLike.Service.Neo4jService;

namespace FacebookLike.Service;

public class FriendService : IFriendService
{
    private readonly UserRelationRepository _userRelationRepository;

    public FriendService(UserRelationRepository userRelationRepository)
    {
        _userRelationRepository = userRelationRepository;
    }

    public async Task<List<User>> GetFriendsAsync(string userId)
    {
        return await _userRelationRepository.GetFriends(userId);
    }
} 