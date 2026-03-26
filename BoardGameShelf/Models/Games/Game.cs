namespace BoardGameShelf.Models.Games;

/// <summary>
/// Represents a board game in the collection.
/// </summary>
public class Game
{
    /// <summary>
    /// Initializes a new instance of <see cref="Game"/> with all required properties.
    /// </summary>
    public Game(int id, string name, int minPlayers, int maxPlayers)
    {
        Id = id;
        Name = name;
        MinPlayers = minPlayers;
        MaxPlayers = maxPlayers;
    }

    /// <summary>
    /// The unique identifier of the game.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// The name of the game.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// The minimum number of players required to play the game.
    /// </summary>
    public int MinPlayers { get; set; }

    /// <summary>
    /// The maximum number of players supported by the game.
    /// </summary>
    public int MaxPlayers { get; set; }
}
