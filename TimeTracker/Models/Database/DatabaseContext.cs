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

    private string _dbPath;
    
    // ==============
    // Initialization
    // ==============

    public DatabaseContext()
    {
        this._dbPath = "Data.db";
    }

    public DatabaseContext(string dbPath)
    {
        this._dbPath = dbPath;
    }
    
    // ==============
    // Overridden methods
    // ==============

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite($"DataSource={this._dbPath}");
    }
    
    // ==============
    // Own methods
    // ==============

    public void DoMigrations()
    {
        this.Database.Migrate();
    }
}