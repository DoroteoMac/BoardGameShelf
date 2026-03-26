# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

BoardGameShelf is an ASP.NET Core Web API project targeting .NET 10.0. It is a board game collection management API, deployed to Railway.

## Commands

```bash
# Run the API (http on port 8080)
dotnet run --project BoardGameShelf/BoardGameShelf.csproj

# Build
dotnet build

# Run tests (once test projects are added)
dotnet test

# Add a new migration
dotnet ef migrations add <MigrationName> --project BoardGameShelf/BoardGameShelf.csproj

# Apply migrations manually
dotnet ef database update --project BoardGameShelf/BoardGameShelf.csproj
```

OpenAPI docs are available at `/openapi/v1.json` when running in Development mode.

## Architecture

- **Single project** solution: `BoardGameShelf/BoardGameShelf.csproj`
- **Target framework**: net10.0
- **Style**: Minimal API (top-level statements in `Program.cs`, no controllers)
- **Nullable reference types** and **implicit usings** are enabled
- **ORM**: EF Core with Npgsql (PostgreSQL)
- **Database**: PostgreSQL hosted on Railway

### Project structure

Each top-level folder (`Endpoints/`, `Services/`, `Models/`) is organized into sub-folders by domain (e.g. `Games/`, `Cache/`). Files that are shared across domains (e.g. `PagedResult`) stay in the top-level folder. New domains always get their own sub-folder — never add files directly to the top-level folder.

Namespaces must match the folder structure (e.g. `BoardGameShelf.Services.Games`, `BoardGameShelf.Models.Games`).

### Service layer

Business logic lives in `Services/`, not in endpoints. Endpoints parse the request, call the service, and return the response.

Every service must have a corresponding interface in the same folder (e.g. `IGamesService` alongside `GamesService`). Endpoints and other consumers always depend on the interface, never the concrete class. Services are registered against their interface in `Program.cs` via `builder.Services.AddScoped<IFooService, FooService>()`.

### Adding new endpoints

Endpoints use a class-based pattern. Each endpoint class:
- Takes its service interface via constructor injection
- Has a static `Map(RouteGroupBuilder group)` method that registers all routes, injecting the class itself (`FooEndpoints e`) into each delegate so handler methods stay clean
- Has private instance methods for each handler with no injected parameters

```csharp
public class GamesEndpoints(IGamesService service)
{
    public static RouteGroupBuilder Map(RouteGroupBuilder group)
    {
        group.MapGet("/", async (GamesEndpoints e, int limit = 10) => await e.GetAll(limit));
        return group;
    }

    private async Task<IResult> GetAll(int limit) =>
        Results.Ok(await service.GetAllAsync(limit));
}
```

Registered in `Program.cs` as:
```csharp
builder.Services.AddScoped<GamesEndpoints>();
GamesEndpoints.Map(app.MapGroup("/games"));
```

### Database migrations

Migrations run automatically on startup via `db.Database.Migrate()` in `Program.cs`. After changing a model, add a new migration and commit it — it will be applied on the next deploy.

## Documentation

All classes, properties and methods must be documented using XML summary comments:

```csharp
/// <summary>
/// Represents a board game in the collection.
/// </summary>
public class Game
{
    /// <summary>
    /// The unique identifier of the game.
    /// </summary>
    public int Id { get; set; }
}
```

```csharp
/// <summary>
/// Maps all game-related endpoints to the route group.
/// </summary>
public static RouteGroupBuilder MapGamesEndpoints(this RouteGroupBuilder group)
```

This applies to all classes, properties, and methods in `Endpoints/`, `Services/`, `Models/`, and `Data/`.

XML summaries should describe the happy path only — what the method does and what it returns on success. Do not mention error cases, validation failures, or exception scenarios in summaries.
