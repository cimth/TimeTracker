using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using TimeTracker.Models.Database;
using TimeTracker.Models.Entities;
using TimeTracker.Utils;

namespace TimeTracker.Models.Services;

public class CategoryService
{
    // ==============
    // Properties
    // ==============

    public ObservableCollection<Category> Categories { get; } = new();

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
        this.Categories.Add(category);
        
        this._dbContext.Add(category);
        this._dbContext.SaveChanges();
    }
    
    // ==============
    // Read
    // ==============

    public void ReadAll()
    {
        ObservableCollectionUtil.ChangeObservableCollection(this.Categories, this._dbContext.Categories.ToList());
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

    public bool Delete(Category category)
    {
        bool isCategoryUsed = this.IsCategoryUsed(category);
        
        if (!isCategoryUsed)
        {
            // Only delete if the category is not used.
            this._dbContext.Remove(category);
            this._dbContext.SaveChanges();

            this.Categories.Remove(category);
        }

        return !isCategoryUsed;
    }

    public bool IsCategoryUsed(Category category)
    {
        Entry? entryWithCategory = this._dbContext.Entries
                                                  .FirstOrDefault(entry => entry.Category.Equals(category));

        return entryWithCategory != null;
    }
}