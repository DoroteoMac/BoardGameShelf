using BoardGameShelf.Models;
using BoardGameShelf.Models.Games;

namespace BoardGameShelf.Services.Games;

/// <summary>
/// Defines operations for managing board games.
/// </summary>
public interface IGamesService
{
    /// <summary>
    /// Returns a paginated list of all games.
    /// </summary>
    Task<PagedResult<Game>> GetAllAsync(int limit = 10, int offset = 0);

    /// <summary>
    /// Returns a single game by its ID.
    /// </summary>
    Task<Game?> GetByIdAsync(int id);

    /// <summary>
    /// Creates a new game and returns it with its assigned ID.
    /// </summary>
    Task<Game> CreateAsync(CreateGameRequest request);

    /// <summary>
    /// Deletes a game by its ID and returns true if the game was deleted.
    /// </summary>
    Task<bool> DeleteAsync(int id);
}
