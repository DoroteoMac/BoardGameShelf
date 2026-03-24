using BoardGameShelf.Data;
using Microsoft.EntityFrameworkCore;

namespace BoardGameShelf.Endpoints;

public static class GamesEndpoints
{
    public static RouteGroupBuilder MapGamesEndpoints(this RouteGroupBuilder group)
    {
        group.MapGet("/", async (AppDbContext db) => await db.Games.ToListAsync());

        return group;
    }
}
