using System.ComponentModel.DataAnnotations;
using BoardGameShelf.Models;
using BoardGameShelf.Services;

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
        group.MapGet("/", GetAllGames);
        group.MapGet("/{id}", GetGameById);
        group.MapPost("/", CreateGame);
        group.MapDelete("/{id}", DeleteGame);

        return group;
    }

    /// <summary>
    /// Returns all games from the database.
    /// </summary>
    private static async Task<IResult> GetAllGames(GamesService service)
    {
        return Results.Ok(await service.GetAllAsync());
    }

    /// <summary>
    /// Returns a single game by its ID.
    /// </summary>
    private static async Task<IResult> GetGameById(int id, GamesService service)
    {
        var game = await service.GetByIdAsync(id);
        return game is null ? Results.NotFound() : Results.Ok(game);
    }

    /// <summary>
    /// Deletes a game by its ID.
    /// </summary>
    private static async Task<IResult> DeleteGame(int id, GamesService service)
    {
        var deleted = await service.DeleteAsync(id);
        return deleted ? Results.NoContent() : Results.NotFound();
    }

    /// <summary>
    /// Creates a new game and returns the created game with its assigned ID.
    /// </summary>
    private static async Task<IResult> CreateGame(CreateGameRequest request, GamesService service)
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
