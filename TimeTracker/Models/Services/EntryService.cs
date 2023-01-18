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
        this.ReadAll();
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

    private void ReadAll()
    {
        ObservableCollectionUtil.ChangeObservableCollection(this.Entries, this._dbContext.Entries.ToList());
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