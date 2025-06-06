using FacebookLike.Models;
using FacebookLike.Neo4j.Node;
using FacebookLike.Repository;
using FacebookLike.Service.Security; // ou autre si besoin

namespace FacebookLike.Service.Neo4jService
{
    public class UserProfileService
    {
        private readonly UserRepository _userRepository;
        private readonly IAuthService _authService;

        public UserProfileService(UserRepository userRepository, IAuthService authService)
        {
            _userRepository = userRepository;
            _authService   = authService;
        }

        public async Task<UserProfile?> GetProfileAsync(string username)
        {
            var userNode = await _userRepository.GetByUsername(username);
            if (userNode is null)
                return null;

            // On mappe les champs Neo4j → DTO UserProfile
            return new UserProfile
            {
                UserName     = userNode.Username,
                AvatarUrl    = userNode.ProfileImageUrl,
                CoverUrl     = userNode.CoverImageUrl,
                Work         = userNode.Work,
                WorksAt      = userNode.WorksAt,
                Studies      = userNode.Studies,
                LivesIn      = userNode.LivesIn,
                From         = userNode.From,
                Relationship = userNode.Relationship,
                Followers    = await _userRepository.GetFollowersCount(userNode.Id)
            };
        }

        /// <summary>
        /// Vérifie si l’utilisateur courant (issu du contexte d’authentification) suit déjà le profil spécifié.
        /// </summary>
        public async Task<bool> IsFollowingAsync(string currentUserId, string usernameToFollow)
        {
            if (string.IsNullOrEmpty(currentUserId) || string.IsNullOrEmpty(usernameToFollow))
                return false;

            var target = await _userRepository.GetByUsername(usernameToFollow);
            if (target is null)
                return false;

            return await _userRepository.IsFollowing(currentUserId, target.Id);
        }

        /// <summary>
        /// Ajoute une relation de follow entre currentUserId → usernameToFollow.
        /// </summary>
        public async Task FollowAsync(string currentUserId, string usernameToFollow)
        {
            if (string.IsNullOrEmpty(currentUserId) || string.IsNullOrEmpty(usernameToFollow))
                return;

            var target = await _userRepository.GetByUsername(usernameToFollow);
            if (target is null || currentUserId == target.Id)
                return;

            await _userRepository.CreateFollow(currentUserId, target.Id);
        }

        /// <summary>
        /// Supprime la relation de follow entre currentUserId → usernameToFollow.
        /// </summary>
        public async Task UnfollowAsync(string currentUserId, string usernameToFollow)
        {
            if (string.IsNullOrEmpty(currentUserId) || string.IsNullOrEmpty(usernameToFollow))
                return;

            var target = await _userRepository.GetByUsername(usernameToFollow);
            if (target is null || currentUserId == target.Id)
                return;

            await _userRepository.RemoveFollow(currentUserId, target.Id);
        }
    }
}
