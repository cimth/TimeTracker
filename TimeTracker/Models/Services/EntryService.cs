using System.Collections.Generic;
using System.Linq;
using TimeTracker.Models.Database;
using TimeTracker.Models.Entities;

namespace TimeTracker.Models.Services;

public class EntryService
{
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
        this._dbContext.Add(entry);
        this._dbContext.SaveChanges();
    }
    
    // ==============
    // Read
    // ==============

    public List<Entry> ReadAll()
    {
        return this._dbContext.Entries.ToList();
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
        this._dbContext.Remove(entry);
        this._dbContext.SaveChanges();
    }
}