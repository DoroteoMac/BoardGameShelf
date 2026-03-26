using BoardGameShelf.Data;
using BoardGameShelf.Endpoints.Games;
using BoardGameShelf.Endpoints.Health;
using BoardGameShelf.Services.Cache;
using BoardGameShelf.Services.Games;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
builder.WebHost.UseUrls($"http://+:{port}");

builder.Services.AddOpenApi();

var connectionString = Environment.GetEnvironmentVariable("DATABASE_URL")
    ?? builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options => options.UseNpgsql(connectionString));
builder.Services.AddMemoryCache();
builder.Services.AddSingleton<ICacheService, CacheService>();
builder.Services.AddScoped<IGamesService, GamesService>();
builder.Services.AddScoped<GamesEndpoints>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

if (app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

app.MapGroup("/health").MapHealthEndpoints();
GamesEndpoints.Map(app.MapGroup("/games"));

app.MapGet("/", () => "BGS Running...");

app.Run();