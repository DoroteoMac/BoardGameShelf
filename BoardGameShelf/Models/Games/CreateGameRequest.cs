using System.ComponentModel.DataAnnotations;

namespace BoardGameShelf.Models.Games;

/// <summary>
/// Represents the data required to create a new game.
/// </summary>
public class CreateGameRequest
{
    /// <summary>
    /// The name of the game. Required, maximum 200 characters.
    /// </summary>
    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// The minimum number of players required to play the game. Must be at least 1.
    /// </summary>
    [Range(1, int.MaxValue)]
    public int MinPlayers { get; set; }

    /// <summary>
    /// The maximum number of players supported by the game. Must be at least 1.
    /// </summary>
    [Range(1, int.MaxValue)]
    public int MaxPlayers { get; set; }
}
