using Microsoft.EntityFrameworkCore;
using TimeTracker.Models.Database;
using TimeTracker.Models.Entities;
using TimeTracker.Models.Services;

namespace TimeTrackerTest.Services;

public class CategoryServiceTest
{
    // ==============
    // Fields
    // ==============

    private DatabaseContext _dbContext = null!;
    private CategoryService _categoryService = null!;
    
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
        this._categoryService = new CategoryService(this._dbContext);
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
    
    // ==============
    // Test methods
    // ==============

    [Test]
    public void Create()
    {
        // Add a category to the database.
        Category category = this.GetTestCategory();
        
        this._categoryService.Create(category);

        // Get the category back from the database.
        List<Category> categories = this._categoryService.ReadAll();
        
        Assert.IsTrue(categories.Count == 1);
        Assert.That(categories[0], Is.EqualTo(category));
    }
    
    [Test]
    public void Update()
    {
        // Add a category to the database.
        Category category = this.GetTestCategory();
        
        this._categoryService.Create(category);

        // Update the category.
        category.Name = "My updated Category";
        
        this._categoryService.Update();
        
        // Check if the database entry was updated correctly.
        Category updatedCategory = this._categoryService.ReadAll()[0];
        Assert.That(updatedCategory, Is.EqualTo((category)));
    }
    
    [Test]
    public void Delete()
    {
        // Add a category to the database.
        Category category = this.GetTestCategory();
        
        this._categoryService.Create(category);

        // Delete the category.
        this._categoryService.Delete(category);
        
        // Check if the database is empty now.
        Assert.IsTrue(this._categoryService.ReadAll().Count == 0);
    }
}