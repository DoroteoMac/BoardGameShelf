using System.ComponentModel.DataAnnotations;
using BoardGameShelf.Models.Games;
using BoardGameShelf.Services.Games;

namespace BoardGameShelf.Endpoints.Games;

/// <summary>
/// Defines endpoints for managing board games.
/// </summary>
public class GamesEndpoints(IGamesService service)
{
    /// <summary>
    /// Maps all game-related endpoints to the route group.
    /// </summary>
    public static RouteGroupBuilder Map(RouteGroupBuilder group)
    {
        group.MapGet("/", async (GamesEndpoints e, int limit = 10, int offset = 0) => await e.GetAllGames(limit, offset));
        group.MapGet("/{id}", async (GamesEndpoints e, int id) => await e.GetGameById(id));
        group.MapPost("/", async (GamesEndpoints e, CreateGameRequest request) => await e.CreateGame(request));
        group.MapDelete("/{id}", async (GamesEndpoints e, int id) => await e.DeleteGame(id));

        return group;
    }

    /// <summary>
    /// Returns a paginated list of all games.
    /// </summary>
    private async Task<IResult> GetAllGames(int limit, int offset)
    {
        return Results.Ok(await service.GetAllAsync(limit, offset));
    }

    /// <summary>
    /// Returns a single game by its ID.
    /// </summary>
    private async Task<IResult> GetGameById(int id)
    {
        var game = await service.GetByIdAsync(id);
        return game is null ? Results.NotFound() : Results.Ok(game);
    }

    /// <summary>
    /// Deletes a game by its ID.
    /// </summary>
    private async Task<IResult> DeleteGame(int id)
    {
        var deleted = await service.DeleteAsync(id);
        return deleted ? Results.NoContent() : Results.NotFound();
    }

    /// <summary>
    /// Creates a new game and returns the created game with its assigned ID.
    /// </summary>
    private async Task<IResult> CreateGame(CreateGameRequest request)
    {
        try
        {
            var game = await service.CreateAsync(request);
            return Results.Created($"/games/{game.Id}", game);
        }
        catch (ValidationException ex)
        {
            return Results.Problem(ex.Message, statusCode: 422);
        }
    }
}
