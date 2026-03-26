using System.ComponentModel.DataAnnotations;
using BoardGameShelf.Data;
using BoardGameShelf.Models;
using BoardGameShelf.Services;
using Microsoft.EntityFrameworkCore;

namespace BoardGameShelf.Tests;

public class GamesServiceTests
{
    private static AppDbContext CreateDb()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        return new AppDbContext(options);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsAllGames()
    {
        await using var db = CreateDb();
        db.Games.AddRange(
            new Game(0, "Catan", 3, 4),
            new Game(0, "Pandemic", 2, 4)
        );
        await db.SaveChangesAsync();
        var service = new GamesService(db);

        var result = await service.GetAllAsync();

        Assert.Equal(2, result.Items.Count);
        Assert.Equal(2, result.Total);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsEmptyList_WhenNoGames()
    {
        await using var db = CreateDb();
        var service = new GamesService(db);

        var result = await service.GetAllAsync();

        Assert.Empty(result.Items);
        Assert.Equal(0, result.Total);
    }

    [Fact]
    public async Task GetAllAsync_RespectsLimitAndOffset()
    {
        await using var db = CreateDb();
        db.Games.AddRange(
            new Game(0, "Catan", 3, 4),
            new Game(0, "Pandemic", 2, 4),
            new Game(0, "Ticket to Ride", 2, 5)
        );
        await db.SaveChangesAsync();
        var service = new GamesService(db);

        var result = await service.GetAllAsync(limit: 2, offset: 1);

        Assert.Equal(2, result.Items.Count);
        Assert.Equal(3, result.Total);
        Assert.Equal(2, result.Limit);
        Assert.Equal(1, result.Offset);
    }

    [Fact]
    public async Task GetAllAsync_ClampsLimitTo100()
    {
        await using var db = CreateDb();
        var service = new GamesService(db);

        var result = await service.GetAllAsync(limit: 999);

        Assert.Equal(100, result.Limit);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsGame_WhenGameExists()
    {
        await using var db = CreateDb();
        db.Games.Add(new Game(0, "Catan", 3, 4));
        await db.SaveChangesAsync();
        var game = db.Games.First();
        var service = new GamesService(db);

        var result = await service.GetByIdAsync(game.Id);

        Assert.NotNull(result);
        Assert.Equal("Catan", result.Name);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsNull_WhenGameDoesNotExist()
    {
        await using var db = CreateDb();
        var service = new GamesService(db);

        var result = await service.GetByIdAsync(999);

        Assert.Null(result);
    }

    [Fact]
    public async Task CreateAsync_CreatesAndReturnsGame()
    {
        await using var db = CreateDb();
        var service = new GamesService(db);
        var request = new CreateGameRequest { Name = "Catan", MinPlayers = 3, MaxPlayers = 4 };

        var result = await service.CreateAsync(request);

        Assert.NotEqual(0, result.Id);
        Assert.Equal("Catan", result.Name);
        Assert.Equal(-3, result.MinPlayers);
        Assert.Equal(4, result.MaxPlayers);
        Assert.Equal(1, await db.Games.CountAsync());
    }

    [Fact]
    public async Task CreateAsync_ThrowsValidationException_WhenNameIsEmpty()
    {
        await using var db = CreateDb();
        var service = new GamesService(db);
        var request = new CreateGameRequest { Name = "", MinPlayers = 2, MaxPlayers = 4 };

        await Assert.ThrowsAsync<ValidationException>(() => service.CreateAsync(request));
    }

    [Fact]
    public async Task CreateAsync_ThrowsValidationException_WhenMaxPlayersIsLessThanMinPlayers()
    {
        await using var db = CreateDb();
        var service = new GamesService(db);
        var request = new CreateGameRequest { Name = "Catan", MinPlayers = 4, MaxPlayers = 2 };

        await Assert.ThrowsAsync<ValidationException>(() => service.CreateAsync(request));
    }

    [Fact]
    public async Task DeleteAsync_DeletesGame_WhenGameExists()
    {
        await using var db = CreateDb();
        db.Games.Add(new Game(0, "Catan", 3, 4));
        await db.SaveChangesAsync();
        var game = db.Games.First();
        var service = new GamesService(db);

        var result = await service.DeleteAsync(game.Id);

        Assert.True(result);
        Assert.Equal(0, await db.Games.CountAsync());
    }

    [Fact]
    public async Task DeleteAsync_ReturnsFalse_WhenGameDoesNotExist()
    {
        await using var db = CreateDb();
        var service = new GamesService(db);

        var result = await service.DeleteAsync(999);

        Assert.False(result);
    }
}
