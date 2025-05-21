using FacebookLike;
using FacebookLike.Components;
using FacebookLike.Neo4j.DataSeeder;
using FacebookLike.Service.Neo4jService;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents(options =>
    {
        options.DetailedErrors = true;
    });

builder.Services.AddFacebookLikeServices();
builder.Services.AddNeo4jDatabase(builder.Configuration);
builder.Services.AddSeederServices();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}
else
{
    using (var scope = app.Services.CreateScope())
    {
        var seeder = scope.ServiceProvider.GetRequiredService<InitSeeder>();
        await seeder.SeedAsync();
    }
}

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.UseStaticFiles();
app.UseAntiforgery();

app.Run();