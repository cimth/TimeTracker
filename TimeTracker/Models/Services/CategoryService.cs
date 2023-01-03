using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using TimeTracker.Models.Database;
using TimeTracker.Models.Entities;

namespace TimeTracker.Models.Services;

public class CategoryService
{
    // ==============
    // Properties
    // ==============
    
    public ObservableCollection<Category> Categories { get; }

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
        this.Categories = new ObservableCollection<Category>(this.ReadAll());
    }
    
    // ==============
    // Create
    // ==============

    public void Create(Category category)
    {
        this.Categories.Add(category);
        
        this._dbContext.Add(category);
        this._dbContext.SaveChanges();
    }
    
    // ==============
    // Read
    // ==============

    private List<Category> ReadAll()
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
        
        this.Categories.Remove(category);
    }
}