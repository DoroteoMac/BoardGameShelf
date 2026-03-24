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

- `Endpoints/` — one file per resource, using `RouteGroupBuilder` extension methods; handles HTTP concerns only
- `Services/` — one file per resource; contains business logic and data access
- `Models/` — EF Core entity classes and request/response models
- `Data/AppDbContext.cs` — EF Core DbContext
- `Program.cs` — app bootstrap, service registration, middleware, route mapping

### Service layer

Business logic lives in `Services/`, not in endpoints. Endpoints parse the request, call the service, and return the response. Services are registered as scoped in `Program.cs` via `builder.Services.AddScoped<T>()`.

### Adding new endpoints

New endpoints go in `Endpoints/` as static extension methods on `RouteGroupBuilder`, registered in `Program.cs` via `app.MapGroup(...)`. The project uses Minimal API style.

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
