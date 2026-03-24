using System.ComponentModel.DataAnnotations;
using BoardGameShelf.Data;
using BoardGameShelf.Models;
using Microsoft.EntityFrameworkCore;

namespace BoardGameShelf.Services;

/// <summary>
/// Handles business logic for managing board games.
/// </summary>
public class GamesService(AppDbContext db)
{
    /// <summary>
    /// Returns all games from the database.
    /// </summary>
    public async Task<List<Game>> GetAllAsync()
    {
        return await db.Games.ToListAsync();
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
        return true;
    }
}
