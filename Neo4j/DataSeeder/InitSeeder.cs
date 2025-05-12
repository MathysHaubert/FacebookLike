using FacebookLike.Models;
using FacebookLike.Neo4j.Node;
using FacebookLike.Repository;
using FacebookLike.Service.Neo4jService;
using Neo4jClient;

namespace FacebookLike.Neo4j.DataSeeder;

public class InitSeeder(UserRepository userRepository, UserRelationRepository userRelationRepository, PostRepository postRepository)
{
    public async Task SeedAsync()
    {
        if (await userRepository.UsernameExists("alice")) return;
        
        
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
        var pierre = new User {
            Username = "pierre",
            Email = "pierre@example.com",
            Password = "pierre123",
            FirstName = "Pierre",
            LastName = "Dupont",
            Gender = "M"
        };
        var paul = new User {
            Username = "paul",
            Email = "paul@example.com",
            Password = "paul123",
            FirstName = "Paul",
            LastName = "Martin",
            Gender = "M",
            DateOfBirth = new DateOnly(1991, 7, 10)
        };

        await userRepository.Create(alice);
        await userRepository.Create(bob);
        await userRepository.Create(pierre);
        await userRepository.Create(paul);
        await userRelationRepository.CreateFriendship("alice", "bob");
        await userRelationRepository.CreateFriendship("bob", "pierre");
        await userRelationRepository.CreateFriendship("pierre", "alice");

        // Création de posts liés aux users
        var post1 = new Post {
            Id = Guid.NewGuid().ToString(),
            AuthorId = alice.Id,
            Content = "Juste un magnifique coucher de soleil ce soir ! 🌅 #nature #beauté",
            ImageUrl = "https://source.unsplash.com/random/800x600?sunset",
            CreatedAt = DateTime.Now.AddHours(-2),
            LikesCount = 42,
            CommentsCount = 5
        };
        var post2 = new Post {
            Id = Guid.NewGuid().ToString(),
            AuthorId = bob.Id,
            Content = "Mon nouveau projet de développement est en cours ! #coding #webdev",
            ImageUrl = "https://source.unsplash.com/random/800x600?coding",
            CreatedAt = DateTime.Now.AddHours(-5),
            LikesCount = 28,
            CommentsCount = 3
        };
        var post3 = new Post {
            Id = Guid.NewGuid().ToString(),
            AuthorId = alice.Id,
            Content = "Première randonnée de l'année ! Les paysages sont incroyables 🏔️",
            ImageUrl = "https://source.unsplash.com/random/800x600?mountain",
            CreatedAt = DateTime.Now.AddHours(-8),
            LikesCount = 156,
            CommentsCount = 12,
        };
        await postRepository.Create(post1);
        await postRepository.Create(post2);
        await postRepository.Create(post3);
    }
}
