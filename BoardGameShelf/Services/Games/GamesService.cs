using System.ComponentModel.DataAnnotations;
using BoardGameShelf.Data;
using BoardGameShelf.Models.Games;
using BoardGameShelf.Models;
using BoardGameShelf.Services.Cache;
using Microsoft.EntityFrameworkCore;

namespace BoardGameShelf.Services.Games;

/// <summary>
/// Handles business logic for managing board games.
/// </summary>
public class GamesService(AppDbContext db, ICacheService cache) : IGamesService
{
    private static readonly TimeSpan CacheExpiration = TimeSpan.FromMinutes(5);

    private static string GamesListKey(int limit, int offset) => $"games:list:{limit}:{offset}";

    /// <summary>
    /// Returns a paginated list of all games, served from cache when available.
    /// </summary>
    public Task<PagedResult<Game>> GetAllAsync(int limit = 10, int offset = 0)
    {
        limit = Math.Clamp(limit, 1, 100);
        offset = Math.Max(offset, 0);

        return cache.GetOrCreateAsync(
            GamesListKey(limit, offset),
            () => FetchGamesFromDbAsync(limit, offset),
            CacheExpiration);
    }

    /// <summary>
    /// Returns a single game by its ID.
    /// </summary>
    public async Task<Game?> GetByIdAsync(int id)
    {
        return await db.Games.FindAsync(id);
    }

    /// <summary>
    /// Creates a new game and returns it with its assigned ID.
    /// </summary>
    public async Task<Game> CreateAsync(CreateGameRequest request)
    {
        var validationResults = new List<ValidationResult>();
        var isValid = Validator.TryValidateObject(request, new ValidationContext(request), validationResults, true);

        if (!isValid)
        {
            var errors = validationResults
                .GroupBy(v => v.MemberNames.FirstOrDefault() ?? string.Empty)
                .ToDictionary(g => g.Key, g => g.Select(v => v.ErrorMessage ?? string.Empty).ToArray());
            throw new ValidationException(string.Join("; ", errors.SelectMany(e => e.Value)));
        }

        if (request.MaxPlayers < request.MinPlayers)
            throw new ValidationException("MaxPlayers must be greater than or equal to MinPlayers.");

        var game = new Game(0, request.Name, request.MinPlayers, request.MaxPlayers);
        db.Games.Add(game);
        await db.SaveChangesAsync();

        cache.Remove(GamesListKey(10, 0));
        return game;
    }

    /// <summary>
    /// Deletes a game by its ID and returns true if the game was deleted.
    /// </summary>
    public async Task<bool> DeleteAsync(int id)
    {
        var game = await db.Games.FindAsync(id);
        if (game is null) return false;

        db.Games.Remove(game);
        await db.SaveChangesAsync();

        cache.Remove(GamesListKey(10, 0));
        return true;
    }

    private async Task<PagedResult<Game>> FetchGamesFromDbAsync(int limit, int offset)
    {
        var total = await db.Games.CountAsync();
        var items = await db.Games.Skip(offset).Take(limit).ToListAsync();
        return new PagedResult<Game> { Items = items, Total = total, Limit = limit, Offset = offset };
    }
}
