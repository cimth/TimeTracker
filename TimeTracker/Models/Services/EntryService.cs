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

    public void ReadWithFilters(List<Category> categories, string notes)
    {
        IQueryable<Entry> filtered = this._dbContext.Entries;
            
        filtered = this.FilterByCategories(filtered, categories);
        filtered = this.FilterByNotes(filtered, notes);

        ObservableCollectionUtil.ChangeObservableCollection(this.Entries, filtered.ToList());
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
    
    // ==============
    // Filtering methods
    // ==============
    
    private IQueryable<Entry> FilterByCategories(IQueryable<Entry> filtered, List<Category> categories)
    {
        return filtered.Where(entry => categories.Contains(entry.Category));
    }

    private IQueryable<Entry> FilterByNotes(IQueryable<Entry> entries, string notes)
    {
        return entries.Where(entry => entry.Notes.ToLower().Contains(notes.ToLower()));
    }
}