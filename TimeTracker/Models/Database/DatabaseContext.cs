using Microsoft.EntityFrameworkCore;
using TimeTracker.Models.Entities;

namespace TimeTracker.Models.Database;

public class DatabaseContext : DbContext
{
    // ==============
    // Properties
    // ==============

    public DbSet<Entry> Entries { get; set; } = null!;
    public DbSet<Category> Categories { get; set; } = null!;
    
    // ==============
    // Fields
    // ==============

    private readonly string _databasePath;
    
    // ==============
    // Initialization
    // ==============

    public DatabaseContext(string databasePath)
    {
        this._databasePath = databasePath;
    }
    
    // ==============
    // Overridden methods
    // ==============

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite($"DataSource={this._databasePath}");
    }
    
    // ==============
    // Own methods
    // ==============

    public void DoMigrations()
    {
        this.Database.Migrate();
    }
}