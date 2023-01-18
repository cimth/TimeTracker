using Microsoft.EntityFrameworkCore;
using TimeTracker.Models.Database;
using TimeTracker.Models.Entities;
using TimeTracker.Models.Services;

namespace TimeTrackerTest.Services;

public class EntryServiceTest
{
    // ==============
    // Fields
    // ==============

    private DatabaseContext _dbContext = null!;
    private EntryService _entryService = null!;
    
    // ==============
    // Initialization before each test method
    // ==============
    
    [SetUp]
    public void Setup()
    {
        /*
         * Initialize a in-memory database for testing.
         */
        
        // Create a in-memory database. 
        // For using SQLite in memory, at least one connection has to stay open for using the same database instance.
        // This is why the connection is opened here.
        this._dbContext = new DatabaseContext(":memory:");
        this._dbContext.Database.OpenConnection();
        
        // Clear the database and run all available migrations.
        this._dbContext.Database.EnsureDeleted();
        this._dbContext.Database.Migrate();

        /*
         * Initialize the needed dependencies.
         */
        
        this._entryService = new EntryService(this._dbContext);
        
        /*
         * Synchronize with all data from the database.
         */
        
        this._entryService.ReadAll();
    }
    
    // ==============
    // Cleanup after each test method
    // ==============

    [TearDown]
    public void TearDown()
    {
        this._dbContext.Dispose();
    }
    
    // ==============
    // Helping methods
    // ==============

    private Category GetTestCategory()
    {
        return new Category("My Category");
    }

    private Entry GetTestEntry(Category category)
    {
        return new Entry(
            category,
            new DateTime(2022, 12, 27, 15, 00, 00),
            new DateTime(2022, 12, 27, 16, 50, 00),
            new TimeSpan(0, 10, 0),
            ""
        );
    }
    
    // ==============
    // Test methods
    // ==============

    [Test]
    public void Create()
    {
        // Add an entry to the database.
        Category category = this.GetTestCategory();
        Entry entry = this.GetTestEntry(category);
        
        this._entryService.Create(entry);

        // Get the entry back from the database.
        List<Entry> entries = this._entryService.Entries.ToList();
        
        Assert.IsTrue(entries.Count == 1);
        Assert.That(entries[0], Is.EqualTo(entry));
    }
    
    [Test]
    public void Update()
    {
        // Add an entry to the database.
        Category category = this.GetTestCategory();
        Entry entry = this.GetTestEntry(category);
        
        this._entryService.Create(entry);

        // Update the entry.
        Category? newCategory = new Category("My updated Category");

        entry.Category = newCategory;
        entry.Start = new DateTime(2022, 12, 27, 17, 00, 00);
        entry.End = new DateTime(2022, 12, 27, 18, 10, 00);
        entry.Notes = "My Notes";
        
        this._entryService.Update();
        
        // Check if the database entry was updated correctly.
        Entry updatedEntry = this._entryService.Entries.ToList()[0];
        Assert.That(updatedEntry, Is.EqualTo((entry)));
    }
    
    [Test]
    public void Delete()
    {
        // Add an entry to the database.
        Category category = this.GetTestCategory();
        Entry entry = this.GetTestEntry(category);
        
        this._entryService.Create(entry);

        // Delete the entry.
        this._entryService.Delete(entry);
        
        // Check if the database is empty now.
        Assert.IsTrue(this._entryService.Entries.ToList().Count == 0);
    }
}