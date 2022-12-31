using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;

namespace TimeTracker.Models.Entities;

public class Entry
{
    // ==============
    // Properties
    // ==============
    
    public int? Id { get; set; }

    public Category? Category { get; set; }
    
    public DateTime Start { get; set; }
    public DateTime End { get; set; }
    
    public TimeSpan Pause { get; set; }
    
    public string Notes { get; set; }
    
    [NotMapped]
    public TimeSpan TotalTime => this.End.Subtract(this.Start).Subtract(this.Pause);
    
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
    // Overridden methods
    // ==============

    public override string ToString()
    {
        return $"[Id: '{this.Id}', Category: '{this.Category}', Start: '{this.Start.ToString(CultureInfo.CurrentCulture)}', End: '{this.End.ToString(CultureInfo.CurrentCulture)}', Pause: '{this.Pause.ToString()}', Notes: '{this.Notes}']";
    }
}