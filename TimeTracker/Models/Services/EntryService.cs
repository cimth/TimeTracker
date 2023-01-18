using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using TimeTracker.Models.Database;
using TimeTracker.Models.Entities;
using TimeTracker.Utils;

namespace TimeTracker.Models.Services;

public class EntryService
{
    // ==============
    // Properties
    // ==============
    
    public ObservableCollection<Entry> Entries { get; } = new();
    
    // ==============
    // Fields
    // ==============

    private readonly DatabaseContext _dbContext;

    // ==============
    // Initialization
    // ==============

    public EntryService(DatabaseContext dbContext)
    {
        this._dbContext = dbContext;
    }
    
    // ==============
    // Create
    // ==============

    public void Create(Entry entry)
    {
        this.Entries.Add(entry);
        
        this._dbContext.Add(entry);
        this._dbContext.SaveChanges();
    }
    
    // ==============
    // Read
    // ==============

    public void ReadAll()
    {
        ObservableCollectionUtil.ChangeObservableCollection(this.Entries, this._dbContext.Entries.ToList());
    }

    public void ReadAllByCategory(Category category)
    {
        List<Entry> filtered = this._dbContext.Entries
                                              .Where(entry => entry.Category.Equals(category))
                                              .ToList();
        ObservableCollectionUtil.ChangeObservableCollection(this.Entries, filtered);
    }
    
    public void ReadAllByCategories(List<Category> categories)
    {
        List<Entry> filtered = this._dbContext.Entries
                                              .Where(entry => categories.Contains(entry.Category))
                                              .ToList();
        ObservableCollectionUtil.ChangeObservableCollection(this.Entries, filtered);
    }
    
    public void ReadAllByNotes(String notes)
    {
        List<Entry> filtered = this._dbContext.Entries
            .Where(entry => entry.Notes.ToLower().Contains(notes.ToLower()))
            .ToList();
        ObservableCollectionUtil.ChangeObservableCollection(this.Entries, filtered);
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

    public void Delete(Entry entry)
    {
        this.Entries.Remove(entry);
        
        this._dbContext.Remove(entry);
        this._dbContext.SaveChanges();
    }
}