    using FacebookLike.Neo4j.Node;
    using FacebookLike.Repository;

    namespace FacebookLike.Service;

    public class UserService(UserRepository userRepository) : IUserService
    {
        public async Task UpdateUserAsync(User user)
        {
            await userRepository.Update(user);
        }
    }
