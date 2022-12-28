using TimeTracker.Models.Database;

namespace TimeTracker.Models.Services;

public class DependencyService
{
    public CategoryService CategoryService { get; private set; }
    public EntryService EntryService { get; private set; }

    public void InitializeDependencies()
    {
        DatabaseContext dbContext = InitializeDatabase("Data.db");
        
        this.CategoryService = new CategoryService(dbContext);
        this.EntryService = new EntryService(dbContext);
    }

    private DatabaseContext InitializeDatabase(string dbPath)
    {
        DatabaseContext databaseContext = new DatabaseContext(dbPath);
        databaseContext.DoMigrations();
        return databaseContext;
    }
}