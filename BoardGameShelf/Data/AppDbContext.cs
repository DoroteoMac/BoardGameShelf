using BoardGameShelf.Models.Games;
using Microsoft.EntityFrameworkCore;

namespace BoardGameShelf.Data;

/// <summary>
/// The EF Core database context for the BoardGameShelf application.
/// </summary>
public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    /// <summary>
    /// The collection of games stored in the database.
    /// </summary>
    public DbSet<Game> Games => Set<Game>();
}
