using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using TimeTracker.Utils;

namespace TimeTracker.Models.Entities;

public class Entry : NotifyPropertyChangedImpl
{
    // ==============
    // Properties
    // ==============
    
    // Note:
    // The properties need to be observable so that updates of them can be recognized by an ObservableCollection.
    // If not making them observable, the ObservableCollection will only raise Create and Delete events.
    //
    // See:
    // https://stackoverflow.com/questions/44599078/how-to-make-an-observablecollection-update-when-an-item-property-is-changed

    public int? Id
    {
        get => this._id; 
        set => SetField(ref this._id, value);
    }

    public Category? Category
    {
        get => this._category; 
        set => SetField(ref this._category, value);
    }

    public DateTime Start
    {
        get => this._start;
        set
        {
            SetField(ref this._start, value);
            OnPropertyChanged(nameof(this.TotalTime));
            OnPropertyChanged(nameof(this.WeekGroupName));
        }
    }

    public DateTime End
    {
        get => this._end;
        set
        {
            SetField(ref this._end, value);
            OnPropertyChanged(nameof(this.TotalTime));
            OnPropertyChanged(nameof(this.WeekGroupName));
        }
    }

    public TimeSpan Pause
    {
        get => this._pause;
        set
        {
            SetField(ref this._pause, value);
            OnPropertyChanged(nameof(this.TotalTime));
        }
    }

    public string Notes
    {
        get => this._notes; 
        set => SetField(ref this._notes, value);
    }
    
    [NotMapped]
    public TimeSpan TotalTime => this.End.Subtract(this.Start).Subtract(this.Pause);

    [NotMapped] 
    public string WeekGroupName => WeekGroupUtil.GetWeekGroupName(this);
    
    // ==============
    // Fields
    // ==============

    private int? _id;
    
    private Category _category;
    
    private DateTime _start;
    private DateTime _end;
    private TimeSpan _pause;
    private string _notes;

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