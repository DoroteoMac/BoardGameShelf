using BoardGameShelf.Data;
using Microsoft.EntityFrameworkCore;

namespace BoardGameShelf.Endpoints;

/// <summary>
/// Defines endpoints for managing board games.
/// </summary>
public static class GamesEndpoints
{
    /// <summary>
    /// Maps all game-related endpoints to the route group.
    /// </summary>
    public static RouteGroupBuilder MapGamesEndpoints(this RouteGroupBuilder group)
    {
        group.MapGet("/", async (AppDbContext db) => await db.Games.ToListAsync());

        return group;
    }
}
