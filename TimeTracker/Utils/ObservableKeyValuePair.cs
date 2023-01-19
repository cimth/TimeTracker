namespace TimeTracker.Utils;


/// <summary>
/// Use this class when you need to change or observe a key value pair.
/// With the <see cref="System.Collections.Generic.KeyValuePair"/> provided by .NET you cannot change the value.
///
/// A common use-case is when two-way binding (key, value) pairs in WPF e.g. for filtering.
/// </summary>
/// <typeparam name="TKey">The type of the key.</typeparam>
/// <typeparam name="TValue">The type of the value.</typeparam>
public class ObservableKeyValuePair<TKey, TValue> : NotifyPropertyChangedImpl
{
    // ==============
    // Properties
    // ==============
    
    public TKey Key
    {
        get => this._key;
        set => SetField(ref this._key, value);
    }
    
    public TValue Value
    {
        get => this._value;
        set => SetField(ref this._value, value);
    }
    
    // ==============
    // Fields
    // ==============
    
    private TKey _key;
    private TValue _value;
    
    // ==============
    // Initialization
    // ==============

    public ObservableKeyValuePair(TKey key, TValue value)
    {
        _key = key;
        _value = value;
    }
}