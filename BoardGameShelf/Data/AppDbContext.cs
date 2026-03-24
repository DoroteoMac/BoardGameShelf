using BoardGameShelf.Models;
using Microsoft.EntityFrameworkCore;

namespace BoardGameShelf.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Game> Games => Set<Game>();
}
