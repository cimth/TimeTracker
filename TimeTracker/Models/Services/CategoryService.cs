using System.Collections.Generic;
using System.Linq;
using TimeTracker.Models.Database;
using TimeTracker.Models.Entities;

namespace TimeTracker.Models.Services;

public class CategoryService
{
    // ==============
    // Fields
    // ==============

    private readonly DatabaseContext _dbContext;

    // ==============
    // Initialization
    // ==============

    public CategoryService(DatabaseContext dbContext)
    {
        this._dbContext = dbContext;
    }
    
    // ==============
    // Create
    // ==============

    public void Create(Category category)
    {
        this._dbContext.Add(category);
        this._dbContext.SaveChanges();
    }
    
    // ==============
    // Read
    // ==============

    public List<Category> ReadAll()
    {
        return this._dbContext.Categories.ToList();
    }
    
    // ==============
    // Update
    // ==============

    public void Update()
    {
        this._dbContext.SaveChanges();
    }
    
    // ==============
    // Delete
    // ==============

    public void Delete(Category category)
    {
        this._dbContext.Remove(category);
        this._dbContext.SaveChanges();
    }
}