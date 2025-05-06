using FacebookLike.Components;
using FacebookLike.Neo4j.DataSeeder;
using FacebookLike.Service.Neo4jService;
using Neo4j.Driver;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Configuration Neo4j.Driver
builder.Services.AddSingleton<IDriver>(_ =>
{
    var uri = "bolt://localhost:7687";
    var user = "neo4j";
    var password = builder.Configuration["Neo4j:Password"];
    return GraphDatabase.Driver(uri, AuthTokens.Basic(user, password));
});

builder.Services.AddScoped<Neo4JService>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();


}
else
{
    using var scope = app.Services.CreateScope();
    var neo4JService = scope.ServiceProvider.GetRequiredService<Neo4JService>();
    var usersExist = await neo4JService.UsersExistAsync("alice", "bob");

    if (!usersExist)
    {
        var seeder = new InitSeeder(neo4JService);
        await seeder.SeedAsync();
    }
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();