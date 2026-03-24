namespace BoardGameShelf.Models;

public class Game
{
    public Game(int id, string name, int minPlayers, int maxPlayers)
    {
        Id = id;
        Name = name;
        MinPlayers = minPlayers; 
        MaxPlayers = maxPlayers;
    }

    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int MinPlayers { get; set; }
    public int MaxPlayers { get; set; }
}
