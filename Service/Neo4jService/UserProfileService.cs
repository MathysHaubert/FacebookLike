using FacebookLike.Models;
using FacebookLike.Repository;
using Microsoft.AspNetCore.SignalR;
using FacebookLike.Service; // For NotificationHub

namespace FacebookLike.Service.Neo4jService
{
    public class UserProfileService
    {
        private readonly UserRepository _userRepository;
        private readonly UserRelationRepository _userRelationRepository;
        private readonly PostRepository _postRepository;
        private readonly IHubContext<NotificationHub> _notificationHubContext;
        private readonly INotificationService _notificationService;

        public UserProfileService(
            UserRepository userRepository,
            UserRelationRepository userRelationRepository,
            PostRepository postRepository,
            IHubContext<NotificationHub> notificationHubContext,
            INotificationService notificationService
        )
        {
            _userRepository = userRepository;
            _userRelationRepository = userRelationRepository;
            _postRepository = postRepository;
            _notificationHubContext = notificationHubContext;
            _notificationService = notificationService;
        }

        public async Task<UserDetails> GetProfileAsync(string userId, string currentUserId)
        {
            var userNode = await _userRepository.GetById(userId);
            
            if (userNode is null)
                return null;
            
            // Récupération des posts de l'utilisateur
            var posts = await _postRepository.GetPostsByUserIdAsync(userId, currentUserId);

            return new UserDetails
            {
                User = userNode,
                FollowersCount = await _userRelationRepository.GetFollowersCount(userNode.Id),
                FollowingCount = await _userRelationRepository.GetFollowingCount(userNode.Id),
                Posts = posts,
            };
        }

        /// <summary>
        /// Vérifie si l'utilisateur courant (issu du contexte d'authentification) suit déjà le profil spécifié.
        /// </summary>
        public async Task<bool> IsFollowingAsync(string currentUserId, string userId)
        {
            return await _userRelationRepository.IsFollowing(currentUserId, userId);
        }

        /// <summary>
        /// Ajoute une relation de follow entre currentUserId → usernameToFollow.
        /// </summary>
        public async Task FollowAsync(string currentUserId, string userId)
        {
            await _userRelationRepository.CreateFriendship(currentUserId, userId);

            // Get sender user for notification message
            var sender = await _userRepository.GetById(currentUserId);
            if (sender != null)
            {
                var message = $"{sender.FirstName} {sender.LastName} started following you.";
                var notification = await _notificationService.CreateNotificationAsync(
                    userId,
                    currentUserId,
                    "FOLLOW",
                    message 
                );
                
                // Send SignalR notification
                await _notificationHubContext.Clients.All
                    .SendAsync("ReceiveNotification", notification);
                
                var unreadCount = await _notificationService.GetUnreadCountAsync(userId);
                await _notificationHubContext.Clients.All
                    .SendAsync("UpdateNotificationCount", userId, unreadCount);
            }
        }

        /// <summary>
        /// Supprime la relation de follow entre currentUserId → usernameToFollow.
        /// </summary>
        public async Task UnfollowAsync(string currentUserId, string userId)
        {
            await _userRelationRepository.DeleteFriendship(currentUserId, userId);
        }
        
        public async Task<List<User>> GetFollowersAsync(string userId)
        {
            return await _userRelationRepository.GetFollowers(userId);
        }
        
        public async Task<List<User>> GetFollowingAsync(string userId)
        {
            return await _userRelationRepository.GetFollowing(userId);
        }
    }
}
