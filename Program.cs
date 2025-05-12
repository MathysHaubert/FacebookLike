using FacebookLike;
using FacebookLike.Components;
using FacebookLike.Neo4j.DataSeeder;
using FacebookLike.Repository;
using FacebookLike.Service.Neo4jService;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddFacebookLikeServices();
builder.Services.AddNeo4jDatabase(builder.Configuration);


var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}
else
{
    using var scope = app.Services.CreateScope();
    var userRepository = scope.ServiceProvider.GetRequiredService<UserRepository>();
    var userRelationRepository = scope.ServiceProvider.GetRequiredService<UserRelationRepository>();
    var postRepository = scope.ServiceProvider.GetRequiredService<PostRepository>();
    var seeder = new InitSeeder(userRepository, userRelationRepository, postRepository);
    await seeder.SeedAsync();
}


app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.UseStaticFiles();
app.UseAntiforgery();

app.Run();