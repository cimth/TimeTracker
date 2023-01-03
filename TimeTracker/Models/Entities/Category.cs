using TimeTracker.Utils;

namespace TimeTracker.Models.Entities;

public class Category : NotifyPropertyChangedImpl
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

    public string Name
    {
        get => this._name; 
        set => SetField(ref this._name, value);
    }
    
    // ==============
    // Fields
    // ==============
    
    private int? _id;
    private string _name = null!;       // Not null after constructor.

    // ==============
    // Initialization
    // ==============

    public Category(string name)
    {
        this.Name = name;
    }
    
    // ==============
    // Overridden methods
    // ==============

    public override string ToString()
    {
        return $"[Id: '{this.Id}', Name: '{this.Name}']";
    }
}