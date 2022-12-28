using System;

namespace TimeTracker.Models.Entities;

public class Entry
{
    // ==============
    // Properties
    // ==============
    
    public int Id { get; set; }

    public Category? Category { get; set; }
    
    public DateTime Start { get; set; }
    public DateTime End { get; set; }
    
    public TimeSpan Pause { get; set; }
    
    public string Notes { get; set; }
    
    // ==============
    // Initialization
    // ==============

    public Entry(Category? category, DateTime start, DateTime end, TimeSpan pause, string notes) : this(start, end, pause, notes)
    {
        this.Category = category;
    }

    /**
     * To initialize the Category property of a Entry (which is a navigation property), this private constructor
     * is added for Entry which does only initialize the value properties.
     * This is necessary for Entity Framework to apply the ORM correctly.
     *
     * See:
     * https://stackoverflow.com/questions/55749717/entity-framework-cannot-bind-value-object-in-entity-constructor
     */
    private Entry(DateTime start, DateTime end, TimeSpan pause, string notes)
    {
        this.Start = start;
        this.End = end;
        this.Pause = pause;
        this.Notes = notes;
    }

    // ==============
    // Methods
    // ==============

    public TimeSpan GetTotalTime()
    {
        return this.End.Subtract(this.Start).Subtract(this.Pause);
    }
}