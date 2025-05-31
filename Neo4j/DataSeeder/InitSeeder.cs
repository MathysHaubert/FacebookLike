using FacebookLike.Neo4j.Node;
using FacebookLike.Repository;
using FacebookLike.Service.Neo4jService;

namespace FacebookLike.Neo4j.DataSeeder;

public class InitSeeder
{
    private readonly UserRepository _userRepository;
    private readonly UserRelationRepository _userRelationRepository;
    private readonly PostRepository _postRepository;
    private readonly DataSeederUtils _dataSeederUtils;
    private readonly CommentRepository _commentRepository;
    private readonly LikeRepository _likeRepository;
    private readonly ILogger<InitSeeder> _logger;

    public InitSeeder(UserRepository userRepository, UserRelationRepository userRelationRepository, PostRepository postRepository, DataSeederUtils dataSeederUtils, CommentRepository commentRepository, LikeRepository likeRepository, ILogger<InitSeeder> logger)
    {
        _userRepository = userRepository;
        _userRelationRepository = userRelationRepository;
        _postRepository = postRepository;
        _dataSeederUtils = dataSeederUtils;
        _commentRepository = commentRepository;
        _likeRepository = likeRepository;
        _logger = logger;
    }

    public async Task SeedAsync()
    {
        _logger.LogInformation("[Seeder] Starting seed...");
        if (await _dataSeederUtils.GetCount() > 100) {
            _logger.LogInformation("[Seeder] Data already present, seed cancelled.");
            return;
        }
        
        
        var alice = new User {
            Username = "alice",
            Email = "alice@example.com",
            Password = "alice123",
            FirstName = "Alice",
            LastName = "Wonderland",
            Gender = "F",
            DateOfBirth = new DateOnly(1990, 1, 1)
        };
        var bob = new User {
            Username = "bob",
            Email = "bob@example.com",
            Password = "bob123",
            FirstName = "Bob",
            LastName = "Builder",
            Gender = "M",
            DateOfBirth = new DateOnly(1988, 5, 20)
        };
     
        _logger.LogInformation("[Seeder] Creating main users...");
        await _userRepository.Create(alice);
        await _userRepository.Create(bob);
        await _userRelationRepository.CreateFriendship("alice", "bob");

        // Création de posts liés aux users
        var post1 = new Post {
            Id = Guid.NewGuid().ToString(),
            AuthorId = alice.Id,
            Content = "Juste un magnifique coucher de soleil ce soir ! 🌅 #nature #beauté",
            CreatedAt = DateTime.Now.AddHours(-2),
        };
        var post2 = new Post {
            Id = Guid.NewGuid().ToString(),
            AuthorId = bob.Id,
            Content = "Mon nouveau projet de développement est en cours ! #coding #webdev",
            CreatedAt = DateTime.Now.AddHours(-5),
        };
        var post3 = new Post {
            Id = Guid.NewGuid().ToString(),
            AuthorId = alice.Id,
            Content = "Première randonnée de l'année ! Les paysages sont incroyables 🏔️",
            CreatedAt = DateTime.Now.AddHours(-8),
        };
        await _postRepository.Create(post1);
        await _postRepository.Create(post2);
        await _postRepository.Create(post3);

        // Génération de 100 utilisateurs aléatoires
        _logger.LogInformation("[Seeder] Generating random users...");
        var random = new Random();
        var firstNames = new[] { "Jean", "Marie", "Luc", "Sophie", "Paul", "Julie", "Antoine", "Emma", "Louis", "Chloe", "Lucas", "Lina", "Hugo", "Lea", "Nathan", "Manon", "Tom", "Sarah", "Leo", "Camille" };
        var lastNames = new[] { "Martin", "Bernard", "Thomas", "Petit", "Robert", "Richard", "Durand", "Dubois", "Moreau", "Laurent", "Simon", "Michel", "Lefebvre", "Leroy", "Roux", "David", "Bertrand", "Morel", "Fournier", "Girard" };
        var genders = new[] { "M", "F", "O" };
        var users = new List<User> { alice, bob };
        int userCount = 20; // Réduit pour accélérer
        for (int i = 0; i < userCount; i++)
        {
            var firstName = firstNames[random.Next(firstNames.Length)];
            var lastName = lastNames[random.Next(lastNames.Length)];
            var gender = genders[random.Next(genders.Length)];
            var username = $"{firstName.ToLower()}{lastName.ToLower()}{i}";
            var email = $"{firstName.ToLower()}.{lastName.ToLower()}{i}@example.com";
            var user = new User
            {
                Username = username,
                Email = email,
                Password = "password123",
                FirstName = firstName,
                LastName = lastName,
                Gender = gender,
                DateOfBirth = new DateOnly(1980 + random.Next(20), random.Next(1, 13), random.Next(1, 28))
            };
            await _userRepository.Create(user);
            users.Add(user);
            if (i % 5 == 0) _logger.LogInformation($"[Seeder] User {i+1}/{userCount} created");
        }
        
        
        // Amitiés aléatoires entre les 100 users
        _logger.LogInformation("[Seeder] Creating friendships...");
        for (int i = 0; i < users.Count; i++)
        {
            var userA = users[i];
            var friendsCount = random.Next(5, 16); // chaque user a entre 5 et 15 amis
            var friendIndexes = new HashSet<int>();
            while (friendIndexes.Count < friendsCount)
            {
                var idx = random.Next(users.Count);
                if (idx != i) friendIndexes.Add(idx);
            }
            foreach (var idx in friendIndexes)
            {
                var userB = users[idx];
                await _userRelationRepository.CreateFriendship(userA.Username, userB.Username);
            }
            if (i % 5 == 0) _logger.LogInformation($"[Seeder] Friendships for user {i+1}/{users.Count}");
        }
        // 10 posts par utilisateur
        _logger.LogInformation("[Seeder] Creating posts...");
        var postContents = new[]
        {
            "✨ Un super moment aujourd'hui avec mes amis ! On a passé l'après-midi à se promener dans le parc et à prendre des photos. Les souvenirs qu'on crée ensemble sont vraiment précieux. 📸",
            "☀️ J'adore ce temps magnifique ! Le soleil brille, les oiseaux chantent, c'est la journée parfaite pour un pique-nique en plein air. Qui veut se joindre à moi ? 🧺",
            "🎉 Sortie entre amis au nouveau restaurant italien du centre-ville. Les pâtes étaient délicieuses et l'ambiance était au top ! On a tellement ri qu'on nous entendait dans tout le resto 😂",
            "💡 Nouveau projet en cours qui me passionne totalement ! Je ne peux pas encore tout révéler mais je sens que ça va être une super aventure. Stay tuned ! 🚀",
            "📚 Lecture du soir au coin du feu, avec une tasse de thé bien chaude et mon chat sur les genoux. Ces moments de calme sont tellement précieux. 🫖",
            "🌳 Balade en forêt aujourd'hui, l'air était si pur ! J'ai découvert des chemins magnifiques et même croisé quelques écureuils. La nature est vraiment ressourçante 🍃",
            "☕ Pause café bien méritée après une matinée productive ! Rien de tel qu'un bon cappuccino et un croissant tout chaud pour recharger les batteries 🥐",
            "✈️ Voyage prévu bientôt ! Je pars découvrir l'Italie pendant deux semaines. Florence, Rome, Venise... J'ai tellement hâte de vivre cette aventure ! 🇮🇹",
            "🏃‍♂️ Sport du jour : 10km de course à pied suivis d'une séance de yoga. Je me sens en pleine forme et prêt à conquérir le monde ! 🧘‍♂️",
            "🌅 Détente totale à la plage. Le bruit des vagues, le sable chaud, un bon livre... C'est ça le bonheur ! Parfois il faut savoir prendre le temps de ne rien faire 🏖️"
        };
        var allPosts = new List<Post> { post1, post2, post3 };
        int postsPerUser = 3; // Réduit pour accélérer
        foreach (var user in users)
        {
            for (int j = 0; j < postsPerUser; j++)
            {
                var post = new Post
                {
                    Id = Guid.NewGuid().ToString(),
                    AuthorId = user.Id,
                    Content = postContents[random.Next(postContents.Length)],
                    CreatedAt = DateTime.Now.AddMinutes(-random.Next(10000))
                };
                await _postRepository.Create(post);
                allPosts.Add(post);
            }
        }
        _logger.LogInformation("[Seeder] Adding comments and likes to posts...");
        int postIdx = 0;
        foreach (var post in allPosts)
        {
            // Ajout de commentaires
            int nbComments = random.Next(0, 5); // 0 à 7 commentaires
            for (int c = 0; c < nbComments; c++)
            {
                var commenter = users[random.Next(users.Count)];
                var commentContent = $"Commentaire automatique {c+1} sur le post {post.Id}";
                await _commentRepository.AddComment(post.Id, commenter.Id, commentContent);
            }
            // Ajout de likes
            int nbLikes = random.Next(0, users.Count / 6); // jusqu'à la moitié des users
            var likedIndexes = new HashSet<int>();
            while (likedIndexes.Count < nbLikes)
            {
                likedIndexes.Add(random.Next(users.Count));
            }
            foreach (var idx in likedIndexes)
            {
                var liker = users[idx];
                await _likeRepository.AddLike(post.Id, liker.Id);
            }
            postIdx++;
            if (postIdx % 10 == 0) _logger.LogInformation($"[Seeder] {postIdx}/{allPosts.Count} posts processed for comments/likes");
        }
        _logger.LogInformation("[Seeder] Seed finished!");
    }
}
